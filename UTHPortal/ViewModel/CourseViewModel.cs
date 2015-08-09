using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using UTHPortal.Common;
using UTHPortal.Models;

namespace UTHPortal.ViewModel
{
    public class CourseViewModel : UpdatableViewModel<CourseModel>
    {
        /// <summary>
        /// List that contains all announcements sorted by date.
        /// </summary>
        public IList<CourseAnnounceEx> AllAnnouncements
        {
            get { return _allAnnouncements; }
            set { Set(() => AllAnnouncements, ref _allAnnouncements, value); }
        }
        private IList<CourseAnnounceEx> _allAnnouncements;

        public CourseViewModel()
        {
            if (IsInDesignMode) {
                Data = new CourseModel();
                Data.Code = "ce213";
                Data.Info = new CourseInfoModel()
                {
                    CodeSite = "ΗΥ213",
                    CodeEclass = "MHX118",
                    Semester = 4,
                    LinkEclass = "http://eclass.uth.gr/eclass/courses/MHX118/",
                    LinkSite = "http://inf.uth.gr/cat=5&id=233",
                    Name = "Αριθμητική Ανάλυση"
                };

                AllAnnouncements = new List<CourseAnnounceEx>();
                for (int i = 0; i < 4; i++) {
                    var ca = new CourseAnnounceEx();
                    var cb = new CourseAnnounceEx();

                    ca.Date = new DateTime(2015, 8 - i, 9);
                    cb.Date = new DateTime(2015, 7 - i, 13);
                    ca.HasTime = cb.HasTime = false;
                    ca.Title = cb.Title = "HY213";

                    ca.Plaintext = "Aenean consectetur posuere arcu sodales viverra. Phasellus facilisis lacinia est, eget cursus magna tempus et. Nam non scelerisque mauris. Nulla facilisi. Praesent efficitur magna eget nisi varius, a bibendum turpis feugiat. . Aliquam feugiat orci a sem pharetra, vitae consectetur diam porta. Nullam pulvinar felis ac consectetur luctus. Integer eget iaculis ipsum, sit amet accumsan neque.";
                    ca.Source = "ιστοσελίδα";
                    AllAnnouncements.Add(ca);

                    cb.Plaintext = "Fusce vitae tincidunt ante.Mauris at tortor lobortis, tincidunt arcu ac, consectetur neque.Etiam maximus, justo ut rutrum iaculis, nisi mauris tempor mi, sit amet aliquet orci magna id nulla. Lorem ipsum dolor sit amet, consectetur adipiscing elit";
                    cb.Source = "eclass";
                    AllAnnouncements.Add(cb);
                }
            }
        }

        protected override async void ExecutePageLoaded()
        {
            if (navigationService.StateExists(this.GetType())) {
                /* TODO: Find a better place for those 2 */
                RemoteDataAvailable = false;
                LocalDataAvailable = false;

                // Create the selected course url
                Url = String.Format(
                    RestAPI.InfDeptCourse,
                    (string)navigationService.GetAndRemoveState(this.GetType())
                );

                // Check if we have a saved view for the current course
                AllAnnouncements = new List<CourseAnnounceEx>();
                await GetSavedView();

                // Perform the refresh
                bool AutoRefresh = (bool)storageService.GetSettingsEntry("AutoRefresh");
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

            bool AutoRefresh = (bool)storageService.GetSettingsEntry("AutoRefresh");
            if (!RemoteDataAvailable && !LocalDataAvailable) {
                // At this point I have no data to display

                await viewService.ShowMessageDialog(
                    "Δεν υπάρχουν αποθηκευμένα δεδομένα για το συγκεκριμένο μάθημα",
                    "Πρόβλημα!"
                );

                /* Means we already tried to update */
                if (AutoRefresh) {
                    navigationService.GoBack();
                }
            }
            else {

                /* If no announcements are retrieved, then go back */
                if ((Data.Announcements.Site == null || Data.Announcements.Site.Count == 0) &&
                    (Data.Announcements.Eclass == null || Data.Announcements.Eclass.Count == 0)) {
                    await viewService.ShowMessageDialog(
                        "Δεν υπάρχουν διαθέσιμες ανακοινώσεις αυτή την στιγμή",
                        Data.Info.Name
                    );

                    navigationService.GoBack();
                }
                else {
                    // At this point Data should NOT be null!

                    // Populate the AllAnnouncements collection
                    AllAnnouncements.Clear();
                    if (Data.Announcements.Site != null) {
                        foreach (CourseAnnounce announce in Data.Announcements.Site) {
                            var newAnnounce = new CourseAnnounceEx(announce);
                            newAnnounce.Source = "ιστοσελίδα";

                            AllAnnouncements.Add(newAnnounce);
                        }
                    }

                    if (Data.Announcements.Eclass != null) {
                        foreach (CourseAnnounce announce in Data.Announcements.Eclass) {
                            var newAnnounce = new CourseAnnounceEx(announce);
                            newAnnounce.Source = "eclass";

                            AllAnnouncements.Add(newAnnounce);
                        }
                    }

                    // Sort the collection
                    AllAnnouncements = AllAnnouncements.OrderBy(announce => announce.Date)
                                                       .Reverse()
                                                       .ToList();
                }
            }

        }

    }
    
    public class CourseAnnounceEx : CourseAnnounce
    {
        public CourseAnnounceEx()
        { }

        public CourseAnnounceEx(CourseAnnounce announce)
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
        public string Source
        { get; set; }
    }
}
