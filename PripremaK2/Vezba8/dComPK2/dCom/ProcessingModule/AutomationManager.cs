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
                new Tuple<ushort, ushort>((ushort)PointType.DIGITAL_INPUT,(ushort)1000),
                new Tuple<ushort, ushort>((ushort)PointType.DIGITAL_INPUT,(ushort)1001),
                new Tuple<ushort, ushort>((ushort)PointType.ANALOG_OUTPUT,(ushort)3000),
                new Tuple<ushort, ushort>((ushort)PointType.ANALOG_OUTPUT,(ushort)3001),
            };
            List<IPoint> points = storage.GetPoints(tuples);

            IConfigItem item = configuration.GetConfigurationItems().Find(x => x.StartAddress == 3000);


            while (!shutDown)
			{
                

                if (points[0].RawValue == 0 && points[1].RawValue == 1 &&
                   points[2].RawValue >= EGUConverter.ConvertToRaw(item.A,item.B,2000) &&
                   points[3].RawValue <= EGUConverter.ConvertToRaw(item.A, item.B, 1500))
                {
                    commandExecutor.CreateAndExecuteFunction(ModbusFunctionCode.WRITE_SINGLE_COIL, 40, 1);
                    commandExecutor.CreateAndExecuteFunction(ModbusFunctionCode.WRITE_SINGLE_REGISTER, 3000, (ushort)(points[2].RawValue - EGUConverter.ConvertToRaw(item.A, item.B, 10)));
                    commandExecutor.CreateAndExecuteFunction(ModbusFunctionCode.WRITE_SINGLE_REGISTER, 3001, (ushort)(points[3].RawValue + EGUConverter.ConvertToRaw(item.A, item.B, 10)));
                }


                Thread.Sleep(2000); // don't burn CPU
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
