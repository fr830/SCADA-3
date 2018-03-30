using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace dCom.Modbus.ModbusFunctions
{
	public class ReadDiscreteInputsFunction : ModbusFunction
	{
		public ReadDiscreteInputsFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
		{
			CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
		}

		/// <inheritdoc />
		public override byte[] PackRequest()
		{
            byte[] retVal = new byte[12];
            ModbusReadCommandParameters mrcp = this.CommandParameters as ModbusReadCommandParameters;

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

            for (int i = 0; i < q; i++)
            {
                for (int j = 0; j <8;  j++)
                {
                    value = (ushort)(response[9 + i] & (byte)0x1);
                    response[9 + i] /= 2;

                    if(mrcp.Quantity < (j + i * 8)) { break;}

                    dic.Add(new Tuple<PointType, ushort>(PointType.DIGITAL_INPUT, (ushort)(mrcp.StartAddress + (ushort)(j + i * 8))),value);
                }
            }

            return dic;
        }
	}
}