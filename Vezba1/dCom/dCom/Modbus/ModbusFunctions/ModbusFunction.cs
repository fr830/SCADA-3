using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;

namespace dCom.Modbus.ModbusFunctions
{
	public abstract class ModbusFunction
	{
        private ModbusCommandParameters commandParameters;
        public ModbusFunction(ModbusCommandParameters commandParameters)
        {
            this.commandParameters = commandParameters;
        }

        public ModbusCommandParameters CommandParameters
        {
            get
            {
                return commandParameters;
            }

            set
            {
                commandParameters = value;
            }
        }

        public abstract byte[] PackRequest();
        public abstract Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response);
	}
}
