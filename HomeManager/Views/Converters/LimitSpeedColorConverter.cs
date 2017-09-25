using HomeManager.Statistics.Enums;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace HomeManager.Views.Converters
{
    public class LimitSpeedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is SpeedDangerLevel))
                throw new ArgumentException("Value must be instance of SpeedDangerLevel enumeration.");

            SpeedDangerLevel danger = (SpeedDangerLevel)value;

            if (danger == SpeedDangerLevel.Green)
                return new SolidColorBrush(Colors.Green);
            else if (danger == SpeedDangerLevel.Orange)
                return new SolidColorBrush(Colors.Orange);
            else
                return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
