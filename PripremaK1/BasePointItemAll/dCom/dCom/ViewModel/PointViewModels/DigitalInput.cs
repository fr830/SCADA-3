using dCom.Connection;
using System;

namespace dCom.ViewModel
{
	public class DigitalInput : BasePointItem
	{
		private DState state;
		private DateTime lastChangeTime = DateTime.Now;
		private DState abnormalState = DState.OPENED;

		public DigitalInput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater) : base(type, address, commandExecutor, name, stateUpdater, defaultValue)
		{
			State = (DState)defaultValue;
		}

		public DigitalInput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater, ushort minValue, ushort maxValue) : this(type, address, defaultValue, commandExecutor, name, stateUpdater)
		{
			this.Min = minValue;
			this.Max = maxValue;
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
			}
		}

		public DateTime LastChangeTime
		{
			get
			{
				return lastChangeTime;
			}

			set
			{
				lastChangeTime = value;
				OnPropertyChanged("LastChangeTime");
			}
		}

		public DState AbnormalState
		{
			get
			{
				return abnormalState;
			}

			set
			{
				abnormalState = value;
				OnPropertyChanged("AbnormalState");
			}
		}

		protected override void CommandExecutor_UpdatePointEvent(PointType type, ushort pointAddres, ushort newValue)
		{
			if (this.type == type && this.address == pointAddres)
			{
				State = (DState)newValue;
				RawValue = newValue;
				Timestamp = DateTime.Now;
			}
		}
	}
}