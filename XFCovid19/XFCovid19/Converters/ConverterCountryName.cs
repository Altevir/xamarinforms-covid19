using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFCovid19.Helpers;

namespace XFCovid19.Converters
{
    public class ConverterCountryName : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TranslateExtension.TranslateText(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
