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
            ModbusWriteCommandParameters para = this.CommandParameters as ModbusWriteCommandParameters;
            byte[] retVal = new byte[12];

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.TransactionId)), 0, retVal, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.ProtocolId)), 0, retVal, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.Length)), 0, retVal, 4, 2);

            retVal[6] = para.UnitId;
            retVal[7] = para.FunctionCode;

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.OutputAddress)), 0, retVal, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)para.Value)), 0, retVal, 10, 2);

            return retVal;
        }

		/// <inheritdoc />
		public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
		{
            ModbusWriteCommandParameters para = this.CommandParameters as ModbusWriteCommandParameters;

            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();
            
            ushort value = (ushort)(response[11] & (byte)0x1);
            dic.Add(new Tuple<PointType, ushort>(PointType.DIGITAL_OUTPUT, para.OutputAddress), value);
          

            return dic;
        }
	}
}