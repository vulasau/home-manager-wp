﻿using System;
using System.Windows.Data;

namespace HomeManager.Views.Converters
{
    public class ReversedBooleanConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is bool)
                return !(bool)value;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
