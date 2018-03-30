//PARSERESPONSE-BIT

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