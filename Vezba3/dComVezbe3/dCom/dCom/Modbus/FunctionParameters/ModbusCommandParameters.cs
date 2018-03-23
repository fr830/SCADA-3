using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCom.Modbus.FunctionParameters
{
    public abstract class ModbusCommandParameters
    {
        private ushort transactionId;
        private ushort protocolId;
        private ushort length;
        private byte unitId;
        private byte functionCode;

        public ModbusCommandParameters(ushort length, byte functionCode)
        {
            TransactionId = Configuration.ConfigReader.Instance.GetTransactionId();
			UnitId = Configuration.ConfigReader.Instance.UnitAddress;

			ProtocolId = 0;
            Length = length;
            FunctionCode = functionCode;
        }

        public ushort TransactionId
        {
            get
            {
                return transactionId;
            }

            set
            {
                transactionId = value;
            }
        }

        public ushort ProtocolId
        {
            get
            {
                return protocolId;
            }

            set
            {
                protocolId = value;
            }
        }

        public ushort Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
            }
        }

        public byte UnitId
        {
            get
            {
                return unitId;
            }

            set
            {
                unitId = value;
            }
        }

        public byte FunctionCode
        {
            get
            {
                return functionCode;
            }

            set
            {
                functionCode = value;
            }
        }
    }
}
