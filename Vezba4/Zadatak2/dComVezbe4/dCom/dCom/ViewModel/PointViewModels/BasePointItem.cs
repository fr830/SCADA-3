using dCom.Connection;
using dCom.Modbus;
using dCom.Modbus.FunctionParameters;
using dCom.Modbus.ModbusFunctions;
using dCom.Utils;
using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace dCom.ViewModel
{
	public class BasePointItem : ViewModelBase, IDataErrorInfo
	{
		protected PointType type;
		protected ushort address;
		private PointQuality quality = PointQuality.VALID;
		private DateTime timestamp = DateTime.Now;
		private string name = string.Empty;
		private ushort rawValue;
		private ushort commandedValue;
		private ushort min;
		private ushort max;

		private bool isEnabledForCommanding = true;

		private FunctionExecutor commandExecutor;

		protected Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

		private IStateUpdater stateUpdater;

		/// <summary>
		/// Command that is executed when write button is clicked on control window;
		/// Command should create write command parameters and provide it to FunctionFactory
		/// </summary>
		public RelayCommand WriteCommand { get; set; }

		/// <summary>
		/// Command that is executed when read button is clicked on control window;
		/// Command should create read command parameters and provide it to FunctionFactory
		/// </summary>
		public RelayCommand ReadCommand { get; set; }

		public BasePointItem(PointType type, ushort address, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater)
		{
			this.type = type;
			this.address = address;
			this.commandExecutor = commandExecutor;
			this.name = name;
			this.stateUpdater = stateUpdater;
			commandExecutor.UpdatePointEvent += CommandExecutor_UpdatePointEvent;
			WriteCommand = new RelayCommand(WriteCommand_Execute);
			ReadCommand = new RelayCommand(ReadCommand_Execute);
		}

		public BasePointItem()
		{
		}

		protected virtual void CommandExecutor_UpdatePointEvent(PointType type, ushort pointAddres, ushort newValue)
		{
			// Intentionally left blank
		}

		/// <summary>
		/// Method that is executed when write button is clicked on control window;
		/// Method should create write command parameters and provide it to FunctionFactory
		/// </summary>
		/// <param name="obj">Not used</param>
		private void WriteCommand_Execute(object obj)
		{
			try
			{
				// TODO implement
				ModbusFunction fn = FunctionFactory.CreateModbusFunction(null);
				this.commandExecutor.EnqueueCommand(fn);
			}
			catch (Exception ex)
			{
				string message = $"{ex.TargetSite.ReflectedType.Name}.{ex.TargetSite.Name}: {ex.Message}";
				this.stateUpdater.LogMessage(message);
			}
		}

		/// <summary>
		/// Method that is executed when read button is clicked on control window;
		/// Method should create read command parameters and provide it to FunctionFactory
		/// </summary>
		/// <param name="obj">Not used</param>
		private void ReadCommand_Execute(object obj)
		{
			try
			{
				// TODO implement
				ModbusFunction fn = FunctionFactory.CreateModbusFunction(null);
				this.commandExecutor.EnqueueCommand(fn);
			}
			catch (Exception ex)
			{
				string message = $"{ex.TargetSite.ReflectedType.Name}.{ex.TargetSite.Name}: {ex.Message}";
				this.stateUpdater.LogMessage(message);
			}
		}

		#region Properties

		public ushort Min
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

		public ushort Max
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

		public PointType Type
		{
			get
			{
				return type;
			}

			set
			{
				type = value;
				OnPropertyChanged("Type");
			}
		}

		/// <summary>
		/// Address of point on MdbSim Simulator
		/// </summary>
		public ushort Address
		{
			get
			{
				return address;
			}

			set
			{
				address = value;
				OnPropertyChanged("Address");
			}
		}

		public PointQuality Quality
		{
			get
			{
				return quality;
			}

			set
			{
				quality = value;
				OnPropertyChanged("Quality");
			}
		}

		public DateTime Timestamp
		{
			get
			{
				return timestamp;
			}

			set
			{
				timestamp = value;
				OnPropertyChanged("Timestamp");
			}
		}

		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				name = value;
			}
		}

		public virtual string DisplayValue
		{
			get
			{
				return rawValue.ToString();
			}
		}

		/// <summary>
		/// Value that is sent on MdbSim simulator
		/// </summary>
		public ushort CommandedValue
		{
			get
			{
				return commandedValue;
			}

			set
			{
				commandedValue = value;
				OnPropertyChanged("CommandedValue");
			}
		}

		/// <summary>
		/// Raw value, read from MdbSim
		/// </summary>
		public virtual ushort RawValue
		{
			get
			{
				return rawValue;
			}
			set
			{
				rawValue = value;
				OnPropertyChanged("RawValue");
			}
		}

		#endregion Properties

		#region Input validation

		public string Error
		{
			get
			{
				return string.Empty;
			}
		}

		public bool IsEnabledForCommanding
		{
			get
			{
				return isEnabledForCommanding;
			}

			set
			{
				isEnabledForCommanding = value;
				OnPropertyChanged("IsEnabledForCommanding");
			}
		}

		public string this[string columnName]
		{
			get
			{
				IsEnabledForCommanding = true;
				string message = string.Empty;
				if (columnName == "CommandedValue")
				{
					if (commandedValue > Max)
					{
						message = $"Entered value cannot be greater than {Max}.";
						IsEnabledForCommanding = false;
					}
					if (commandedValue < Min)
					{
						message = $"Entered value cannot be lower than {Min}.";
						IsEnabledForCommanding = false;
					}
				}
				return message;
			}
		}

		#endregion Input validation
	}
}