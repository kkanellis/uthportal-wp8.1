using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTHPortal.Common;
using UTHPortal.Models;
using UTHPortal.Views;

namespace UTHPortal.ViewModel
{
    public class AppSettingsViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private IStorageService _storageService;
        private IDataService _dataService;

        private AppSettingsModel _data;
        public AppSettingsModel Data
        {
            get { return _data; }
            set { Set(() => Data, ref _data, value); }
        }

        public AppSettingsViewModel()
        {
            if (!IsInDesignMode)
            {
                _navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
                _storageService = SimpleIoc.Default.GetInstance<IStorageService>();
                _dataService = SimpleIoc.Default.GetInstance<IDataService>();

                Data = new AppSettingsModel(_storageService);
                Messenger.Default.Register<List<CourseModel>>(this, "SelectCourses",
                selectedCourses =>
                {
                    Data.SelectedCourses = selectedCourses;
                });
            }
        }

        private RelayCommand _selectCourses;

        /// <summary>
        /// Gets the SelectCourses.
        /// </summary>
        public RelayCommand SelectCourses
        {
            get
            {
                return _selectCourses
                    ?? (_selectCourses = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(
                            typeof(AppSettingsSelectCoursesView),
                            typeof(AppSettingsSelectCoursesViewModel),
                            Data.SelectedCourses
                        );     
                    }));
            }
        }
    }
}
