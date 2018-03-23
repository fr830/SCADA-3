using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace dCom.Converters
{
//DO_REG RW
//DI_REG R
//IN_REG R
//HR_INT RW

	public class PointTypeToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && value is PointType)
			{
				PointType pt = (PointType)value;
				if (pt == PointType.DO_REG || pt == PointType.HR_INT)
				{
					return Visibility.Visible;
				}
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
