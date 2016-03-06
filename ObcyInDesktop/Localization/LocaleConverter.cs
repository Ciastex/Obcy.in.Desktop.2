using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ObcyInDesktop.Localization
{
    public class LocaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return "NoTranslationAvailable";

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return parameter as string;

            var s = parameter as string;

            if (s != null)
            {
                return LocaleSelector.GetLocaleString(s);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
