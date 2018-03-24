using dCom.Configuration;
using dCom.Connection;
using dCom.Modbus;
using dCom.Modbus.FunctionParameters;
using dCom.Modbus.ModbusFunctions;
using dCom.ViewModel;
using System;
using System.Threading;

namespace dCom.Acquisition
{
	public class Acquisitor : IDisposable
	{
		private AutoResetEvent acquisitionTrigger;
		private FunctionExecutor commandExecutor;
		private Thread acquisitionWorker;
		private IStateUpdater stateUpdater;
		private bool acquisitionStopSignal = true;

		public Acquisitor(AutoResetEvent acquisitionTrigger, FunctionExecutor commandExecutor, IStateUpdater stateUpdater)
		{
			this.stateUpdater = stateUpdater;
			this.acquisitionTrigger = acquisitionTrigger;
			this.commandExecutor = commandExecutor;
			this.InitializeAcquisitionThread();
			this.StartAcquisitionThread();
		}

		#region Private Methods

		private void InitializeAcquisitionThread()
		{
			this.acquisitionWorker = new Thread(Acquisition_DoWork);
			this.acquisitionWorker.Name = "Acquisition thread";
		}

		private void StartAcquisitionThread()
		{
			acquisitionWorker.Start();
		}

		/// <summary>
		/// Acquisition thread
		///		Awaits for trigger;
		///		After configured period send appropriate command to MdbSim for each point type
		/// </summary>
		private void Acquisition_DoWork()
		{
            int cnt = 0;

            ConfigReader cr = ConfigReader.Instance;

            int DOCnt = cr.GetAcquisitionInterval("DigOut");
            int AOCnt = cr.GetAcquisitionInterval("AnaOut");

            while (true)
            {
                if (cnt % DOCnt == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_COILS, cr.GetStartAddress("DigOut"), cr.GetNumberOfRegisters("DigOut"));
                    ModbusFunction mf = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(mf);
                }

                if (cnt % AOCnt == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_HOLDING_REGISTERS, cr.GetStartAddress("AnaOut"), cr.GetNumberOfRegisters("AnaOut"));
                    ModbusFunction mf = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(mf);
                }

                try
                {
                    acquisitionTrigger.WaitOne();
                    cnt++;

                }
                catch (Exception ex)
                {
                    string message = $"{ex.TargetSite.ReflectedType.Name}.{ex.TargetSite.Name}: {ex.Message}";
                    stateUpdater.LogMessage(message);
                }
            }
        }

		#endregion Private Methods

		public void Dispose()
		{
			acquisitionStopSignal = false;
		}
	}
}