using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingModule
{
	public static class EGUConverter
	{
		public static double ConvertToEGU(double scalingFactor, double deviation, ushort rawValue)
		{
			// TODO implement
			return rawValue;
		}

		public static ushort ConvertToRaw(double scalingFactor, double deviation, double eguValue)
		{
			// TODO implement
			return (ushort)eguValue;
		}
	}
}
