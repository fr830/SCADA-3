using dCom.Configuration;
using dCom.Connection;
using dCom.Modbus.ModbusFunctions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace dCom.ViewModel
{
	public class MainViewModel : ViewModelBase, IDisposable, IStateUpdater
	{
		public ObservableCollection<BasePointItem> Points { get; set; }

		private BackgroundWorker timerWorker;
		private ConnectionState connectionState;

		TimeSpan elapsedTime = new TimeSpan();

		Dispatcher dispather = Dispatcher.CurrentDispatcher;

		private string logText;

		private StringBuilder logBuilder;

		public DateTime CurrentTime
		{
			get
			{
				return currentTime;
			}

			set
			{
				currentTime = value;
				OnPropertyChanged("CurrentTime");
			}
		}

		public ConnectionState ConnectionState
		{
			get
			{
				return connectionState;
			}

			set
			{
				connectionState = value;
				OnPropertyChanged("ConnectionState");
			}
		}

		public string LogText
		{
			get
			{
				return logText;
			}

			set
			{
				logText = value;
				OnPropertyChanged("LogText");
			}
		}

		public TimeSpan ElapsedTime
		{
			get
			{
				return elapsedTime;
			}

			set
			{
				elapsedTime = value;
				OnPropertyChanged("ElapsedTime");
			}
		}

		private DateTime currentTime;

		private FunctionExecutor commandExecutor;

		public MainViewModel()
		{
			commandExecutor = new FunctionExecutor(this);
			InitializePointCollection();
			InitializeAndStartTimerThread();
			logBuilder = new StringBuilder();
			ConnectionState = ConnectionState.DISCONNECTED;
		}

		private void InitializePointCollection()
		{
			Points = new ObservableCollection<BasePointItem>();
			foreach (var c in ConfigReader.Instance.GetConfigurationItems())
			{
				for (int i = 0; i < c.NumberOfRegisters; i++)
				{
					BasePointItem pi = CreatePoint(c, i, this.commandExecutor);
					if (pi != null)
						Points.Add(pi);
				}
			}
		}

		private BasePointItem CreatePoint(ConfigItem c, int i, FunctionExecutor commandExecutor)
		{
			switch (c.RegistryType)
			{
				case PointType.DI_REG:
					return new DigitalInput(c.RegistryType, (ushort)(c.StartAddress + i), c.DefaultValue, commandExecutor, string.Format("{0} [{1}]", c.Description, i), this, c.MinValue, c.MaxValue);
				case PointType.DO_REG:
					return new DigitalOutput(c.RegistryType, (ushort)(c.StartAddress + i), c.DefaultValue, commandExecutor, string.Format("{0} [{1}]", c.Description, i), this, c.MinValue, c.MaxValue);
				case PointType.IN_REG:
					return new AnalaogInput(c.RegistryType, (ushort)(c.StartAddress + i), c.DefaultValue, commandExecutor, string.Format("{0} [{1}]", c.Description, i), this, c.MinValue, c.MaxValue);
				case PointType.HR_INT:
					return new AnalogOutput(c.RegistryType, (ushort)(c.StartAddress + i), c.DefaultValue, commandExecutor, string.Format("{0} [{1}]", c.Description, i), this, c.MinValue, c.MaxValue);
				default:
					return null;
			}
		}

		private void InitializeAndStartTimerThread()
		{
			InitializeTimerThread();
			StartTimerThread();
		}

		private void InitializeTimerThread()
		{
			timerWorker = new BackgroundWorker();
			timerWorker.WorkerSupportsCancellation = true;
			timerWorker.DoWork += TimerWorker_DoWork;
		}

		private void StartTimerThread()
		{
			timerWorker.RunWorkerAsync();
		}

		private void TimerWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				CurrentTime = DateTime.Now;
				ElapsedTime = ElapsedTime.Add(new TimeSpan(0, 0, 1));
				Thread.Sleep(1000);
			}
		}

		public void Dispose()
		{
			timerWorker.CancelAsync();
			timerWorker.Dispose();
			commandExecutor.Dispose();
		}

		public void UpdateConnectionState(ConnectionState currentConnectionState)
		{

			dispather.BeginInvoke((Action)(() =>
			{
				ConnectionState = currentConnectionState;
			}));
		}

		public void HandleCommandInProgress(ModbusFunction currentCommand)
		{
			LogMessage($"A {currentCommand.GetType().Name} command is in progress!");
		}

		public void LogMessage(string message)
		{
			logBuilder.Append($"{message}{Environment.NewLine}");
			dispather.BeginInvoke((Action)(() =>
			{
				LogText = logBuilder.ToString();
			}));
		}
	}
}
