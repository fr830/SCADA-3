using dCom.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCom.ViewModel
{
	public class AnalogOutput : BasePointItem
	{

		short lastCommandedValue;
		DateTime lastCommandTime = DateTime.Now;
		string egu;
		short value;
		short min;
		short max;

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

		public short Value
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

		public short Min
		{
			get
			{
				return min;
			}

			set
			{
				min = value;
				OnPropertyChanged("Min");
			}
		}

		public short Max
		{
			get
			{
				return max;
			}

			set
			{
				max = value;
				OnPropertyChanged("Max");
			}
		}

		public AnalogOutput(PointType type, ushort address, int rawValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater) : base(type, address, rawValue, commandExecutor, name, stateUpdater)
		{

		}

		public override string DisplayValue
		{
			get
			{
				return value.ToString();
			}
		}

		public override string RawValue
		{
			get
			{
				return value.ToString();
			}
			set
			{
				this.value = Convert.ToInt16(value);
			}
		}
		protected override void CommandExecutor_UpdatePointEvent(PointType type, ushort pointAddres, ushort newValue)
		{
			if (this.type == type && this.address == pointAddres)
			{
				Value = (short)newValue;
				Timestamp = DateTime.Now;
			}
		}
	}
}
