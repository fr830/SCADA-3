using dCom.Configuration;
using dCom.Connection;
using dCom.Modbus;
using dCom.Modbus.FunctionParameters;
using dCom.Modbus.ModbusFunctions;
using dCom.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dCom.Acquisition
{
	public class Acquisitor : IDisposable
	{
		private AutoResetEvent acquisitionTrigger;
		private FunctionExecutor commandExecutor;
		private Thread acquisitionWorker;
		private IStateUpdater stateUpdater;

		public Acquisitor(AutoResetEvent acquisitionTrigger, FunctionExecutor commandExecutor, IStateUpdater stateUpdater)
		{
			this.stateUpdater = stateUpdater;
			this.acquisitionTrigger = acquisitionTrigger;
			this.commandExecutor = commandExecutor;
			this.InitializeAcquisitionThread();
			this.StartAcquisitionThread();
			
		}

		private void InitializeAcquisitionThread()
		{
			this.acquisitionWorker = new Thread(Acquisition_DoWork);
			this.acquisitionWorker.Name = "Acquisition thread";
		}

		private void StartAcquisitionThread()
		{
			acquisitionWorker.Start();
		}

		private void Acquisition_DoWork()
		{
            int cnt = 0;

            ConfigReader cr = ConfigReader.Instance;
            int DOCnt = cr.GetAcquisitionIntervalForPointType(PointType.DIGITAL_OUTPUT);
            int DICnt = cr.GetAcquisitionIntervalForPointType(PointType.DIGITAL_INPUT);
            int AICnt = cr.GetAcquisitionIntervalForPointType(PointType.ANALOG_INPUT);
            int AOCnt = cr.GetAcquisitionIntervalForPointType(PointType.ANALOG_OUTPUT);



            while (true)
            {
                //// TODO implement
                if (cnt % DOCnt == 0)
                {
                    // TODO implement
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_COILS, cr.GetStartAddressForPointType(PointType.DIGITAL_OUTPUT), cr.GetNumberOfRegistersForPointType(PointType.DIGITAL_OUTPUT));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(fn);

                }
                if (cnt % DICnt == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_DISCRETE_INPUTS, cr.GetStartAddressForPointType(PointType.DIGITAL_INPUT), cr.GetNumberOfRegistersForPointType(PointType.DIGITAL_INPUT));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(fn);
                }
                if (cnt % AOCnt == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_HOLDING_REGISTERS, cr.GetStartAddressForPointType(PointType.ANALOG_OUTPUT), cr.GetNumberOfRegistersForPointType(PointType.ANALOG_OUTPUT));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(mcp);
                    this.commandExecutor.EnqueueCommand(fn);
                }
                if (cnt % AICnt == 0)
                {
                    ModbusReadCommandParameters mcp = new ModbusReadCommandParameters(6, (byte)ModbusFunctionCode.READ_INPUT_REGISTERS, cr.GetStartAddressForPointType(PointType.ANALOG_INPUT), cr.GetNumberOfRegistersForPointType(PointType.ANALOG_INPUT));
                    ModbusFunction fn = FunctionFactory.CreateModbusFunction(mcp);
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

		public void Dispose()
		{
			this.acquisitionWorker.Abort();
		}
	}
}
