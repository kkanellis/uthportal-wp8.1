using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTHPortal.Common;
using UTHPortal.Models;
using UTHPortal.Views;
using Windows.UI.ViewManagement;

namespace UTHPortal.ViewModel
{
    public class CourseListViewModel : UpdatableViewModel<CourseAllModel>
    {
        private IEnumerable<CourseModel> _selectedCourses;
        public IEnumerable<CourseModel> SelectedCourses
        {
            get { return _selectedCourses; }
            set { Set(() => SelectedCourses, ref _selectedCourses, value); }
        }

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
                                              _navigationService.NavigateTo(
                                                  typeof(CourseView),
                                                  typeof(CourseViewModel),
                                                  course.Code);
                                          }));
            }
        }

        protected override async void ExecutePageLoaded()
        {
            if (_navigationService.StateExists(this.GetType()))
            {
                Url = (string)_navigationService.GetAndRemoveState(this.GetType());

                await GetSavedView();

                /* Get the selected courses */
                SelectedCourses = (List<CourseModel>)_storageService.GetSettingsEntry("SelectedCourses");

                if ((bool)_storageService.GetSettingsEntry("AutoRefresh"))
                {
                    RefreshCommand.Execute(null);
                }
            }
        }
    }
}
