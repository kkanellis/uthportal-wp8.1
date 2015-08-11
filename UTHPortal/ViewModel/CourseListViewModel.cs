using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using UTHPortal.Models;
using UTHPortal.Views;
using GalaSoft.MvvmLight.Threading;

namespace UTHPortal.ViewModel
{
    public class CourseListViewModel : UpdatableViewModel<CourseAllModel>
    {
        public IEnumerable<CourseModel> SelectedCourses
        {
            get { return _selectedCourses; }
            set { Set(() => SelectedCourses, ref _selectedCourses, value); }
        }
        private IEnumerable<CourseModel> _selectedCourses;

        public IEnumerable<IGrouping<int, CourseModel> > GrouppedCourses
        {
            get { return _grouppedCourses; }
            set { Set(() => GrouppedCourses, ref _grouppedCourses, value); }
        }
        private IEnumerable<IGrouping<int, CourseModel> > _grouppedCourses;

        public CourseListViewModel()
        {
            if (IsInDesignMode) {
                var OrderedCourses = new List<CourseModel>();
                for (int i = 0; i < 5; i++) {
                    var newEntry = new CourseModel();
                    newEntry.Info = new CourseInfoModel();
                    newEntry.Info.Name = "Προγραμματισμός ΙΙ";
                    newEntry.Info.CodeSite = "ΗΜ999";
                    newEntry.Info.Semester = (i / 2 + 1);
                    newEntry.Info.Required = (i % 2 == 0);

                    OrderedCourses.Add(newEntry);
                }

                GrouppedCourses = OrderedCourses.GroupBy(course => course.Info.Semester)
                                                .OrderBy(course => course.Key)
                                                .ToList();
            }
        }

        private RelayCommand<CourseModel> _showDetails;

        /// <summary>
        /// Show the UniversityRssDetailsView based on what item was tapped
        /// </summary>
        public RelayCommand<CourseModel> ShowDetails
        {
            get
            {
                return _showDetails
                    ?? (_showDetails = new RelayCommand<CourseModel>(
                                          course =>
                                          {
                                              navigationService.NavigateTo(
                                                  typeof(CourseView),
                                                  typeof(CourseViewModel),
                                                  course.Code);
                                          }));
            }
        }

        protected override async Task RetrieveSavedView()
        {
            await base.RetrieveSavedView();

            // Get the user-defined courses
            SelectedCourses = (List<CourseModel>)storageService.GetSettingsEntry("SelectedCourses");
        }

        protected override async Task Postproccess()
        {
            await Task.Run(() => {
                var orderedCourses = Data.Courses.OrderBy(course => course.Info.Semester)
                                                 .ThenBy(course => course.Info.Required)
                                                 .ThenBy(course => course.Info.CodeSite)
                                                 .GroupBy(course => course.Info.Semester)
                                                 .OrderBy(couse => couse.Key)
                                                 .ToList();

                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    GrouppedCourses = orderedCourses;
                });
            });
        } 
    }
}
