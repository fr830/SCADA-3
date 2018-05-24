using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public interface IModbusFunction
	{
		Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes);
		byte[] PackRequest();
	}
}
