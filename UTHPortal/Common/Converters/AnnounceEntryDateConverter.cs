using System;
using Windows.UI.Xaml.Data;
using UTHPortal.Models;

namespace UTHPortal.Common.Converters
{
    public class AnnounceEntryDateConverter : IValueConverter
    {
        string dateFormat = "dddd, dd MMMM, yyyy";
        string datetimeFormat = "dddd, dd MMMM, yyyy HH:mm";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null) {
                var entry = (Announce)value;

                if (entry.HasTime) {
                    return entry.Date.ToString(datetimeFormat);
                }
                else {
                    return entry.Date.ToString(dateFormat);
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
