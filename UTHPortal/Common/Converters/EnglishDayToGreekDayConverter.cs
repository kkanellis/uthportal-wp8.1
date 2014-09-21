using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UTHPortal.Common.Converters
{
    public class EnglishDayToGreekDayConverter : IValueConverter
    {
        Dictionary<string, string> Days;

        public EnglishDayToGreekDayConverter()
        {
            Days = new Dictionary<string, string>();
            Days.Add("sunday", "κυριακή");
            Days.Add("monday", "δευτέρα");
            Days.Add("tuesday", "τρίτη");
            Days.Add("wednesday", "τετάρτη");
            Days.Add("thursday", "πέμπτη");
            Days.Add("friday", "παρασκευή");
            Days.Add("saturday", "σάββατο");
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //return Days[((string)value).ToLower()];
            return value.ToString().ToLower();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
