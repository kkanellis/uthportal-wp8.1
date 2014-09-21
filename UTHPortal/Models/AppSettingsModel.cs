using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

        private bool _autoRefresh;
        public bool AutoRefresh
        {
            get {
                _autoRefresh = (bool)_storageService.GetSettingsEntry("AutoRefresh");
                return _autoRefresh;
            }
            set {
                if (_storageService.SaveSettingsEntry("AutoRefresh", value))
                {
                    Set(() => AutoRefresh, ref _autoRefresh, value);
                }
            }
        }

        private List<CourseModel> _selectedCourses;
        public List<CourseModel> SelectedCourses
        {
            get {
                _selectedCourses = (List<CourseModel>)_storageService.GetSettingsEntry("SelectedCourses");
                return _selectedCourses;
            }
            set {
                if (_storageService.SaveSettingsEntry("SelectedCourses", value))
                {
                    Set(() => SelectedCourses, ref _selectedCourses, value);
                }
            }
        }

        private String[] _departments = {"ΗΜΜΥ/ΤΜΗΥΔ (inf)"};
        public String[] Departments
        {
            get { return _departments; }
        }

        private int _selectedDepartment;
        public int SelectedDepartment
        {
            get {
                _selectedDepartment = (int)_storageService.GetSettingsEntry("SelectedDepartment");
                return _selectedDepartment;
            }
            set {
                if (_storageService.SaveSettingsEntry("SelectedDepartment", value))
                {
                    Set(() => SelectedDepartment, ref _selectedDepartment, value);
                }
           }
        }
    }

    public class CourseModelChecked : CourseModel
    {
        public bool IsChecked { get; set; }
    }

}
