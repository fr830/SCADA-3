using dCom.Acquisition;
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
		private object lockObject = new object();

		public ObservableCollection<BasePointItem> Points { get; set; }

		private Thread timerWorker;
		private ConnectionState connectionState;
		private Acquisitor acquisitor;

		private AutoResetEvent acquisitionEvent = new AutoResetEvent(false);

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
			this.acquisitor = new Acquisitor(acquisitionEvent, this.commandExecutor, this);
			InitializePointCollection();
			InitializeAndStartThreads();
			logBuilder = new StringBuilder();
			ConnectionState = ConnectionState.DISCONNECTED;
			Thread.CurrentThread.Name = "Main Thread";
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
				case PointType.DIGITAL_INPUT:
					return new DigitalInput(c.RegistryType, (ushort)(c.StartAddress + i), c.DefaultValue, commandExecutor, string.Format("{0} [{1}]", c.Description, i), this, c.MinValue, c.MaxValue);
				case PointType.DIGITAL_OUTPUT:
					return new DigitalOutput(c.RegistryType, (ushort)(c.StartAddress + i), c.DefaultValue, commandExecutor, string.Format("{0} [{1}]", c.Description, i), this, c.MinValue, c.MaxValue);
				case PointType.ANALOG_INPUT:
					return new AnalaogInput(c.RegistryType, (ushort)(c.StartAddress + i), c.DefaultValue, commandExecutor, string.Format("{0} [{1}]", c.Description, i), this, c.MinValue, c.MaxValue);
				case PointType.ANALOG_OUTPUT:
					return new AnalogOutput(c.RegistryType, (ushort)(c.StartAddress + i), c.DefaultValue, commandExecutor, string.Format("{0} [{1}]", c.Description, i), this, c.MinValue, c.MaxValue);
				default:
					return null;
			}
		}

		private void InitializeAndStartThreads()
		{
			InitializeTimerThread();
			StartTimerThread();
		}

		private void InitializeTimerThread()
		{
			timerWorker = new Thread(TimerWorker_DoWork);
			timerWorker.Name = "Timer Thread";
		}

		private void StartTimerThread()
		{
			timerWorker.Start();
		}

		private void TimerWorker_DoWork()
		{
			while (true)
			{
				CurrentTime = DateTime.Now;
				ElapsedTime = ElapsedTime.Add(new TimeSpan(0, 0, 1));
				acquisitionEvent.Set();
				Thread.Sleep(1000);
			}
		}

		public void Dispose()
		{
			timerWorker.Abort();
			commandExecutor.Dispose();
			this.acquisitor.Dispose();
			acquisitionEvent.Dispose();
		}

		public void UpdateConnectionState(ConnectionState currentConnectionState)
		{

			dispather.Invoke((Action)(() =>
			{
				ConnectionState = currentConnectionState;
			}));
		}

		public void LogMessage(string message)
		{
			string threadName = Thread.CurrentThread.Name;

			dispather.Invoke((Action)(() =>
			{
				lock (lockObject)
				{
					logBuilder.Append($"{DateTime.Now} [{threadName}]: {message}{Environment.NewLine}");
					LogText = logBuilder.ToString();
				}
			}));
		}
	}
}
