using dCom.Connection;
using System;

namespace dCom.ViewModel
{
	public class AnalogOutput : BasePointItem
	{
		private short lastCommandedValue;
		private DateTime lastCommandTime = DateTime.Now;
		private string egu;
		private ushort value;

		public short LastCommandedValue
		{
			get
			{
				return lastCommandedValue;
			}

			set
			{
				lastCommandedValue = value;
				OnPropertyChanged("LastCommandedValue");
			}
		}

		public DateTime LastCommandTime
		{
			get
			{
				return lastCommandTime;
			}

			set
			{
				lastCommandTime = value;
				OnPropertyChanged("LastCommandTime");
			}
		}

		public string Egu
		{
			get
			{
				return egu;
			}

			set
			{
				egu = value;
				OnPropertyChanged("Egu");
			}
		}

		public ushort Value
		{
			get
			{
				return value;
			}

			set
			{
				this.value = value;
				OnPropertyChanged("Value");
			}
		}

		public AnalogOutput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater) : base(type, address, commandExecutor, name, stateUpdater, defaultValue)
		{
			Value = defaultValue;
		}

		public AnalogOutput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater, ushort minValue, ushort maxValue) : this(type, address, defaultValue, commandExecutor, name, stateUpdater)
		{
			this.Min = minValue;
			this.Max = maxValue;
		}

		protected override void CommandExecutor_UpdatePointEvent(PointType type, ushort pointAddres, ushort newValue)
		{
			if (this.type == type && this.address == pointAddres)
			{
				Value = newValue;
				RawValue = newValue;
				Timestamp = DateTime.Now;
			}
		}
	}
}