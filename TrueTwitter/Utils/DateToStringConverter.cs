using System;
using Windows.UI.Xaml.Data;

namespace TrueTwitter.Utils
{
    /// <summary>
    /// Convert DateTime to user readable string
    /// </summary>
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime dt = DateTime.Parse(value.ToString());
            return dt.ToString("dddd dd MMMM yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
