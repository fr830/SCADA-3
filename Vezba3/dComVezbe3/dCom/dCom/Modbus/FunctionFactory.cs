﻿using dCom.Modbus.FunctionParameters;
using dCom.Modbus.ModbusFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCom.Modbus
{
	public class FunctionFactory
	{
		public static ModbusFunction CreateModbusFunction(ModbusCommandParameters commandParameters)
		{

			switch ((ModbusFunctionCode)commandParameters.FunctionCode)
			{
				case ModbusFunctionCode.READ_COILS:
					return new ReadCoilsFunction(commandParameters);
				case ModbusFunctionCode.READ_DISCRETE_INPUTS:
					return new ReadDiscreteInputsFunction(commandParameters);
				case ModbusFunctionCode.READ_INPUT_REGISTERS:
					return new ReadInputRegistersFunction(commandParameters);
				case ModbusFunctionCode.READ_HOLDING_REGISTERS:
					return new ReadHoldingRegistersFunction(commandParameters);
				case ModbusFunctionCode.WRITE_SINGLE_COIL:
					return new WriteSingleCoilFunction(commandParameters);
				case ModbusFunctionCode.WRITE_SINGLE_REGISTER:
					return new WriteSingleRegisterFunction(commandParameters);
				default:
					return null;
			}
		}
	}
}
