using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTHPortal.Common;
using UTHPortal.Models;
using Windows.UI.ViewManagement;
using Windows.UI.Popups;
using System.Collections.ObjectModel;

namespace UTHPortal.ViewModel
{
    public class CourseViewModel : UpdatableViewModel<CourseModel>
    {
        private int _pivotSelectedIndex;
        public int PivotSelectedIndex
        {
            get { return _pivotSelectedIndex; }
            set { Set(() => PivotSelectedIndex, ref _pivotSelectedIndex, value);}
        }

        public CourseViewModel()
        {
            if (IsInDesignMode)
            {
                Data = new CourseModel();
                Data.Code = "ce213";
                Data.Info = new CourseInfoModel() { Link = "http://inf.uth.gr/cat=5&id=233", Name = "Αριθμητική Ανάλυση" };
                Data.Announcements = new CourseAnnounceModel();
                Data.Announcements.Eclass = new List<CourseAnnounce>();
                Data.Announcements.Site = new List<CourseAnnounce>();

                for (int i = 0; i < 5; i++)
                {
                    CourseAnnounce ca = new CourseAnnounce();
                    ca.Date = DateTime.Now;
                    ca.HasTime = false;
                    ca.Plaintext = "Τα τμήματα εξέτασης του εργαστηριακού μέρους του μαθήματος βρίσκονται εδώ.\n   Κατά την εξέταση θα χρειαστείτε τη φοιτητική σας ταυτότητα.";
                    ca.Title = "CE213";

                    Data.Announcements.Eclass.Add(ca);
                    Data.Announcements.Site.Add(ca);
                }
            }
        }

        protected override async void ExecutePageLoaded()
        {
            if (_navigationService.StateExists(this.GetType()))
            {
                /* TODO: Find a better place for those 2 */
                RefreshedSuccessfull = false;
                SavedViewAvailable = false;

                /* Create the selected course url */
                Url = String.Format(
                    RestAPI.InfDeptCourse,
                    (string)_navigationService.GetAndRemoveState(this.GetType()));


                /* Check if we have a saved view for the current course */
                await GetSavedView();

                /* Perform the refresh */
                bool AutoRefresh = (bool)_storageService.GetSettingsEntry("AutoRefresh");
                if (AutoRefresh) {
                    await DispatcherHelper.RunAsync(() => {
                        RefreshCommand.Execute(null);
                    });
                }
                else {
                    await ValidateDisplayData();
                }
            }
        }

        protected override async Task ValidateDisplayData()
        {
            bool AutoRefresh = (bool)_storageService.GetSettingsEntry("AutoRefresh");

            if (!RefreshedSuccessfull && !SavedViewAvailable) {
                /* At this point I have to data to display */

                await _viewService.ShowMessageDialog(
                        "Δεν υπάρχουν αποθηκευμένα δεδομένα για το συγκεκριμένο μάθημα",
                        "Πρόβλημα!");

                /* Means we already tried to update */
                if (AutoRefresh) {
                    _navigationService.GoBack();
                }
            }
            else {
                /* If site has no announcements, then go to eclass tab */
                PivotSelectedIndex = 0;
                if (Data.Announcements.Site.Count == 0 &&
                    Data.Announcements.Eclass.Count > 0) {
                    PivotSelectedIndex = 1;
                }

                /* If no announcements are retrieved, then go back */
                if (Data.Announcements.Site.Count == 0 &&
                    Data.Announcements.Eclass.Count == 0) {
                    await _viewService.ShowMessageDialog(
                        "Δεν υπάρχουν διαθέσιμες ανακοινώσεις αυτή την στιγμή",
                        Data.Info.Name);

                    _navigationService.GoBack();
                }
            }
        }

    }
}
