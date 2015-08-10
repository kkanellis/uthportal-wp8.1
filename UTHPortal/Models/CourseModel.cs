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

        [JsonProperty("instructor")]
        public string Instructor { get; set; }

        [JsonProperty("code_site")]
        public string CodeSite { get; set; }

        [JsonProperty("code_eclass")]
        public string CodeEclass { get; set; }

        [JsonProperty("link_site")]
        public string LinkSite { get; set; }

        [JsonProperty("link_eclass")]
        public string LinkEclass { get; set; }

        [JsonProperty("semester")]
        public Int32 Semester { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}
