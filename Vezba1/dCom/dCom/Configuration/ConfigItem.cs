﻿using System;
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
		int numberOfRegisters;
		ushort startAddress;
		int decimalSeparatorPlace;
		int minValue;
		int maxValue;
		int defaultValue;
		string processingType;
		string description;
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

		public int NumberOfRegisters
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

		public int DecimalSeparatorPlace
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

		public int MinValue
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

		public int MaxValue
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

		public int DefaultValue
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
		#endregion

		public ConfigItem(List<string> configurationParameters)
		{
			RegistryType = (PointType)Enum.Parse(typeof(PointType),configurationParameters[0]);
			int temp;
			Int32.TryParse(configurationParameters[1], out temp);
			NumberOfRegisters = temp;
			Int32.TryParse(configurationParameters[2], out temp);
			StartAddress = (ushort)temp;
			Int32.TryParse(configurationParameters[3], out temp);
			DecimalSeparatorPlace = temp;
			Int32.TryParse(configurationParameters[4], out temp);
			MinValue = temp;
			Int32.TryParse(configurationParameters[5], out temp);
			MaxValue = temp;
			Int32.TryParse(configurationParameters[6], out temp);
			DefaultValue = temp;
			ProcessingType = configurationParameters[7];
			Description = configurationParameters[8].TrimStart('@');
		}
	}
}
