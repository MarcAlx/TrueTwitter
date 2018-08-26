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
    public class CountToVisibilityConverter : IValueConverter
    {
        public CountToVisibilityConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var count = int.Parse(value.ToString());
                if (count > 0)
                {
                    return Visibility.Visible;
                }
            }
            catch
            {

            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
