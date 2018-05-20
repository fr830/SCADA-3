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
			while (!shutDown)
			{
                Thread.Sleep(50); // don't burn CPU
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
