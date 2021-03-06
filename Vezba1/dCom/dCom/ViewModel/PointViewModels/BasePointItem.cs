﻿using dCom.Connection;
using dCom.Modbus;
using dCom.Modbus.FunctionParameters;
using dCom.Modbus.ModbusFunctions;
using dCom.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace dCom.ViewModel
{
	public class BasePointItem : ViewModelBase
	{
		protected PointType type;
		protected ushort address;
		PointQuality quality = PointQuality.VALID;
		DateTime timestamp = DateTime.Now;
		string name = string.Empty;

		string displayValue = "N/A";
		string rawValue = "N/A";
		private FunctionExecutor commandExecutor;

		protected Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

		IStateUpdater stateUpdater;

		public RelayCommand WriteCommand { get; set; }
		public RelayCommand ReadCommand { get; set; }

		public BasePointItem(PointType type, ushort address, int rawValue, FunctionExecutor commandExecutor, string name, IStateUpdater stateUpdater)
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

		private void WriteCommand_Execute(object obj)
		{
			try
			{
				// TODO implement
				ModbusFunction fn = FunctionFactory.CreateModbusFunction(this.Type, CommandType.WRITE, null);
				this.commandExecutor.ExecuteCommand(fn);
			}
			catch (Exception ex)
			{
				this.stateUpdater.LogMessage(ex.Message);
			}
		}
		private void ReadCommand_Execute(object obj)
		{
			try
			{
                ModbusReadCommandParameters parameters = new ModbusReadCommandParameters(6,(byte)Type,this.Address,1);
				ModbusFunction fn = FunctionFactory.CreateModbusFunction(this.Type, CommandType.READ, parameters);
				this.commandExecutor.ExecuteCommand(fn);
			}
			catch (Exception ex)
			{
				this.stateUpdater.LogMessage(ex.Message);
			}
		}

		#region Properties
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
				return displayValue;
			}
		}

		public virtual string RawValue
		{
			get
			{
				return rawValue;
			}
			set
			{
				rawValue = value;
			}
		}

		#endregion
	}
}
