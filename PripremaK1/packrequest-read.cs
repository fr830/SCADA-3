//PACKREQUEST-READ

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