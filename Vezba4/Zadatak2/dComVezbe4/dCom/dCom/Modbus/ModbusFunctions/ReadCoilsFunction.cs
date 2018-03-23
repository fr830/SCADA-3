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
            Console.WriteLine(quantity);
            for (int i = 0; i < quantity; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    value = (ushort)(response[9 + i] & (byte)0x1);
                    response[9 + i] /= 2;

                    if (mdmReadCommParams.Quantity < (j + i * 8)) { break; }


                    dic.Add(new Tuple<PointType, ushort>(PointType.DIGITAL_OUTPUT, (ushort)(mdmReadCommParams.StartAddress + (j + i * 8))), value);
                }

            }
            return dic;
        }
	}
}