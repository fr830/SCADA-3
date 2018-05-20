using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingModule
{
	public static class AlarmProcessor
	{
		public static AlarmType GetAlarmForAnalogPoint(double eguValue, IConfigItem configItem)
		{
			// TODO implement
			return AlarmType.NO_ALARM;
		}

		//private static bool CheckReasonability(double eguValue, IConfigItem configItem)
		//{
			// TODO implement
		//}

		public static AlarmType GetAlarmForDigitalPoint(ushort state, IConfigItem configItem)
		{
			// TODO implement
			return AlarmType.NO_ALARM;
		}
	}
}
