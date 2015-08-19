using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace UTHPortal.Models
{
    public class AnnounceModel : ObservableObject
    {
        private ObservableCollection<Announce> _Entries;

        [JsonProperty("entries")]
        public virtual ObservableCollection<Announce> Entries
        {
            get { return _Entries; }
            set { Set(() => Entries, ref _Entries, value); }
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("first_updated")]
        public DateTime FirstUpdated { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
       
    }

    public class Announce
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("plaintext")]
        public string Plaintext { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("has_time")]
        public bool HasTime { get; set; }
    }

    public class AnnounceEx : Announce
    {
        public AnnounceEx()
        { }

        public AnnounceEx(Announce announce)
        {
            // TODO: Find a better way for this!
            Date = announce.Date;
            HasTime = announce.HasTime;
            Html = announce.Html;
            Link = announce.Link;
            Plaintext = announce.Plaintext;
            Title = announce.Title;
        }

        // Can be site, eclass or email
        public string Source { get; set; }
    }

}
