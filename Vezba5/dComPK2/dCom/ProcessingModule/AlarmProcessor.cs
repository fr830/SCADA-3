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
            if (CheckReasonability(eguValue, configItem))
                return AlarmType.NO_ALARM;
            else
                return AlarmType.REASONABILITY_FAILURE;
		}

		private static bool CheckReasonability(double eguValue, IConfigItem configItem)
		{
            if (eguValue <= configItem.EGU_Min || eguValue >= configItem.EGU_Max)
                return false;
            else
                return true;
		}

		public static AlarmType GetAlarmForDigitalPoint(ushort state, IConfigItem configItem)
		{
			
			return AlarmType.NO_ALARM;
		}
	}
}
