using System;
using System.Globalization;
using System.Windows.Data;

namespace CefSharp.Binding
{
    class TitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "CefSharp.Cromium.Wpf - " + (value ?? "No title especified");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Windows.Data.Binding.DoNothing;
        }
    }
}
