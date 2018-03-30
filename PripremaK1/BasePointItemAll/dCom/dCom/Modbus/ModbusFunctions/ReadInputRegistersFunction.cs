using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace dCom.Modbus.ModbusFunctions
{
	public class ReadInputRegistersFunction : ModbusFunction
	{
		public ReadInputRegistersFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
		{
			CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
		}

		/// <inheritdoc />
		public override byte[] PackRequest()
		{
            ModbusReadCommandParameters para = this.CommandParameters as ModbusReadCommandParameters;
            byte[] retVal = new byte[12];

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.TransactionId)), 0, retVal, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.ProtocolId)), 0, retVal, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.Length)), 0, retVal, 4, 2);

            retVal[6] = para.UnitId;
            retVal[7] = para.FunctionCode;

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.StartAddress)), 0, retVal, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.Quantity)), 0, retVal, 10, 2);

            return retVal;
        }

		/// <inheritdoc />
		public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
		{
            ModbusReadCommandParameters para = this.CommandParameters as ModbusReadCommandParameters;

            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();

            ushort value = (ushort)(response[10] + (response[9] << 8));

            dic.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT, para.StartAddress), value);

            return dic;
        }
	}
}