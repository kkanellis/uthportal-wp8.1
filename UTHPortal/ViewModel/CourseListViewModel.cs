using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using UTHPortal.Models;
using UTHPortal.Views;

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

        public CourseListViewModel()
        {
            if (IsInDesignMode)
            {
                Data = new CourseAllModel();
                Data.Courses = new ObservableCollection<CourseModel>();
                for (int i = 0; i < 5; i++)
                {
                    CourseModel newEntry = new CourseModel();
                    newEntry.Info = new CourseInfoModel();
                    newEntry.Info.Name = "Προγραμματισμός ΙΙ";
                    newEntry.Code = "CE121";

                    Data.Courses.Add(newEntry);
                }
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

        protected override async void ExecutePageLoaded()
        {
            if (navigationService.StateExists(this.GetType()))
            {
                Url = (string)navigationService.GetAndRemoveState(this.GetType());

                await GetSavedView();

                /* Get the selected courses */
                SelectedCourses = (List<CourseModel>)storageService.GetSettingsEntry("SelectedCourses");

                if ((bool)storageService.GetSettingsEntry("AutoRefresh"))
                {
                    RefreshCommand.Execute(null);
                }
            }
        }
    }
}
