using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using UTHPortal.Models;

namespace UTHPortal.ViewModel
{
    public class FoodmenuViewModel : UpdatableViewModel<FoodmenuModel>
    {
        /// <summary>
        /// Int representing the selected day fot pivot control.
        /// </summary>
        public int SelectedDay
        {
            get { return _selectedDay; }
            set { Set(() => SelectedDay, ref _selectedDay, value); }
        }
        private int _selectedDay;

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
        }

        protected override async void ExecutePageLoaded()
        {
            if (navigationService.StateExists(this.GetType())) {
                Url = (string)navigationService.GetAndRemoveState(this.GetType());

                await RetrieveSavedView();
                await DispatcherHelper.RunAsync(() => {
                    RefreshCommand.Execute(null);
                });
           }
        }

        protected override async Task ValidateView()
        {
            bool LocalDataValid = LocalDataAvailable && !IsOldMenuSaved();

            if (!RemoteDataAvailable && !LocalDataValid) {
                if (!LocalDataAvailable || IsOldMenuSaved()) {
                    await viewService.ShowMessageDialog(
                        "Δεν έχει ανακοινωθεί ακόμα το μενού της λέσχης.",
                        "Μενού λέσχης"
                    );

                    navigationService.GoBack();
                }
            }
            else if (RemoteDataAvailable || (!RemoteDataAvailable && LocalDataValid)) {
                DayOfWeek today = DateTime.Now.DayOfWeek;

                // Sunday = 0, so we must convert to Monday = 0
                SelectedDay = ((int)today + 6) % 7;
            } 
        }

        private bool IsOldMenuSaved() {
            // Find last monday dateTime
            DateTime LastMonday = DateTime.Now.Subtract(TimeSpan.FromDays(((int)(DateTime.Now.DayOfWeek + 6) % 7)));

            if (LocalDataAvailable && LastMonday.Date != Data.Days[0].Date.Date) {
                return true;
            }
            return false;
        }
    }
}
