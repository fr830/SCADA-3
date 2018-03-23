using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace dCom.Modbus.ModbusFunctions
{
	public class WriteSingleRegisterFunction : ModbusFunction
	{
		public WriteSingleRegisterFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
		{
			CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusWriteCommandParameters));
		}

		public override byte[] PackRequest()
		{
            ModbusWriteCommandParameters mdmWriteCommParams = this.CommandParameters as ModbusWriteCommandParameters;
            byte[] mdbRequest = new byte[12];
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mdmWriteCommParams.TransactionId)), 0, mdbRequest, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mdmWriteCommParams.ProtocolId)), 0, mdbRequest, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mdmWriteCommParams.Length)), 0, mdbRequest, 4, 2);
            mdbRequest[6] = mdmWriteCommParams.UnitId;
            mdbRequest[7] = mdmWriteCommParams.FunctionCode;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mdmWriteCommParams.OutputAddress)), 0, mdbRequest, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)mdmWriteCommParams.Value)), 0, mdbRequest, 10, 2);
            return mdbRequest;
        }

		public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
		{
            ModbusWriteCommandParameters mdmWriteCommParams = this.CommandParameters as ModbusWriteCommandParameters;
            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();

            
            byte port1 = response[11];
            byte port2 = response[10];

            ushort value = (ushort)(port1 + (port2 << 8));
            dic.Add(new Tuple<PointType, ushort>(PointType.HR_INT, mdmWriteCommParams.OutputAddress), value);
            return dic;
        }
	}
}
