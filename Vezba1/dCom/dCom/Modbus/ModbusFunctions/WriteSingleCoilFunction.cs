using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;

namespace dCom.Modbus.ModbusFunctions
{
	public class WriteSingleCoilFunction : ModbusFunction
	{
		public WriteSingleCoilFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
		{
		}

		public override byte[] PackRequest()
		{
			throw new NotImplementedException();
		}

		public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
		{
			throw new NotImplementedException();
		}
	}
}
