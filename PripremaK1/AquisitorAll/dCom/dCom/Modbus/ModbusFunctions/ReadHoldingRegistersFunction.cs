using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace dCom.Modbus.ModbusFunctions
{
	public class ReadHoldingRegistersFunction : ModbusFunction
	{
		public ReadHoldingRegistersFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
		{
			CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
		}

		/// <inheritdoc />
		public override byte[] PackRequest()
		{
            ModbusReadCommandParameters mrcp = this.CommandParameters as ModbusReadCommandParameters;
            byte[] retVal = new byte[12];

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mrcp.TransactionId)), 0, retVal, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mrcp.ProtocolId)), 0, retVal, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mrcp.Length)), 0, retVal, 4, 2);

            retVal[6] = mrcp.UnitId;
            retVal[7] = mrcp.FunctionCode;

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mrcp.StartAddress)), 0, retVal, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mrcp.Quantity)), 0, retVal, 10, 2);

            return retVal;
        }

		/// <inheritdoc />
		public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
		{
            ModbusReadCommandParameters mrcp = this.CommandParameters as ModbusReadCommandParameters;
            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();

            ushort q = response[8];
            ushort value;

            int start1 = 7;
            int start2 = 8;
            for (int i = 0; i < q/2; i++)
            {
                byte p1 = response[start1 += 2];
                byte p2 = response[start2 += 2];

                value = (ushort)(p2 + (p1 << 8));

                dic.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT, (ushort)(mrcp.StartAddress + i)), value);
            }


                return dic;
        }
	}
}