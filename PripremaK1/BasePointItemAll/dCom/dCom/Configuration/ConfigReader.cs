using dCom.Exceptions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dCom.Configuration
{
	public class ConfigReader
	{
		private ushort transactionId = 0;

		private static ConfigReader instance;

		public static ConfigReader Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ConfigReader();
				}

				return instance;
			}
		}

		private byte unitAddress;
		private int tcpAddress;
		private ConfigItemEqualityComparer confItemEqComp = new ConfigItemEqualityComparer();

		private Dictionary<string, ConfigItem> pointTypeToConfiguration = new Dictionary<string, ConfigItem>();

		private string path = "RtuCfg.txt";

		private ConfigReader()
		{
			if (!File.Exists(path))
			{
				OpenConfigFile();
			}

			ReadConfiguration();
		}

		public int GetAcquisitionInterval(string pointDescription)
		{
			ConfigItem ci;
			if (pointTypeToConfiguration.TryGetValue(pointDescription, out ci))
			{
				return ci.AcquisitionInterval;
			}
			throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointDescription)));
		}

		public ushort GetStartAddress(string pointDescription)
		{
			ConfigItem ci;
			if (pointTypeToConfiguration.TryGetValue(pointDescription, out ci))
			{
				return ci.StartAddress;
			}
			throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointDescription)));
		}

		public ushort GetNumberOfRegisters(string pointDescription)
		{
			ConfigItem ci;
			if (pointTypeToConfiguration.TryGetValue(pointDescription, out ci))
			{
				return ci.NumberOfRegisters;
			}
			throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointDescription)));
		}

		private void OpenConfigFile()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Multiselect = false;
			dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			dlg.FileOk += Dlg_FileOk;
			dlg.ShowDialog();
		}

		private void Dlg_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			path = (sender as OpenFileDialog).FileName;
		}

		private void ReadConfiguration()
		{
			using (TextReader tr = new StreamReader(path))
			{
				string s = string.Empty;
				while ((s = tr.ReadLine()) != null)
				{
					string[] splited = s.Split(' ', '\t');
					List<string> filtered = splited.ToList().FindAll(t => !string.IsNullOrEmpty(t));
					if(filtered.Count == 0)
					{
						continue;
					}
					if (s.StartsWith("STA"))
					{
						unitAddress = Convert.ToByte(filtered[filtered.Count - 1]);
						continue;
					}
					if (s.StartsWith("TCP"))
					{
						TcpAddress = Convert.ToInt32(filtered[filtered.Count - 1]);
						continue;
					}
					if (filtered.Count == 10)
					{
						try
						{
							ConfigItem ci = new ConfigItem(filtered);
							if (pointTypeToConfiguration.Count > 0)
							{
								foreach (ConfigItem cf in pointTypeToConfiguration.Values)
								{
									if (!confItemEqComp.Equals(cf, ci))
									{
										pointTypeToConfiguration.Add(ci.Description, ci);
										break;
									}
								}
							}
							else
							{
								pointTypeToConfiguration.Add(ci.Description, ci);
							}
						}
						catch (ArgumentException argEx)
						{
							throw new ConfigurationException($"Configuration error: {argEx.Message}", argEx);
						}
						catch (Exception ex)
						{
							throw ex;
						}
					}
					else if(filtered.Count != 10)
					{
						throw new ConfigurationException("Some of configuration parameters are missing! Check RtuCfg.txt file!");
					}
				}
				if(pointTypeToConfiguration.Count == 0)
				{
					throw new ConfigurationException("Configuration error! Check RtuCfg.txt file!");
				}
			}
		}

		public ushort GetTransactionId()
		{
			return transactionId++;
		}

		public byte UnitAddress
		{
			get
			{
				return unitAddress;
			}

			private set
			{
				unitAddress = value;
			}
		}

		public int TcpAddress
		{
			get
			{
				return tcpAddress;
			}

			private set
			{
				tcpAddress = value;
			}
		}

		public List<ConfigItem> GetConfigurationItems()
		{
			return new List<ConfigItem>(pointTypeToConfiguration.Values);
		}
	}
}