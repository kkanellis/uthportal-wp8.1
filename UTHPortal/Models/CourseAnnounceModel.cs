﻿using GalaSoft.MvvmLight;
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
        public IList<CourseAnnounce> Emails { get; set; }

        [JsonProperty("site")]
        public IList<CourseAnnounce> Site { get; set; }

        [JsonProperty("eclass")]
        public IList<CourseAnnounce> Eclass { get; set; }

        [JsonProperty("first_updated")]
        public DateTime FirstUpdated { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("link_site")]
        public string LinkSite { get; set; }

        [JsonProperty("link_eclass")]
        public string LinkEclass { get; set; }
    }

    public class CourseAnnounce : Announce
    {
        [JsonProperty("has_time")]
        public bool HasTime { get; set; }
    }
}
