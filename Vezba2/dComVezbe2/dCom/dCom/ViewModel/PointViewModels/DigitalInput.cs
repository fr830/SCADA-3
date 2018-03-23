using dCom.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCom.ViewModel
{
	public class DigitalInput : BasePointItem
	{
		DState state;
		DateTime lastChangeTime = DateTime.Now;
		DState abnormalState = DState.OPENED;

		public DigitalInput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater) : base(type, address, commandExecutor, name, stateUpdater)
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
				OnPropertyChanged("DisplayValue");
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

		public override string DisplayValue
		{
			get
			{
				return state.ToString();
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
