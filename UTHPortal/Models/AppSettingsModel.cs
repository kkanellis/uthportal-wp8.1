using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;
using UTHPortal.Common;

namespace UTHPortal.Models
{
    public class AppSettingsModel : ObservableObject
    {
        private IStorageService storageService;

        public AppSettingsModel()
        {
            storageService = SimpleIoc.Default.GetInstance<IStorageService>();
        }

        public bool FirstLaunched { get; set; }

        public List<PushEvent> ActivePushEvents
        {
            get
            {
                return (List<PushEvent>)storageService.GetSettingsEntry("ActivePushEvents");
            }
            set
            {
                if (storageService.SetSettingsEntry("ActivePushEvents", value)) {
                    Set(() => ActivePushEvents, ref _activePushEvents, value);
                }
            }
        }
        private List<PushEvent> _activePushEvents;

        public List<CourseModel> SelectedCourses
        {
            get {
                return (List<CourseModel>)storageService.GetSettingsEntry("SelectedCourses");
            }
            set {
                if (storageService.SetSettingsEntry("SelectedCourses", value))
                {
                    Set(() => SelectedCourses, ref _selectedCourses, value);
                }
            }
        }
        private List<CourseModel> _selectedCourses;

        private string [] _departments = {"ΗΜΜΥ/ΤΜΗΥΔ (inf)"};
        public string [] Departments
        {
            get { return _departments; }
        }

        public int SelectedDepartment
        {
            get {
                return (int)storageService.GetSettingsEntry("SelectedDepartment");
            }
            set {
                if (storageService.SetSettingsEntry("SelectedDepartment", value))
                {
                    Set(() => SelectedDepartment, ref _selectedDepartment, value);
                }
           }
        }
        private int _selectedDepartment;
    }

}
