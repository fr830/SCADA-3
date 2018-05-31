using Common;
using dCom.Exceptions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dCom.Configuration
{
	internal class ConfigReader : IConfiguration
	{
		private ushort transactionId = 0;

		private byte unitAddress;
		private int tcpPort;
		private ConfigItemEqualityComparer confItemEqComp = new ConfigItemEqualityComparer();

		private Dictionary<string, IConfigItem> pointTypeToConfiguration = new Dictionary<string, IConfigItem>();

		private string path = "RtuCfg.txt";

		public ConfigReader()
		{
			if (!File.Exists(path))
			{
				OpenConfigFile();
			}

			ReadConfiguration();
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
					if (filtered.Count == 0)
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
						TcpPort = Convert.ToInt32(filtered[filtered.Count - 1]);
						continue;
					}
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
				if (pointTypeToConfiguration.Count == 0)
				{
					throw new ConfigurationException("Configuration error! Check RtuCfg.txt file!");
				}
			}
		}


		public int TcpPort
		{
			get
			{
				return tcpPort;
			}

			private set
			{
				tcpPort = value;
			}
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

        public ushort GetTransactionId()
        {
            return transactionId++;
        }

        public List<IConfigItem> GetConfigurationItems()
        {
            return new List<IConfigItem>(pointTypeToConfiguration.Values);
        }

        public ushort GetStartAddress(string pointDescription)
        {
            IConfigItem ci;
            if (pointTypeToConfiguration.TryGetValue(pointDescription, out ci))
            {
                return ci.StartAddress;
            }
            throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointDescription)));
        }

        public ushort GetNumberOfRegisters(string pointDescription)
        {
            IConfigItem ci;
            if (pointTypeToConfiguration.TryGetValue(pointDescription, out ci))
            {
                return ci.NumberOfRegisters;
            }
            throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointDescription)));
        }

        public int GetAcquisitionInterval(string pointDescription)
        {
            IConfigItem ci;
            if (pointTypeToConfiguration.TryGetValue(pointDescription, out ci))
            {
                return ci.AcquisitionInterval;
            }
            throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointDescription)));
        }


	}
}