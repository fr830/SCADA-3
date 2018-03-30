//PARSERESPONSE-2BYTES

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