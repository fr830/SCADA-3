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
		/// 
		///		Kao uslov za while petlju korititi acquisitionStopSignal da bi se akvizicioni thread ugasio kada se aplikacija ugasi
		/// </summary>
		private void Acquisition_DoWork()
		{
            ConfigReader cr = ConfigReader.Instance;

            int cnt = 0;

            int DO = cr.GetAcquisitionInterval("DigOut");
            int DI = cr.GetAcquisitionInterval("DigIn");
            int AO = cr.GetAcquisitionInterval("AnaOut");
            int AI = cr.GetAcquisitionInterval("AnaIn");
            int DO1 = cr.GetAcquisitionInterval("DigOut");
            int DI1 = cr.GetAcquisitionInterval("DigIn");
            int AO1 = cr.GetAcquisitionInterval("AnaOut");
            int AI1 = cr.GetAcquisitionInterval("AnaIn");

            while (true)
            {
                if(cnt % DO == 0)
                {
                    ModbusReadCommandParameters para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_COILS, cr.GetStartAddress("DigOut"), cr.GetNumberOfRegisters("DigOut"));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % DI == 0)
                {
                    ModbusReadCommandParameters para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_DISCRETE_INPUTS, cr.GetStartAddress("DigIn"), cr.GetNumberOfRegisters("DigIn"));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % AO == 0)
                {
                    ModbusReadCommandParameters para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_HOLDING_REGISTERS, cr.GetStartAddress("AnaOut"), cr.GetNumberOfRegisters("AnaOut"));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % AI == 0)
                {
                    ModbusReadCommandParameters para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_INPUT_REGISTERS, cr.GetStartAddress("AnaIn"), cr.GetNumberOfRegisters("AnaIn"));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % DO1 == 0)
                {
                    ModbusReadCommandParameters para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_COILS, cr.GetStartAddress("DigOut1"), cr.GetNumberOfRegisters("DigOut1"));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % DI1 == 0)
                {
                    ModbusReadCommandParameters para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_DISCRETE_INPUTS, cr.GetStartAddress("DigIn1"), cr.GetNumberOfRegisters("DigIn1"));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % AO1 == 0)
                {
                    ModbusReadCommandParameters para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_HOLDING_REGISTERS, cr.GetStartAddress("AnaOut1"), cr.GetNumberOfRegisters("AnaOut1"));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % AI1 == 0)
                {
                    ModbusReadCommandParameters para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_INPUT_REGISTERS, cr.GetStartAddress("AnaIn1"), cr.GetNumberOfRegisters("AnaIn1"));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                try
                {
                    cnt++;
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