using Common;
using System;
using System.Collections.Generic;

namespace dCom.Configuration
{
	internal class ConfigItem : IConfigItem
	{
		#region Fields

		private PointType registryType;
		private ushort numberOfRegisters;
		private ushort startAddress;
		private ushort decimalSeparatorPlace;
		private ushort minValue;
		private ushort maxValue;
		private ushort defaultValue;
		private string processingType;
		private string description;
		private int acquisitionInterval;
        int a;
        int b;
        int eGU_Min;
        int eGU_Max;
        int abnormalValue;
        int lowAlarm;
        int highAlarm;

		#endregion Fields

		#region Properties

		public PointType RegistryType
		{
			get
			{
				return registryType;
			}

			set
			{
				registryType = value;
			}
		}

		public ushort NumberOfRegisters
		{
			get
			{
				return numberOfRegisters;
			}

			set
			{
				numberOfRegisters = value;
			}
		}

		public ushort StartAddress
		{
			get
			{
				return startAddress;
			}

			set
			{
				startAddress = value;
			}
		}

		public ushort DecimalSeparatorPlace
		{
			get
			{
				return decimalSeparatorPlace;
			}

			set
			{
				decimalSeparatorPlace = value;
			}
		}

		public ushort MinValue
		{
			get
			{
				return minValue;
			}

			set
			{
				minValue = value;
			}
		}

		public ushort MaxValue
		{
			get
			{
				return maxValue;
			}

			set
			{
				maxValue = value;
			}
		}

		public ushort DefaultValue
		{
			get
			{
				return defaultValue;
			}

			set
			{
				defaultValue = value;
			}
		}

		public string ProcessingType
		{
			get
			{
				return processingType;
			}

			set
			{
				processingType = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}

			set
			{
				description = value;
			}
		}

		public int AcquisitionInterval
		{
			get
			{
				return acquisitionInterval;
			}

			set
			{
				acquisitionInterval = value;
			}
		}

        public int A { get => a; set => a = value; }
        public int B { get => b; set => b = value; }
        public int EGU_Min { get => eGU_Min; set => eGU_Min = value; }
        public int EGU_Max { get => eGU_Max; set => eGU_Max = value; }
        public int AbnormalValue { get => abnormalValue; set => abnormalValue = value; }
        public int LowAlarm { get => lowAlarm; set => lowAlarm = value; }
        public int HighAlarm { get => highAlarm; set => highAlarm = value; }

        #endregion Properties

        public ConfigItem(List<string> configurationParameters)
		{
			RegistryType = GetRegistryType(configurationParameters[0]);
			int temp;
			Int32.TryParse(configurationParameters[1], out temp);
			NumberOfRegisters = (ushort)temp;
			Int32.TryParse(configurationParameters[2], out temp);
			StartAddress = (ushort)temp;
			Int32.TryParse(configurationParameters[3], out temp);
			DecimalSeparatorPlace = (ushort)temp;
			Int32.TryParse(configurationParameters[4], out temp);
			MinValue = (ushort)temp;
			Int32.TryParse(configurationParameters[5], out temp);
			MaxValue = (ushort)temp;
			Int32.TryParse(configurationParameters[6], out temp);
			DefaultValue = (ushort)temp;
			ProcessingType = configurationParameters[7];
			Description = configurationParameters[8].TrimStart('@');
			if (configurationParameters[9] != "#")
			{
				Int32.TryParse(configurationParameters[9], out temp);
				AcquisitionInterval = temp;
			}
			else
			{
				AcquisitionInterval = 1;
			}

            if (configurationParameters[10] != "#")
            {
                Int32.TryParse(configurationParameters[10], out temp);
                A = temp;
            }
            else
            {
                A = 1;
            }

            if (configurationParameters[11] != "#")
            {
                Int32.TryParse(configurationParameters[11], out temp);
                B = temp;
            }
            else
            {
                B = 1;
            }

            if (configurationParameters[12] != "#")
            {
                Int32.TryParse(configurationParameters[12], out temp);
                EGU_Min = temp;
            }
            else
            {
                EGU_Min = 1;
            }

            if (configurationParameters[13] != "#")
            {
                Int32.TryParse(configurationParameters[13], out temp);
                EGU_Max = temp;
            }
            else
            {
                EGU_Max = 1;
            }

            if (configurationParameters[14] != "#")
            {
                Int32.TryParse(configurationParameters[14], out temp);
                abnormalValue = temp;
            }
            else
            {
                abnormalValue = 1;
            }

            if (configurationParameters[15] != "#")
            {
                Int32.TryParse(configurationParameters[15], out temp);
                lowAlarm = temp;
            }
            else
            {
                lowAlarm = 1;
            }

            if (configurationParameters[16] != "#")
            {
                Int32.TryParse(configurationParameters[16], out temp);
                highAlarm = temp;
            }
            else
            {
                highAlarm = 1;
            }
        }

		private PointType GetRegistryType(string registryTypeName)
		{
			PointType registryType;
			switch (registryTypeName)
			{
				case "DO_REG":
					registryType = PointType.DIGITAL_OUTPUT;
					break;

				case "DI_REG":
					registryType = PointType.DIGITAL_INPUT;
					break;

				case "IN_REG":
					registryType = PointType.ANALOG_INPUT;
					break;

				case "HR_INT":
					registryType = PointType.ANALOG_OUTPUT;
					break;

				default:
					registryType = PointType.HR_LONG;
					break;
			}
			return registryType;
		}
	}
}