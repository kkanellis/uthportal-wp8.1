using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTHPortal.Models
{
    public class FoodmenuModel : ObservableObject
    {
        [JsonProperty("menu")]
        public IList<DayMenu> Days { get; set; }

        [JsonProperty("first_updated")]
        public DateTime FirstUpdated { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
    }

    public class DayMenu
    {
        [JsonProperty("lunch")]
        public Meal Lunch { get; set; }

        [JsonProperty("dinner")]
        public Meal Dinner { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("name")]
        public string DayName { get; set; }
    }

    public class Meal
    {
        [JsonProperty("main")]
        public string Main { get; set; }

        [JsonProperty("salad")]
        public string Salad { get; set; }

        [JsonProperty("desert")]
        public string Desert { get; set; }
    }
}
