using System;
using System.Windows;
using System.Windows.Data;

namespace HomeManager.Views.Converters
{
    public class NullToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var inversed = parameter != null && bool.Parse(parameter.ToString().ToLower()) ? true : false;
            var visibility = Convert(value);

            if (!inversed)
                return visibility;

            return visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        private Visibility Convert(object value)
        {
            if (value is int)
                return (int)value > 0 ? Visibility.Visible : Visibility.Collapsed;

            if (value is double)
                return (double)value > 0 ? Visibility.Visible : Visibility.Collapsed;

            if (value is string)
                return string.IsNullOrEmpty((string)value) ? Visibility.Visible : Visibility.Collapsed;

            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
