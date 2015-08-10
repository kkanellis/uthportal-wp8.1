using System;
using Windows.UI.Xaml.Data;

namespace UTHPortal.Common.Converters
{
    class PrettyDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = ((DateTime)value).Date;
            var now = DateTime.Today;

            var dateDiff = (now - date).Days;

            if (dateDiff < 7) {
                return date.ToString("ddd");
            }
            else if(dateDiff < 365) {
                return date.ToString("dd/MM");
            }
            else {
                return date.ToString("yyyy");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
