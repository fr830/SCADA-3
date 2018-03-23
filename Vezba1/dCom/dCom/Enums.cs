using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCom
{
	public enum DState : short
	{
		CLOSED = 0,
		OPENED = 1,
	}

	public enum ModbusFunctionCode : short
	{
		READ_COILS					= 0x01,
		READ_DISCRETE_INPUTS		= 0x02,
		READ_HOLDING_REGISTERS		= 0x03,
		READ_INPUT_REGISTERS		= 0x04,
		WRITE_SINGLE_COIL			= 0x05,
		WRITE_SINGLE_REGISTER		= 0x06,
	}

	public enum PointQuality : short
	{
		VALID = 0x01,
		INVALID = 0x02,
	}

	public enum PointType : short
	{
		DO_REG		= 0x01,
        DI_REG		= 0x02,
		IN_REG		= 0x03,
		HR_INT		= 0x04,
		HR_LONG		= 0x05,
	}

	public enum ConnectionState : short
	{
		CONNECTED = 0,
		DISCONNECTED = 1,
	}

	public enum CommandType : short
	{
		READ = 0x00,
		WRITE = 0x01,
	}
}
