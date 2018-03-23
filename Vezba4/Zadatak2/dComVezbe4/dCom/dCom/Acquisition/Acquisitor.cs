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

            int DOcnt1 = cr.GetAcquisitionInterval("DigOut1");
            int DOcnt2 = cr.GetAcquisitionInterval("DigOut2");

            int DIcnt1 = cr.GetAcquisitionInterval("DigIn1");
            int DIcnt2 = cr.GetAcquisitionInterval("DigIn2");

            int AOcnt1 = cr.GetAcquisitionInterval("AnaOut1");
            int AOcnt2 = cr.GetAcquisitionInterval("AnaOut2");

            int AIcnt1 = cr.GetAcquisitionInterval("AnaIn1");
            int AIcnt2 = cr.GetAcquisitionInterval("AnaIn2");
            while (true)
            {
                if (cnt % DOcnt1 == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_COILS, cr.GetStartAddress("DigOut1"), cr.GetNumberOfRegisters("DigOut1"));
                    ModbusFunction mf = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(mf);

                }
                if (cnt % DOcnt2 == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_COILS, cr.GetStartAddress("DigOut2"), cr.GetNumberOfRegisters("DigOut2"));
                    ModbusFunction mf = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(mf);
                }
                if (cnt % DIcnt1 == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_DISCRETE_INPUTS, cr.GetStartAddress("DigIn1"), cr.GetNumberOfRegisters("DigIn1"));
                    ModbusFunction mf = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(mf);
                }
                if (cnt % DIcnt2 == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_DISCRETE_INPUTS, cr.GetStartAddress("DigIn2"), cr.GetNumberOfRegisters("DigIn2"));
                    ModbusFunction mf = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(mf);
                }
                if (cnt % AOcnt1 == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_HOLDING_REGISTERS, cr.GetStartAddress("AnaOut1"), cr.GetNumberOfRegisters("AnaOut1"));
                    ModbusFunction mf = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(mf);
                }
                if (cnt % AOcnt2 == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_HOLDING_REGISTERS, cr.GetStartAddress("AnaOut2"), cr.GetNumberOfRegisters("AnaOut2"));
                    ModbusFunction mf = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(mf);
                }
                if (cnt % AIcnt1 == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_INPUT_REGISTERS, cr.GetStartAddress("AnaIn1"), cr.GetNumberOfRegisters("AnaIn1"));
                    ModbusFunction mf = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(mf);
                }
                if (cnt % AIcnt2 == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_INPUT_REGISTERS, cr.GetStartAddress("AnaIn2"), cr.GetNumberOfRegisters("AnaIn2"));
                    ModbusFunction mf = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(mf);
                }

                try
                {


                    cnt++;
                    // TODO implement
                    acquisitionTrigger.WaitOne();

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