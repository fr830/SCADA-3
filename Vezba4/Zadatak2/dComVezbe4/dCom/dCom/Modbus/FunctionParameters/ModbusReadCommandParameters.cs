﻿namespace dCom.Modbus.FunctionParameters
{
	public class ModbusReadCommandParameters : ModbusCommandParameters
	{
		private ushort startAddress;
		private ushort quantity;

		public ModbusReadCommandParameters(ushort length, byte functionCode, ushort startAddress, ushort quantity)
				: base(length, functionCode)
		{
			StartAddress = startAddress;
			Quantity = quantity;
		}

		public ushort StartAddress
		{
			get
			{
				return startAddress;
			}

			private set
			{
				startAddress = value;
			}
		}

		public ushort Quantity
		{
			get
			{
				return quantity;
			}

			private set
			{
				quantity = value;
			}
		}
	}
}