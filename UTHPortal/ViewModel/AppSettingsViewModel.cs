using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
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

        public AppSettingsModel Settings
        {
            get { return _settings; }
            set { Set(() => Settings, ref _settings, value); }
        }
        private AppSettingsModel _settings;

        public AppSettingsViewModel()
        {
            if (!IsInDesignMode)
            {
                _navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
                _storageService = SimpleIoc.Default.GetInstance<IStorageService>();
                _dataService = SimpleIoc.Default.GetInstance<IDataService>();

                Settings = new AppSettingsModel();

                Messenger.Default.Register<List<CourseModel>>(this, "SelectCourses",
                selectedCourses =>
                {
                    Settings.SelectedCourses = selectedCourses;
                });
            }
        }


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
                            Settings.SelectedCourses
                        );     
                    }));
            }
        }
        private RelayCommand _selectCourses;
    }
}
