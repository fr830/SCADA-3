using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public static class PointIdentifierHelper
	{
		public static int GetNewPointId(ushort type, ushort address)
		{
			return type << 16 ^ address;
		}
	}
}
