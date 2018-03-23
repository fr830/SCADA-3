using dCom.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCom.ViewModel
{
	public class AnalaogInput : BasePointItem
	{
		ushort value;
		DateTime lastChangeTime;
		string egu;

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
				OnPropertyChanged("DisplayValue");
				OnPropertyChanged("RawValue");
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

		public AnalaogInput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater) : base(type, address, commandExecutor, name, stateUpdater)
		{
			Value = defaultValue;
		}

		public AnalaogInput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater, ushort minValue, ushort maxValue) : this(type, address, defaultValue, commandExecutor, name, stateUpdater)
		{
			this.Min = minValue;
			this.Max = maxValue;
		}

		public override string DisplayValue
		{
			get
			{
				return value.ToString();
			}
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
