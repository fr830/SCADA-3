using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace dCom.Modbus.ModbusFunctions
{
	public class WriteSingleCoilFunction : ModbusFunction
	{
		public WriteSingleCoilFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
		{
			CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusWriteCommandParameters));
		}

		/// <inheritdoc />
		public override byte[] PackRequest()
		{
            throw new NotImplementedException();
        }

		/// <inheritdoc />
		public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
		{
            throw new NotImplementedException();
        }
	}
}