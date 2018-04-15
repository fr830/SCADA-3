using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace dCom.Modbus.ModbusFunctions
{
	public class ReadCoilsFunction : ModbusFunction
	{
		public ReadCoilsFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
		{
			CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
		}

		/// <inheritdoc/>
		public override byte[] PackRequest()
		{
            ModbusReadCommandParameters para = this.CommandParameters as ModbusReadCommandParameters;
            byte[] ret = new byte[12];

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.TransactionId)), 0, ret, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.ProtocolId)), 0, ret, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.Length)), 0, ret, 4, 2);

            ret[6] = para.UnitId;
            ret[7] = para.FunctionCode;

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.StartAddress)), 0, ret, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.Quantity)), 0, ret, 10, 2);

            return ret;
        }

		/// <inheritdoc />
		public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
		{
            ModbusReadCommandParameters para = this.CommandParameters as ModbusReadCommandParameters;
            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();

            int q = response[8];

            for (int i = 0; i < q; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (para.Quantity < (j + i * 8)) { break; }

                    ushort v = (ushort)(response[9 + i] & (byte)0x1);
                    response[9 + i] /= 2;

                    dic.Add(new Tuple<PointType, ushort>(PointType.DIGITAL_OUTPUT, (ushort)(para.StartAddress + (j + i * 8))), v);
                }
            }
            return dic;
        }
	}
}