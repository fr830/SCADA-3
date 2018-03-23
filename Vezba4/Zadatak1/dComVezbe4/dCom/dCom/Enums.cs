namespace dCom
{
	public enum DState : short
	{
		CLOSED = 0,
		OPENED = 1,
	}

	public enum ModbusFunctionCode : short
	{
		READ_COILS = 0x01,
		READ_DISCRETE_INPUTS = 0x02,
		READ_HOLDING_REGISTERS = 0x03,
		READ_INPUT_REGISTERS = 0x04,
		WRITE_SINGLE_COIL = 0x05,
		WRITE_SINGLE_REGISTER = 0x06,
	}

	public enum PointQuality : short
	{
		VALID = 0x01,
		INVALID = 0x02,
	}

	public enum PointType : short
	{
		DIGITAL_OUTPUT = 0x01,
		DIGITAL_INPUT = 0x02,
		ANALOG_INPUT = 0x03,
		ANALOG_OUTPUT = 0x04,
		HR_LONG = 0x05,
	}

	public enum ConnectionState : short
	{
		CONNECTED = 0,
		DISCONNECTED = 1,
	}
}