﻿using System;
using System.Windows;
using System.Windows.Data;

namespace HomeManager.Views.Converters
{
    public class BoolToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool))
                return value;

            var reversed = parameter != null ? bool.Parse(parameter.ToString()) : false;

            if (value != null)
            {
                if (reversed)
                    return (bool)value ? Visibility.Collapsed : Visibility.Visible;
                else
                    return (bool)value ? Visibility.Visible : Visibility.Collapsed; 
            }
                
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
    }
}
