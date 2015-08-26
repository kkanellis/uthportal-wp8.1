using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace UTHPortal.Models
{
    public class AnnounceModel : ObservableObject
    {
        [JsonProperty("entries")]
        public IList<Announce> Entries
        {
            get { return _entries; }
            set { Set(() => Entries, ref _entries, value); }
        }
        private IList<Announce> _entries;

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("first_updated")]
        public DateTime FirstUpdated { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        public List<AnnounceEx> GetAnnouncements(string displayFormat, string displayParams)
        {
            var items = new List<AnnounceEx>();
            foreach(var entry in Entries) {
                items.Add(new AnnounceEx(entry, string.Format(displayFormat, displayParams)));
            }
            return items;
        }
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
        
        public AnnounceEx(Announce announce, string source)
        {
            // TODO: Find a better way for this!
            Date = announce.Date;
            HasTime = announce.HasTime;
            Html = announce.Html;
            Link = announce.Link;
            Plaintext = announce.Plaintext;
            Title = announce.Title;

            Source = source;
        }

        public AnnounceEx(Announce announce) : 
            this(announce, string.Empty)
        { }

        // Can be site, eclass or email
        public string Source { get; set; }
    }

}
