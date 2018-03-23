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


		private List<ConfigItem> configurationItems = new List<ConfigItem>(16);
		private byte unitAddress;
		private int tcpAddress;

		private string path = "RtuCfg.txt";
		private ConfigReader()
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
					string[] splited = s.Split(' ');
					List<string> filtered = splited.ToList().FindAll(t => !string.IsNullOrEmpty(t));
					if (s.StartsWith("STA"))
					{
						unitAddress = Convert.ToByte(filtered[filtered.Count - 1]);
					}
					if (s.StartsWith("TCP"))
					{
						TcpAddress = Convert.ToInt32(filtered[filtered.Count - 1]);
					}
					if (filtered.Count == 9)
					{
						ConfigItem ci = new ConfigItem(filtered);
						configurationItems.Add(ci);
					}
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
			return new List<ConfigItem>(configurationItems);
		}
	}
}
