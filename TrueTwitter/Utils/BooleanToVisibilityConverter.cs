using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TrueTwitter.Utils
{
    /// <summary>
    /// Convert boolean to visibility
    /// 
    /// true -> visible
    /// false -> collapsed
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public BooleanToVisibilityConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool && (bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (value is Visibility && (Visibility)value == Visibility.Visible);
        }
    }
}
