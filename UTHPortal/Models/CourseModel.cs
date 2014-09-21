using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UTHPortal.Models
{
    public class CourseModel : ObservableObject
    {
        /* Adding xmlignore to not serialize this part to LocalSettings */
        [XmlIgnore()]
        [JsonProperty("announcements")]
        public CourseAnnounceModel Announcements { get; set; }

        [JsonProperty("info")]
        public CourseInfoModel Info { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class CourseAllModel : ObservableObject
    {
        [JsonProperty("children")]
        public ObservableCollection<CourseModel> Courses { get; set; }
    }

    public class CourseInfoModel : ObservableObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
