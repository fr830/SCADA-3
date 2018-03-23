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
		ushort value;
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
				OnPropertyChanged("DisplayValue");
			}
		}

		public AnalogOutput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater) : base(type, address, commandExecutor, name, stateUpdater)
		{
			Value = defaultValue;
		}

		public AnalogOutput(PointType type, ushort address, ushort defaultValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater, ushort minValue, ushort maxValue) : this(type, address, defaultValue, commandExecutor, name, stateUpdater)
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
