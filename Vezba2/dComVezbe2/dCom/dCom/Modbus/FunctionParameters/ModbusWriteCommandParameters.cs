using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCom.Modbus.FunctionParameters
{
	public class ModbusWriteCommandParameters : ModbusCommandParameters
	{
		private ushort outputAddress;
		private ushort value;

		public ModbusWriteCommandParameters(ushort length, byte functionCode, ushort outputAddress, ushort value)
			: base(length, functionCode)
		{
			OutputAddress = outputAddress;
			Value = value;
		}

		public ushort OutputAddress
		{
			get
			{
				return outputAddress;
			}

			set
			{
				outputAddress = value;
			}
		}

		public ushort Value
		{
			get
			{
				return value;
			}

			set
			{
				this.value = value;
			}
		}
	}
}
