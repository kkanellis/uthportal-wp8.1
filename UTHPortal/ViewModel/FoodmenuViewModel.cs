using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTHPortal.Models;

namespace UTHPortal.ViewModel
{
    public class FoodmenuViewModel : UpdatableViewModel<FoodmenuModel>
    {
        /*private string testJosn = @"{ ""date"": ""2014-05-12T00:00:00Z"", ""menu"":[ 
{ ""date"": ""2014-05-12T00:00:00Z"", ""lunch"":{ ""main"":"" 1. ΜΠΑΚΑΛΙΑΡΟΣ 2. ΚΑΛΑΜΑΡΑΚΙΑ ΠΑΤΑΤΕΣ ΦΟΥΡΝΟΥ ΡΙΖΖΟΤΟ "", ""desert"":"" ΦΡΟΥΤΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ "" }, ""dinner"":{ ""main"":"" 1. ΚΟΤΟΠΟΥΛΟ ΚΑΤΣΑΡΟΛΑΣ 2. ΟΜΕΛΕΤΑ 1. ΜΑΝΕΣΤΡΑ 2. ΜΑΚ. ΚΟΦΤΟ "", ""desert"":"" ΦΡΟΥΤΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ ΦΕΤΑ "" }, ""name"":""Monday"" },
{ ""date"": ""2014-05-13T00:00:00Z"", ""lunch"":{ ""main"":"" 1. ΚΟΛΟΚΥΘΑΚΙΑ ΓΕΜΙΣΤΑ 2.ΜΑΚΑΡΟΝΙΑ ΜΕ ΚΙΜΑ "", ""desert"":"" ΦΡΟΥΤΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ ΚΕΦ/ΡΙ "" }, ""dinner"":{ ""main"":"" 1. ΑΡΑΚΑΣ 2. ΜΠΡΙΑΜ "", ""desert"":"" ΦΡΟΥΤΑ Η ΓΛΥΚΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ ΦΕΤΑ "" }, ""name"":""Tuesday"" },
{ ""date"": ""2014-05-14T00:00:00Z"", ""lunch"":{ ""main"":"" 1.ΦΑΣΟΛΑΔΑ 2. ΦΑΚΕΣ "", ""desert"":"" ΦΡΟΥΤΑ Η ΓΛΥΚΑ "", ""salad"":"" ΤΑΡΑΜΑΣ - ΕΛΙΕΣ ΠΙΚΛΕΣ – ΦΕΤΑ "" }, ""dinner"":{ ""main"":"" 1. ΚΟΤΟΠΟΥΛΟ ΛΕΜΟΝΑΤΟ 2. ΜΠΟΥΚΙΕΣ ΚΟΤ/ΛΟΥ ΠΑΝΕ ΠΕΝΝΕΣ ΧΥΛΟΠΙΤΕΣ "", ""desert"":"" ΦΡΟΥΤΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ "" }, ""name"":""Wednesday"" },
{ ""date"": ""2014-05-15T00:00:00Z"", ""lunch"":{ ""main"":"" 1.ΜΟΣΧΑΡΙ ΓΙΟΥΒΕΤΣΙ 2.ΧΟΙΡΙΝΟ ΨΗΤΟ ΚΡΙΘΑΡΑΚΙ ΜΑΚ. ΚΟΦΤΟ "", ""desert"":"" ΦΡΟΥΤΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ ΦΕΤΑ "" }, ""dinner"":{ ""main"":"" 1.ΨΑΡΙ ΦΙΛΕΤΟ ΦΟΥΡΝΟΥ 2. ΣΟΥΠΙΕΣ ΚΡΑΣΑΤΕΣ ΠΑΤΑΤΕΣ ΡΥΖΙ ΜΕ ΛΑΧΑΝΙΚΑ "", ""desert"":"" ΦΡΟΥΤΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ "" }, ""name"":""Thursday"" },
{ ""date"": ""2014-05-16T00:00:00Z"", ""lunch"":{ ""main"":"" ΣΟΥΤΖΟΥΚΑΚΙΑ ΣΜΥΡΝΑΙΙΚΑ ΜΟΥΣΑΚΑΣ ΠΟΥΡΕΣ "", ""desert"":"" ΦΡΟΥΤΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ "" }, ""dinner"":{ ""main"":"" ΦΑΣΟΛΙΑ ΓΙΓΑΝΤΕΣ ΡΕΒΥΘΙΑ "", ""desert"":"" ΦΡΟΥΤΑ Ή ΓΛΥΚΑ "", ""salad"":"" ΤΑΡΑΜΑΣ – ΕΛΙΕΣ ΠΙΚΛΕΣ - ΦΕΤΑ "" }, ""name"":""Friday"" },
{ ""date"": ""2014-05-17T00:00:00Z"", ""lunch"":{ ""main"":"" 1.ΜΠΟΥΤΙ ΚΟΤ/ΛΟ ΨΗΤΟ 2. ΣΝΙΤΣΕΛ ΚΟΤ/ΛΟ ΜΑΚ. ΚΟΦΤΟ "", ""desert"":"" ΦΡΟΥΤΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ ΦΕΤΑ "" }, ""dinner"":{ ""main"":"" ΧΟΙΡΙΝΗ ΣΠΑΛΑ ΚΑΤΣΑΡΟΛΑΣ ΧΟΙΡΙΝΟ ΛΕΜΟΝΑΤΟ ΠΕΝΝΕΣ ΚΡΙΘΑΡΑΚΙ "", ""desert"":"" ΦΡΟΥΤΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ "" }, ""name"":""Saturday"" },
{ ""date"": ""2014-05-18T00:00:00Z"", ""lunch"":{ ""main"":"" 1.ΜΠΡΙΖΟΛΑ ΧΟΙΡΙΝΗ 2.ΜΟΣΧΑΡΙ. ΚΟΚΚΙΝΙΣΤΟ ΡΙΖΟΤΟ "", ""desert"":"" ΦΡΟΥΤΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ "" }, ""dinner"":{ ""main"":"" 1. ΜΕΛΙΤΖΑΝΕΣ ΓΙΑΧΝΙ 2. ΣΠΑΝΑΚΟΡΥΖΟ "", ""desert"":"" ΦΡΟΥΤΑ Η ΓΛΥΚΑ "", ""salad"":"" ΣΑΛΑΤΑ ΕΠΟΧΗΣ ΦΕΤΑ "" }, ""name"":""Sunday"" } ],
   ""last_updated"": ""2014-05-14T23:31:19.298Z"" }";*/

