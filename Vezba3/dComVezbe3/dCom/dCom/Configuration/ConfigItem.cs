using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dCom.Configuration
{
	public class ConfigItem
	{
		#region Fields
		PointType registryType;
		ushort numberOfRegisters;
		ushort startAddress;
		ushort decimalSeparatorPlace;
		ushort minValue;
		ushort maxValue;
		ushort defaultValue;
		string processingType;
		string description;
		int acquisitionInterval;
		#endregion

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

		#endregion

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
			Int32.TryParse(configurationParameters[9], out temp);
			AcquisitionInterval = temp;
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
