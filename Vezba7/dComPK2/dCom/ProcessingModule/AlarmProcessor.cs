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
            {
                if (CheckAlarmility(eguValue, configItem) == 0)
                {
                    return AlarmType.NO_ALARM;
                }
                else if (CheckAlarmility(eguValue, configItem) == 1)
                {
                    return AlarmType.HIGH_ALARM;
                }
                else
                {
                    return AlarmType.LOW_ALARM;
                }
            }
            else
            {
                return AlarmType.REASONABILITY_FAILURE;
            }
        }

        private static bool CheckReasonability(double eguValue, IConfigItem configItem)
        {
            if (eguValue <= configItem.EGU_Min || eguValue >= configItem.EGU_Max)
                return false;
            else
                return true;

        }

        private static int CheckAlarmility(double eguValue, IConfigItem configItem)
        {
            if (eguValue <= configItem.LowAlarm)
                return -1;
            else if (eguValue >= configItem.HighAlarm)
                return 1;
            else
                return 0;

        }

        public static AlarmType GetAlarmForDigitalPoint(ushort state, IConfigItem configItem)
        {
            if (state != configItem.AbnormalValue)
                return AlarmType.NO_ALARM;
            else
                return AlarmType.ABNORMAL_VALUE;
        }
    }
}

