using dCom.Connection;
using System;

namespace dCom.ViewModel
{
	public class DigitalOutput : BasePointItem
	{
		private DState state;
		private DState lastCommandedState;
		private DateTime lastCommanTime = DateTime.Now;
		private DState abnormalState = DState.OPENED;

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

		public DState LastCommandedState
		{
			get
			{
				return lastCommandedState;
			}

			set
			{
				lastCommandedState = value;
				OnPropertyChanged("LastCommandedState");
			}
		}

		public DateTime LastCommanTime
		{
			get
			{
				return lastCommanTime;
			}

			set
			{
				lastCommanTime = value;
				OnPropertyChanged("LastCommanTime");
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

		public DigitalOutput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater) : base(type, address, commandExecutor, name, stateUpdater)
		{
			State = (DState)defaultValue;
		}

		public DigitalOutput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater, ushort minValue, ushort maxValue) : this(type, address, defaultValue, commandExecutor, name, stateUpdater)
		{
			this.Min = minValue;
			this.Max = maxValue;
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