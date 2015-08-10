using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTHPortal.Models
{
    public class CourseAnnounceModel : ObservableObject 
    {
        [JsonProperty("emails")]
        public IList<Announce> Emails { get; set; }

        [JsonProperty("site")]
        public IList<Announce> Site { get; set; }

        [JsonProperty("eclass")]
        public IList<Announce> Eclass { get; set; }

        [JsonProperty("first_updated")]
        public DateTime FirstUpdated { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("link_site")]
        public string LinkSite { get; set; }

        [JsonProperty("link_eclass")]
        public string LinkEclass { get; set; }
    }
    
}
