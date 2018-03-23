using dCom.Modbus.FunctionParameters;
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
		public static ModbusFunction CreateModbusFunction(PointType pointType, CommandType commandType, ModbusCommandParameters commandParameters)
		{
			if(commandType == CommandType.READ)
			{
				switch (pointType)
				{
					case PointType.DO_REG:
						return new ReadCoilsFunction(commandParameters);
					case PointType.DI_REG:
						return new ReadDiscreteInputsFunction(commandParameters);
					case PointType.IN_REG:
						return new ReadInputRegistersFunction(commandParameters);
					case PointType.HR_INT:
						return new ReadHoldingRegistersFunction(commandParameters);
				}
			}
			if(commandType == CommandType.WRITE)
			{
				switch (pointType)
				{
					case PointType.DO_REG:
						return new WriteSingleCoilFunction(commandParameters);
					case PointType.HR_INT:
						return new WriteSingleRegisterFunction(commandParameters);
				}
			}
			return null;
		}
	}
}
