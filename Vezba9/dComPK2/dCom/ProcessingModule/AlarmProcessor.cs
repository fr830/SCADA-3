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
                if (CheckAlarmility(eguValue, configItem) == 1)
                    return AlarmType.HIGH_ALARM;
                else if (CheckAlarmility(eguValue, configItem) == -1)
                    return AlarmType.LOW_ALARM;
                else
                    return AlarmType.NO_ALARM;
            else
			    return AlarmType.REASONABILITY_FAILURE;
		}

        private static bool CheckReasonability(double eguvalue, IConfigItem configitem)
        {
            if (eguvalue <= configitem.EGU_Min || eguvalue >= configitem.EGU_Max)
                return false;
            else
                return true;

        }

        private static int CheckAlarmility(double eguvalue, IConfigItem configitem)
        {
            if (eguvalue <= configitem.LowAlarm)
                return -1;
            else if (eguvalue >= configitem.HighAlarm)
                return 1;
            else
                return 0;

        }

        public static AlarmType GetAlarmForDigitalPoint(ushort state, IConfigItem configItem)
		{
            if (state == configItem.AbnormalValue)
                return AlarmType.ABNORMAL_VALUE;
            else
			    return AlarmType.NO_ALARM;
		}
	}
}
