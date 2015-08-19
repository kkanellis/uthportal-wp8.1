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
        public IList<AnnounceEx> AllAnnouncements
        {
            get { return _allAnnouncements; }
            set { Set(() => AllAnnouncements, ref _allAnnouncements, value); }
        }
        private IList<AnnounceEx> _allAnnouncements;

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
                    Name = "Αριθμητική Ανάλυση",
                    Instructor="Τσομπανοπούλου Παναγιώτα"
                };

                AllAnnouncements = new List<AnnounceEx>();
                for (int i = 0; i < 4; i++) {
                    var ca = new AnnounceEx();
                    var cb = new AnnounceEx();

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
                // TODO: Find a better place for those 2 
                RemoteDataAvailable = false;
                LocalDataAvailable = false;

                // Create the selected course url
                Url = String.Format(
                    RestAPI.InfDeptCourseUrl,
                    (string)navigationService.GetAndRemoveState(this.GetType())
                );

                // Check if we have a saved view for the current course
                AllAnnouncements = new List<AnnounceEx>();
                await RetrieveSavedView();

                // Perform the refresh
                await DispatcherHelper.RunAsync(() => {
                    RefreshCommand.Execute(null);
                });
            }
        }

        protected override async Task Postproccess()
        {
            AllAnnouncements = new List<AnnounceEx>();

            // Populate the AllAnnouncements collection
            await Task.Run(() => {
                if (Data.Announcements.Site != null) {
                    foreach (Announce announce in Data.Announcements.Site) {
                        var newAnnounce = new AnnounceEx(announce);
                        newAnnounce.Source = "ιστοσελίδα";

                        AllAnnouncements.Add(newAnnounce);
                    }
                }

                if (Data.Announcements.Eclass != null) {
                    foreach (Announce announce in Data.Announcements.Eclass) {
                        var newAnnounce = new AnnounceEx(announce);
                        newAnnounce.Source = "eclass";

                        AllAnnouncements.Add(newAnnounce);
                    }
                }

                // Sort the collection
                var sortedAnnouncements = AllAnnouncements.OrderBy(announce => announce.Date)
                                                   .Reverse()
                                                   .ToList();
                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    AllAnnouncements = sortedAnnouncements;
                });
            });
        }

        protected override async Task ValidateView()
        {
            if (!RemoteDataAvailable && !LocalDataAvailable) {
                // At this point I have no data to display

                await viewService.ShowMessageDialog(
                    "Δεν υπάρχουν αποθηκευμένα δεδομένα για το συγκεκριμένο μάθημα",
                    "Πρόβλημα!"
                );

                navigationService.GoBack();
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
            }

        }

    }
    
    
}
