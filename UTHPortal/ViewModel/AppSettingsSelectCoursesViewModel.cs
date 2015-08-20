using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UTHPortal.Common;
using UTHPortal.Models;

namespace UTHPortal.ViewModel
{
    public class Pair<T, U>
    {
        public T first { get; set; }
        public U second { get; set; }

        public Pair()
        { }

        public Pair(T first, U second)
        {
            this.first = first;
            this.second = second;
        }

        public static Pair<T, U> Create(T first, U second)
        {
            return new Pair<T, U>(first, second);
        }
    }

    public class AppSettingsSelectCoursesViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private IStorageService storageService;
        private IDataService dataService;

        public ObservableCollection<Pair<bool, CourseModel> > Courses
        {
            get { return _courses; }
            set { Set(() => Courses, ref _courses, value); }
        }
        private ObservableCollection<Pair<bool, CourseModel> > _courses;

        public AppSettingsSelectCoursesViewModel()
        {
            if (IsInDesignMode)
            {
                Courses = new ObservableCollection<Pair<bool, CourseModel>>();
                for (int i = 0; i < 5; i++)
                {
                    var course = new CourseModel();
                    course.Info = new CourseInfoModel();
                    course.Info.Name = "Προγραμματισμός Ι";
                    course.Info.LinkSite = "http://inf-server.inf.uth.gr/courses/CE120";

                    Courses.Add(
                        Pair<bool, CourseModel>.Create(
                            i % 2 == 0 ? true :false, 
                            course
                        )
                   );
                }
            }
            else
            {
                navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
                storageService = SimpleIoc.Default.GetInstance<IStorageService>();
                dataService = SimpleIoc.Default.GetInstance<IDataService>();
            }
        }


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
                            var selectedCourses = (List<CourseModel>)navigationService.GetAndRemoveState(this.GetType());

                            Courses = new ObservableCollection<Pair<bool, CourseModel>>();

                            var coursesRestItem = RestAPI.GetItem("inf.courses.");
                            Debug.Assert(coursesRestItem != null, "Can't find saved courses list");

                            string json = await storageService.LoadJSON(coursesRestItem);

                            var fullCourses = ((CourseAllModel)dataService.ParseJson(json, typeof(CourseAllModel))).Courses;

                            if (selectedCourses != null) {
                                foreach (CourseModel course in fullCourses) {
                                    bool isSelected = selectedCourses.Find(sCourse => sCourse.Code == course.Code) != null;

                                    Courses.Add(Pair<bool, CourseModel>.Create(isSelected, course));
                                }
                            }
                            else {
                                foreach(CourseModel course in fullCourses) {
                                    Courses.Add(Pair<bool, CourseModel>.Create(false, course));
                                }
                            }
                        }));
            }
        }
        private RelayCommand _pageLoaded;


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
                                              var selectedCourses = new List<CourseModel>();
                                              foreach (var course in Courses)
                                              {
                                                  if (course.first) {
                                                      selectedCourses.Add(course.second);
                                                  }
                                              }

                                              Messenger.Default.Send(selectedCourses, "SelectCourses");
                                              navigationService.GoBack();
                                          }));
            }
        }
        private RelayCommand _saveCommand;

    }
}
