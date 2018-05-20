using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace dCom.Converters
{
	public class AlarmToBackgroundColorConverter : IValueConverter
	{
		private SolidColorBrush red = new SolidColorBrush(Colors.Red);
		private SolidColorBrush transparent = new SolidColorBrush(Colors.Transparent);
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SolidColorBrush color = new SolidColorBrush(Colors.Transparent);
			if (value != null && value is AlarmType)
			{
				AlarmType a = (AlarmType)value;
				if(a != AlarmType.NO_ALARM)
				{
					color = new SolidColorBrush(Colors.Red);
					color.Opacity = 0.5;
				}
			}
			return color;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
