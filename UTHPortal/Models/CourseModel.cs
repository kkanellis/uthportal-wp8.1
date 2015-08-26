using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UTHPortal.Common;

namespace UTHPortal.Models
{
    public class CourseModel : ObservableObject
    {
        [JsonProperty("announcements")]
        public CourseAnnounceModel Announcements { get; set; }

        [JsonProperty("info")]
        public CourseInfoModel Info { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        public List<AnnounceEx> GetAnnouncements(string displayFormat, string[] displayParams)
        {
            var items = new List<AnnounceEx>();

            // TODO: Truncate strings
            if (Announcements.Site != null) {
                foreach(var announce in Announcements.Site) {
                    var source = Util.ObjectToString(this, displayFormat, displayParams) + "[ιστοσελίδα]";
                    items.Add(new AnnounceEx(announce, source));
                }
            }

            if (Announcements.Eclass != null) {
                foreach(var announce in Announcements.Eclass) {
                    var source = Util.ObjectToString(this, displayFormat, displayParams) + "[eclass]";
                    items.Add(new AnnounceEx(announce, source));
                }
            }

            return items.OrderBy(announce => announce.Date).Reverse().ToList();
        }
    }

    public class CourseAllModel : ObservableObject
    {
        [JsonProperty("children")]
        public ObservableCollection<CourseModel> Courses { get; set; }

        [JsonProperty("collection")]
        public string Collection { get; set; }
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
        public int Semester { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}
