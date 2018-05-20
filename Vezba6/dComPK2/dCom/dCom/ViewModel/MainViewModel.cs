using Common;
using dCom.Configuration;
using Modbus.Acquisition;
using Modbus.Connection;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Collections.Generic;
using ProcessingModule;

namespace dCom.ViewModel
{
	internal class MainViewModel : ViewModelBase, IDisposable, IStateUpdater, IStorage
	{
		public ObservableCollection<BasePointItem> Points { get; set; }

		private object lockObj = new object();

		#region Fields

		private object lockObject = new object();
		private Thread timerWorker;
		private ConnectionState connectionState;
		private Modbus.Acquisition.Acquisitor acquisitor;
		private AutoResetEvent acquisitionTrigger = new AutoResetEvent(false);
		private TimeSpan elapsedTime = new TimeSpan();
		private Dispatcher dispather = Dispatcher.CurrentDispatcher;
		private string logText;
		private StringBuilder logBuilder;
		private DateTime currentTime;
		private IFunctionExecutor commandExecutor;
		private IAutomationManager automationManager;
		private bool timerThreadStopSignal = true;
		private bool disposed = false;
		IConfiguration configuration;
		#endregion Fields

		Dictionary<int, IPoint> pointsCache = new Dictionary<int, IPoint>();

		#region Properties

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
				if(connectionState == ConnectionState.CONNECTED)
				{
					automationManager.Start();
				}
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

		#endregion Properties

		public MainViewModel()
		{
			configuration = new ConfigReader();
			commandExecutor = new FunctionExecutor(this, configuration, lockObj);
			this.acquisitor = new Acquisitor(acquisitionTrigger, this.commandExecutor, this, configuration);
			this.automationManager = new AutomationManager(this, commandExecutor, configuration);
			InitializePointCollection();
			InitializeAndStartThreads();
			logBuilder = new StringBuilder();
			ConnectionState = ConnectionState.DISCONNECTED;
			Thread.CurrentThread.Name = "Main Thread";
		}

		#region Private methods

		private void InitializePointCollection()
		{
			Points = new ObservableCollection<BasePointItem>();
			foreach (var c in configuration.GetConfigurationItems())
			{
				for (int i = 0; i < c.NumberOfRegisters; i++)
				{
					BasePointItem pi = CreatePoint(c, i, this.commandExecutor);
					if (pi != null)
					{
						Points.Add(pi);
						pointsCache.Add(pi.PointId, pi);
					}
				}
			}
		}

		private BasePointItem CreatePoint(IConfigItem c, int i, IFunctionExecutor commandExecutor)
		{
			switch (c.RegistryType)
			{
				case PointType.DIGITAL_INPUT:
					return new DigitalInput(c, commandExecutor, this, configuration, i);

				case PointType.DIGITAL_OUTPUT:
					return new DigitalOutput(c, commandExecutor, this, configuration, i);

				case PointType.ANALOG_INPUT:
					return new AnalaogInput(c, commandExecutor, this, configuration, i);

				case PointType.ANALOG_OUTPUT:
					return new AnalogOutput(c, commandExecutor, this, configuration, i);

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

		/// <summary>
		/// Timer thread:
		///		Refreshes timers on UI and signalizes to acquisition thread that one second has elapsed
		/// </summary>
		private void TimerWorker_DoWork()
		{
			while (timerThreadStopSignal)
			{
				if (disposed)
					return;

				CurrentTime = DateTime.Now;
				ElapsedTime = ElapsedTime.Add(new TimeSpan(0, 0, 1));
				acquisitionTrigger.Set();
				Thread.Sleep(1000);
			}
		}

		#endregion Private methods

		#region IStateUpdater implementation

		public void UpdateConnectionState(ConnectionState currentConnectionState)
		{
			dispather.Invoke((Action)(() =>
			{
				ConnectionState = currentConnectionState;
			}));
		}

		public void LogMessage(string message)
		{
			if (disposed)
				return;

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

		#endregion IStateUpdater implementation

		public void Dispose()
		{
			disposed = true;
			timerThreadStopSignal = false;
			(commandExecutor as IDisposable).Dispose();
			this.acquisitor.Dispose();
			acquisitionTrigger.Dispose();
			automationManager.Stop();
		}

		public List<IPoint> GetPoints(List<Tuple<ushort, ushort>> pointIds)
		{
			List<IPoint> retVal = new List<IPoint>(pointIds.Count);
			lock (this.lockObj)
			{
				foreach (var pid in pointIds)
				{
					int id = PointIdentifierHelper.GetNewPointId(pid.Item1, pid.Item2);
					IPoint p = null;
					if (pointsCache.TryGetValue(id, out p))
					{
						retVal.Add(p);
					}
				}
			}
			return retVal;
		}
	}
}