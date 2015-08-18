using GalaSoft.MvvmLight;
using System.Collections.Generic;
using UTHPortal.Common;

namespace UTHPortal.Models
{
    public class AppSettingsModel : ObservableObject
    {
        private IStorageService _storageService;

        public AppSettingsModel(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public bool FirstLaunched { get; set; }

        public List<CourseModel> SelectedCourses
        {
            get {
                _selectedCourses = (List<CourseModel>)_storageService.GetSettingsEntry("SelectedCourses");
                return _selectedCourses;
            }
            set {
                if (_storageService.SetSettingsEntry("SelectedCourses", value))
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
                _selectedDepartment = (int)_storageService.GetSettingsEntry("SelectedDepartment");
                return _selectedDepartment;
            }
            set {
                if (_storageService.SetSettingsEntry("SelectedDepartment", value))
                {
                    Set(() => SelectedDepartment, ref _selectedDepartment, value);
                }
           }
        }
        private int _selectedDepartment;
    }

    public class CourseModelChecked : CourseModel
    {
        public bool IsChecked { get; set; }
    }

}
