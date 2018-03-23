using dCom.Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Reflection;

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

		public override string ToString()
		{
			return $"Transaction: {commandParameters.TransactionId}, command {commandParameters.FunctionCode}";
		}

		protected void CheckArguments(MethodBase m, Type t)
		{
            if (commandParameters.GetType() != t)
			{
				string message = $"{m.ReflectedType.Name}{m.Name} has invalid argument {nameof(commandParameters)} of type {commandParameters.GetType().Name}.{Environment.NewLine}Argumet type should be {t.Name}";
				throw new ArgumentException(message);
			}
		}
		
		public abstract byte[] PackRequest();
		public abstract Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response);
	}
}
