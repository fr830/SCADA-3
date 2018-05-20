using Common;
using Modbus;
using Modbus.FunctionParameters;
using ProcessingModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCom.ViewModel
{
	internal class DigitalBase : BasePointItem
	{
		private DState state;

		public DigitalBase(IConfigItem c, IFunctionExecutor commandExecutor, IStateUpdater stateUpdater, IConfiguration configuration, int i) 
			: base(c, commandExecutor, stateUpdater, configuration, i)
		{
			ProcessRawValue(RawValue);
        }

		public DState State
		{
			get
			{
				return state;
			}

			set
			{
				state = value;
				OnPropertyChanged("State");
				OnPropertyChanged("DisplayValue");
			}
		}

		public override string DisplayValue
		{
			get
			{
				return State.ToString();
			}
		}

		protected override void CommandExecutor_UpdatePointEvent(PointType type, ushort pointAddres, ushort newValue)
		{
			if (this.type == type && this.address == pointAddres)
			{
				RawValue = newValue;
				ProcessRawValue(newValue);
				Timestamp = DateTime.Now;
			}
		}

		private void ProcessRawValue(ushort newValue)
		{
			State = (DState)newValue;
			// TODO implement
            ProcessAlarm(newValue);
		}

		private void ProcessAlarm(ushort state)
		{
			alarm = AlarmProcessor.GetAlarmForDigitalPoint(RawValue, configItem);
			OnPropertyChanged("Alarm");
		}

		protected override void WriteCommand_Execute(object obj)
		{
			try
			{
				ModbusWriteCommandParameters p = new ModbusWriteCommandParameters(6, (byte)GetWriteFunctionCode(type), address, (ushort)CommandedValue, configuration);
				IModbusFunction fn = FunctionFactory.CreateModbusFunction(p);
				this.commandExecutor.EnqueueCommand(fn);
			}
			catch (Exception ex)
			{
				string message = $"{ex.TargetSite.ReflectedType.Name}.{ex.TargetSite.Name}: {ex.Message}";
				this.stateUpdater.LogMessage(message);
			}
		}
	}
}
