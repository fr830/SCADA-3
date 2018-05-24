using Common;
using Modbus;
using Modbus.FunctionParameters;
using ProcessingModule;
using System;

namespace dCom.ViewModel
{
	internal class AnalogBase : BasePointItem
	{
		private double eguValue;

		public AnalogBase(IConfigItem c, IFunctionExecutor commandExecutor, IStateUpdater stateUpdater, IConfiguration configuration, int i)
			: base(c, commandExecutor, stateUpdater, configuration, i)
		{
			ProcessRawValue(RawValue);
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

		public double EguValue
		{
			get
			{
				return eguValue;
			}

			set
			{
				eguValue = value;
				OnPropertyChanged("DisplayValue");
			}
		}

		private void ProcessRawValue(ushort newValue)
		{
			// TODO implement
			EguValue = EGUConverter.ConvertToEGU(configItem.A, configItem.B, newValue);
			ProcessAlarm(EguValue);
		}

        private void ProcessAlarm(double eguValue)
		{
			alarm = AlarmProcessor.GetAlarmForAnalogPoint(eguValue, configItem);
			OnPropertyChanged("Alarm");
		}

		public override string DisplayValue
		{
			get
			{
				return EguValue.ToString();
			}
		}

		protected override void WriteCommand_Execute(object obj)
		{
			try
			{
                // TODO implement
                ushort raw = EGUConverter.ConvertToRaw(CommandedValue,configItem.A,configItem.B);
				ModbusWriteCommandParameters p = new ModbusWriteCommandParameters(6, (byte)GetWriteFunctionCode(type), address, raw, configuration);
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
