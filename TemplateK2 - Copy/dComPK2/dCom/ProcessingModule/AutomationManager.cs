using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessingModule
{
	public class AutomationManager : IAutomationManager
    {
		private Thread automationWorker;
		private IStorage storage;
		private IFunctionExecutor commandExecutor;
        private IConfiguration configuration;


        public AutomationManager(IStorage storage, IFunctionExecutor commandExecutor, IConfiguration configuration)
		{
			this.storage = storage;
			this.commandExecutor = commandExecutor;
            this.configuration = configuration;
		}

		private void InitializeAndStartThreads()
		{
			InitializeAutomationWorkerThread();
			StartAutomationThread();
		}

		private void InitializeAutomationWorkerThread()
		{
			automationWorker = new Thread(AutomationWorker_DoWork);
			automationWorker.Name = "Automation Thread";
		}

		private void StartAutomationThread()
		{
			automationWorker.Start();
		}

		private void AutomationWorker_DoWork()
		{
            List<Tuple<ushort, ushort>> tuples = new List<Tuple<ushort, ushort>>()
            {
                new Tuple<ushort, ushort>((ushort)PointType.DIGITAL_OUTPUT,2000),
                new Tuple<ushort, ushort>((ushort)PointType.DIGITAL_OUTPUT,2001),
                new Tuple<ushort, ushort>((ushort)PointType.ANALOG_OUTPUT,1000)
            };

            List<IPoint> points = storage.GetPoints(tuples);

            IConfigItem item = configuration.GetConfigurationItems().Find(x => x.StartAddress == 1000);

			while (!shutDown)
			{
                ushort value = points[2].RawValue;

                if (points[0].RawValue == 1)
                    value -= EGUConverter.ConvertToRaw(item.A, item.B, 10);

                if (points[1].RawValue == 1)
                    value -= EGUConverter.ConvertToRaw(item.A, item.B, 15);

                if (value != points[2].RawValue)
                    commandExecutor.CreateAndExecuteFunction(ModbusFunctionCode.WRITE_SINGLE_REGISTER, 1000, value);

                Thread.Sleep(1000); // don't burn CPU
                // TODO implement
            }
        }

		#region IDisposable Support
		private bool shutDown = false;

		public void Start()
		{
            InitializeAndStartThreads();
		}

		public void Stop()
		{
			shutDown = true;
		}
		#endregion
	}
}
