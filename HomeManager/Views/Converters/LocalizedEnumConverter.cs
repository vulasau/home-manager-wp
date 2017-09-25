using HomeManager.Resources;
using System;
using System.Resources;
using System.Windows.Data;

namespace HomeManager.Views.Converters
{
    public class LocalizedEnumConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;

            var resources = new ResourceManager("HomeManager.Resources.AppResources", typeof(AppResources).Assembly);
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            return resources.GetString(value.ToString(), currentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
