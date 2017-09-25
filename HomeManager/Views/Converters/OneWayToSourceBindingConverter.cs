using System;
using System.Windows;
using System.Windows.Data;

namespace HomeManager.Views.Converters
{
    public class OneWayToSourceBindingConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double && (double)value == 0)
                return DependencyProperty.UnsetValue;

            if (value is int && (int)value == 0)
                return DependencyProperty.UnsetValue;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString().Replace(',', '.');
        }
    }
}
