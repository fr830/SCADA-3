using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace dCom.Modbus.ModbusFunctions
{
	public class WriteSingleRegisterFunction : ModbusFunction
	{
		public WriteSingleRegisterFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
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