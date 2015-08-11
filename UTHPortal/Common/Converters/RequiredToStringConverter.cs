using System;
using Windows.UI.Xaml.Data;

namespace UTHPortal.Common.Converters
{
    class RequiredToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool required = (bool)value;

            if (required) {
                return "Yποχρεωτικό";
            }
            else {
                return "Eπιλογής";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
