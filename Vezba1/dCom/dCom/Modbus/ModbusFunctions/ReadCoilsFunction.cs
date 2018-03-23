using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;

namespace dCom.Modbus.ModbusFunctions
{
    public class ReadCoilsFunction : ModbusFunction
    {
        public ReadCoilsFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
        {
        }

        public override byte[] PackRequest()
        {
            byte[] retVal = new byte[12];
            byte[] temp = new byte[2];

            /* CommandParameters je klasa abstrakta klasa ModbusCommandParameters
                                                  koji nasledjuju read i write.*/
            temp = BitConverter.GetBytes(CommandParameters.TransactionId);
            Array.Reverse(temp);
            retVal[0] = temp[0];
            retVal[1] = temp[1];

            temp = BitConverter.GetBytes(CommandParameters.ProtocolId);
            Array.Reverse(temp);
            retVal[2] = temp[0];
            retVal[3] = temp[1];

            temp = BitConverter.GetBytes(CommandParameters.Length);
            Array.Reverse(temp);
            retVal[4] = temp[0];
            retVal[5] = temp[1];

            retVal[6] = CommandParameters.UnitId;

            retVal[7] = CommandParameters.FunctionCode;

            /*Moramo kastovati CommandParameters na nasledjenu klasu ModbusReadCommandParameters 
                          klase ModbusCommandParameters da bismo pristupili polju StartAddress*/
            temp = BitConverter.GetBytes(((ModbusReadCommandParameters)CommandParameters).StartAddress);
            Array.Reverse(temp);
            retVal[8] = temp[0];
            retVal[9] = temp[1];

            temp = BitConverter.GetBytes(((ModbusReadCommandParameters)CommandParameters).Quantity);
            Array.Reverse(temp);
            retVal[10] = temp[0];
            retVal[11] = temp[1];

            return retVal;
        }

        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
        {
            Dictionary<Tuple<PointType, ushort>, ushort> retVal = new Dictionary<Tuple<PointType, ushort>, ushort>();

            ushort address = ((ModbusReadCommandParameters)CommandParameters).StartAddress;
            Tuple<PointType, ushort> key = new Tuple<PointType, ushort>(PointType.DO_REG, address);

            ushort mask = 0b00000001;
            ushort value = (ushort)(response[9] & mask);

            retVal.Add(key, value);

            return retVal;
        }
    }
}
