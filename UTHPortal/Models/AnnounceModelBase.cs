using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace UTHPortal.Models
{
    public class AnnounceModelBase : ObservableObject
    {
        private ObservableCollection<Announce> _Entries;

        [JsonProperty("entries")]
        public virtual ObservableCollection<Announce> Entries
        {
            get { return _Entries; }
            set { Set(() => Entries, ref _Entries, value); }
        }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("first_updated")]
        public DateTime FirstUpdated { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("link")]
        public String Link { get; set; }
       
    }

    public class Announce
    {
        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("plaintext")]
        public String Plaintext { get; set; }

        [JsonProperty("html")]
        public String Html { get; set; }

        [JsonProperty("link")]
        public String Link { get; set; }
    }
}
