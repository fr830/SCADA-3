//PACKREQUEST-WRITE

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