        private int _pivotSelectedIndex;
        public int PivotSelectedIndex
        {
            get { return _pivotSelectedIndex; }
            set { Set(() => PivotSelectedIndex, ref _pivotSelectedIndex, value); }
        }

        public FoodmenuViewModel()
        {
            if (IsInDesignMode)
            {
                Data = new FoodmenuModel();
                Data.Days = new List<DayMenu>();
                Data.LastUpdated = DateTime.Now;

                List<DayMenu> tmpDays = new List<DayMenu>();
                for (int i = 0; i < 7; i++) {
                    DayMenu dayMenu = new DayMenu();
                    dayMenu.Date = DateTime.Now;
                    dayMenu.Lunch = new Meal() {
                        Main = "1.ΜΟΣΧΑΡΙ ΓΙΟΥΒΕΤΣΙ 2.ΧΟΙΡΙΝΟ ΨΗΤΟ ΚΡΙΘΑΡΑΚΙ ΜΑΚ. ΚΟΦΤΟ",
                        Salad = "ΣΑΛΑΤΑ ΕΠΟΧΗΣ ΦΕΤΑ",
                        Desert = " ΦΡΟΥΤΑ "
                    };
                    dayMenu.Dinner = new Meal() {
                        Main = "1.ΨΑΡΙ ΦΙΛΕΤΟ ΦΟΥΡΝΟΥ 2. ΣΟΥΠΙΕΣ ΚΡΑΣΑΤΕΣ ΠΑΤΑΤΕΣ ΡΥΖΙ ΜΕ ΛΑΧΑΝΙΚΑ ",
                        Salad = "ΣΑΛΑΤΑ ΕΠΟΧΗΣ ",
                        Desert = "ΦΡΟΥΤΑ Η ΓΛΥΚΑ "
                    };
                    dayMenu.DayName = ((DayOfWeek)i).ToString().ToLower();

                    tmpDays.Add(dayMenu);
                }

                Data.Days = tmpDays;
            }
            /*else {
                Data = (FoodmenuModel)_dataService.ParseJson(testJosn, typeof(FoodmenuModel));
                ValidateDisplayData();
            }*/
        }

        protected override async void ExecutePageLoaded()
        {
            if (_navigationService.StateExists(this.GetType())) {
                Url = (string)_navigationService.GetAndRemoveState(this.GetType());

                await GetSavedView();

                if ((bool)_storageService.GetSettingsEntry("AutoRefresh")) {
                    RefreshCommand.Execute(null);
                }
                else {
                    ValidateDisplayData();
                }
            }
        }

        protected override async void ValidateDisplayData()
        {
            if (!RefreshedSuccessfull) {
                /* Get AutoRefresh settings entry */
                bool AutoRefresh = (bool)_storageService.GetSettingsEntry("AutoRefresh");

                if (!SavedViewAvailable || IsOldMenuSaved()) {
                    await _viewService.ShowMessageDialog(
                        "Δεν έχει ανακοινωθεί ακόμα το μενού της λέσχης.",
                        "Μενού λέσχης");

                    if (AutoRefresh) {
                        _navigationService.GoBack();
                    }
                }
                else {
                    /* Foodmenu is shown */
                    DayOfWeek today = DateTime.Now.DayOfWeek;

                    /* Sunday = 0, so we must convert to Monday = 0 */
                    PivotSelectedIndex = ((int)today + 6) % 7;
                }
            }
            else {
                /* Foodmenu is shown */
                DayOfWeek today = DateTime.Now.DayOfWeek;

                /* Sunday = 0, so we must convert to Monday = 0 */
                PivotSelectedIndex = ((int)today + 6) % 7;
            }
        }

        private bool IsOldMenuSaved() {
            /* Find last monday dateTime */
            DateTime LastMonday = DateTime.Now.Subtract(TimeSpan.FromDays(((int)(DateTime.Now.DayOfWeek + 6) % 7)));

            if (SavedViewAvailable && LastMonday.Date != Data.Days[0].Date.Date) {
                return true;
            }
            return false;
        }
    }
}
