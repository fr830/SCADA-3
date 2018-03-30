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

            int DigOut = cr.GetAcquisitionInterval("DigOut");
            int DigIn = cr.GetAcquisitionInterval("DigIn");
            int AnaOut = cr.GetAcquisitionInterval("AnaOut");
            int AnaIn = cr.GetAcquisitionInterval("AnaIn");

            ModbusReadCommandParameters mrcp = null;
            ModbusFunction fn = null;

            while (true)
            {


                if (cnt % DigOut == 0)
                {
                    mrcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_COILS, cr.GetStartAddress("DigOut"), cr.GetNumberOfRegisters("DigOut"));
                    fn = FunctionFactory.CreateModbusFunction(mrcp);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % DigIn == 0)
                {
                    mrcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_DISCRETE_INPUTS, cr.GetStartAddress("DigIn"), cr.GetNumberOfRegisters("DigIn"));
                    fn = FunctionFactory.CreateModbusFunction(mrcp);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % AnaOut == 0)
                {
                    mrcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_HOLDING_REGISTERS, cr.GetStartAddress("AnaOut"), cr.GetNumberOfRegisters("AnaOut"));
                    fn = FunctionFactory.CreateModbusFunction(mrcp);
                    this.commandExecutor.EnqueueCommand(fn);
                }

                if (cnt % AnaIn == 0)
                {
                    mrcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_INPUT_REGISTERS, cr.GetStartAddress("AnaIn"), cr.GetNumberOfRegisters("AnaIn"));
                    fn = FunctionFactory.CreateModbusFunction(mrcp);
                    this.commandExecutor.EnqueueCommand(fn);
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