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
            //ModbusWriteCommandParameters mdmWriteCommParams = this.CommandParameters as ModbusWriteCommandParameters;
            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();
            //ushort value = (ushort)(response[11] & (byte)0x1);
            //dic.Add(new Tuple<PointType, ushort>(PointType.DIGITAL_OUTPUT, mdmWriteCommParams.OutputAddress), value);
            return dic;
        }
    }
}