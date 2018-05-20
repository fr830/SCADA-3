using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public interface IConfiguration
	{
		int TcpPort { get; }
		byte UnitAddress { get; }

		ushort GetTransactionId();
		List<IConfigItem> GetConfigurationItems();

		ushort GetStartAddress(string pointDescription);

		ushort GetNumberOfRegisters(string pointDescription);

        int GetAcquisitionInterval(string pointDescription);
    }
}
