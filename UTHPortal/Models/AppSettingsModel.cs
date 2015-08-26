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


        public List<RestAPIItem> NotificationItems
        {
            get
            {
                return (List<RestAPIItem>)storageService.GetSettingsEntry(notificationItemsStr);
            }
            set
            {
                if (storageService.SetSettingsEntry(notificationItemsStr, value)) {
                    Set(() => NotificationItems, ref _notificationItems, value);
                }
            }
        }
        private List<RestAPIItem> _notificationItems;
        public const string notificationItemsStr = "NotificationItems";

        public List<CourseModel> SelectedCourses
        {
            get {
                return (List<CourseModel>)storageService.GetSettingsEntry(selectedCoursesStr);
            }
            set {
                if (storageService.SetSettingsEntry(selectedCoursesStr, value))
                {
                    Set(() => SelectedCourses, ref _selectedCourses, value);
                }
            }
        }
        private List<CourseModel> _selectedCourses;
        private const string selectedCoursesStr = "SelectedCourses";

        private string [] _departments = {"ΗΜΜΥ/ΤΜΗΥΔ (inf)"};
        public string [] Departments
        {
            get { return _departments; }
        }

        public int SelectedDepartment
        {
            get {
                return (int)storageService.GetSettingsEntry(selectedDepartmentStr);
            }
            set {
                if (storageService.SetSettingsEntry(selectedCoursesStr, value))
                {
                    Set(() => SelectedDepartment, ref _selectedDepartment, value);
                }
           }
        }
        private int _selectedDepartment;
        private const string selectedDepartmentStr = "SelectedDepartment";
    }

}
