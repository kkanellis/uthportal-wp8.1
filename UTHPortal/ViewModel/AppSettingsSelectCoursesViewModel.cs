using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTHPortal.Common;
using UTHPortal.Models;

namespace UTHPortal.ViewModel
{
    public class AppSettingsSelectCoursesViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private IStorageService _storageService;
        private IDataService _dataService;

        private ObservableCollection<CourseModelChecked> _courses;
        public ObservableCollection<CourseModelChecked> Courses
        {
            get { return _courses; }
            set { Set(() => Courses, ref _courses, value); }
        }

        public AppSettingsSelectCoursesViewModel()
        {
            if (IsInDesignMode)
            {
                Courses = new ObservableCollection<CourseModelChecked>();
                for (int i = 0; i < 5; i++)
                {
                    CourseModelChecked course = new CourseModelChecked();
                    course.Info = new CourseInfoModel();
                    course.Info.Name = "Προγραμματισμός Ι";
                    course.Info.Link = "http://inf-server.inf.uth.gr/courses/CE120";
                    course.IsChecked = (i % 2 == 0) ? true : false;

                    Courses.Add(course);
                }
            }
            else
            {
                _navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
                _storageService = SimpleIoc.Default.GetInstance<IStorageService>();
                _dataService = SimpleIoc.Default.GetInstance<IDataService>();
            }
        }

        private RelayCommand _pageLoaded;

        /// <summary>
        /// Gets the PageLoaded.
        /// </summary>
        public RelayCommand PageLoaded
        {
            get
            {
                return _pageLoaded
                    ?? (_pageLoaded = new RelayCommand(
                        async () =>
                        {
                            List<CourseModel> selectedCourses = null;
                            if(_navigationService.StateExists(this.GetType())){
                                selectedCourses = (List<CourseModel>)_navigationService.GetAndRemoveState(this.GetType());
                            }
                            
                            Courses = new ObservableCollection<CourseModelChecked>();
                            string json = await _storageService.GetAPIData(RestAPI.InfDeptCourses);

                            if (json == null)
                            {
                                await _dataService.RefreshAndSave(RestAPI.InfDeptCourses, typeof(CourseAllModel));
                                json = await _storageService.GetAPIData(RestAPI.InfDeptCourses);

                            }

                            var fullCourses = ((CourseAllModel)_dataService.ParseJson(json, typeof(CourseAllModel))).Courses;
                            foreach (CourseModel course in fullCourses)
                            {
                                CourseModelChecked entry = new CourseModelChecked();
                                entry.Code = course.Code;
                                entry.Info = course.Info;
                                entry.IsChecked = false;

                                if (selectedCourses != null)
                                {
                                    var foundCourse = selectedCourses.Find(
                                        selectedCourse => selectedCourse.Info.Name.Equals(entry.Info.Name));
                                    
                                    if (foundCourse != null)
                                    {
                                        entry.IsChecked = true;
                                    }
                                }

                                Courses.Add(entry);
                            }
                        }));
            }
        }

        private RelayCommand _saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new RelayCommand(
                                          () =>
                                          {
                                              List<CourseModel> selectedCourses = new List<CourseModel>();
                                              foreach (CourseModelChecked course in Courses)
                                              {
                                                  if (course.IsChecked)
                                                  {
                                                      CourseModel baseCourse = new CourseModel();
                                                      baseCourse.Info = course.Info;
                                                      baseCourse.Code = course.Code;

                                                      selectedCourses.Add(baseCourse);
                                                  }
                                              }

                                              Messenger.Default.Send<List<CourseModel>>(selectedCourses, "SelectCourses");
                                              _navigationService.GoBack();
                                          }));
            }
        }

    }
}
