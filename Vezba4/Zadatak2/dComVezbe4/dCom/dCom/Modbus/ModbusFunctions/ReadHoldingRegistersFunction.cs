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
            ModbusReadCommandParameters mdmReadCommParams = this.CommandParameters as ModbusReadCommandParameters;
            byte[] mdbRequest = new byte[12];
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mdmReadCommParams.TransactionId)), 0, mdbRequest, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mdmReadCommParams.ProtocolId)), 0, mdbRequest, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mdmReadCommParams.Length)), 0, mdbRequest, 4, 2);
            mdbRequest[6] = mdmReadCommParams.UnitId;
            mdbRequest[7] = mdmReadCommParams.FunctionCode;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mdmReadCommParams.StartAddress)), 0, mdbRequest, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mdmReadCommParams.Quantity)), 0, mdbRequest, 10, 2);
            return mdbRequest;
        }

		/// <inheritdoc />
		public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
		{
            ModbusReadCommandParameters mdmReadCommParams = this.CommandParameters as ModbusReadCommandParameters;
            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();

            ushort quantity = response[8];

            ushort value;

            int p1 = 7, p2 = 8;
            for (int i = 0; i < quantity / 2; i++)
            {
                byte port1 = response[p1 += 2];
                byte port2 = response[p2 += 2];

                value = (ushort)(port2 + (port1 << 8));
                dic.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT, (ushort)(mdmReadCommParams.StartAddress + i)), value);


            }
            return dic;
        }
	}
}