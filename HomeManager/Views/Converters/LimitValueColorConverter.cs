using System;
using System.Windows.Data;
using System.Windows.Media;

namespace HomeManager.Views.Converters
{
    public class LimitValueColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is double))
                throw new ArgumentException("Value must be double.");

            double percentage = (double)value;

            if (percentage <= 70)
                return new SolidColorBrush(Colors.Green);
            else if (percentage > 70 && percentage <= 90)
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
