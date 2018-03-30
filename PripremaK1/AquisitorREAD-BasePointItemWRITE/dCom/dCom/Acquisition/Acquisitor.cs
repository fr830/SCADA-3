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
            int cnt = 0;
            ConfigReader cr = ConfigReader.Instance;
            ModbusReadCommandParameters para;
            ModbusFunction fn;

            int DigOut1 = cr.GetAcquisitionInterval("DigOut1");
            int DigOut2 = cr.GetAcquisitionInterval("DigOut2");
            int DigIn1 = cr.GetAcquisitionInterval("DigIn1");
            int DigIn2 = cr.GetAcquisitionInterval("DigIn2");
            int AnaOut1 = cr.GetAcquisitionInterval("AnaOut1");
            int AnaOut2 = cr.GetAcquisitionInterval("AnaOut2");
            int AnaIn1 = cr.GetAcquisitionInterval("AnaIn1");
            int AnaIn2 = cr.GetAcquisitionInterval("AnaIn2");

            while (true)
            {
                if (cnt % DigOut1 == 0)
                {
                    para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_COILS, cr.GetStartAddress("DigOut1"), cr.GetNumberOfRegisters("DigOut1"));
                    fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % DigOut2 == 0)
                {
                    para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_COILS, cr.GetStartAddress("DigOut2"), cr.GetNumberOfRegisters("DigOut2"));
                    fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % DigIn1 == 0)
                {
                    para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_DISCRETE_INPUTS, cr.GetStartAddress("DigIn1"), cr.GetNumberOfRegisters("DigIn1"));
                    fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % DigIn2 == 0)
                {
                    para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_DISCRETE_INPUTS, cr.GetStartAddress("DigIn2"), cr.GetNumberOfRegisters("DigIn2"));
                    fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % AnaOut1 == 0)
                {
                    para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_HOLDING_REGISTERS, cr.GetStartAddress("AnaOut1"), cr.GetNumberOfRegisters("AnaOut1"));
                    fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % AnaOut2 == 0)
                {
                    para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_HOLDING_REGISTERS, cr.GetStartAddress("AnaOut2"), cr.GetNumberOfRegisters("AnaOut2"));
                    fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % AnaIn1 == 0)
                {
                    para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_INPUT_REGISTERS, cr.GetStartAddress("AnaIn1"), cr.GetNumberOfRegisters("AnaIn1"));
                    fn = FunctionFactory.CreateModbusFunction(para);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % AnaIn2 == 0)
                {
                    para = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_INPUT_REGISTERS, cr.GetStartAddress("AnaIn2"), cr.GetNumberOfRegisters("AnaIn2"));
                    fn = FunctionFactory.CreateModbusFunction(para);
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