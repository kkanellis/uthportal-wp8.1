using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UTHPortal.Common;
using UTHPortal.Models;
using UTHPortal.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using System;
using Windows.System;

namespace UTHPortal.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private IStorageService _storageService;
        private IViewService _viewService;
        private IDataService _dataService;

        private ObservableCollection<TileModel> _universityButtonList;
        public ObservableCollection<TileModel> UniversityButtonList
        {
            get { return _universityButtonList; }
            set { Set(() => UniversityButtonList, ref _universityButtonList, value); }
        }

        private ObservableCollection<TileModel> _deptButtonList;
        public ObservableCollection<TileModel> DeptButtonList
        {
            get { return _deptButtonList; }
            set { Set(() => DeptButtonList, ref _deptButtonList, value); }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            UniversityButtonList = new ObservableCollection<TileModel>();

            UniversityButtonList.Add(new TileModel(){
                Label = "Ανακοινώσεις",
                Url = RestAPI.UniversityNews,
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.social.uservoice.png", UriKind.Absolute)),
                Click = UniversityAnnounceClick,
                IsImplemented = true
            });
            UniversityButtonList.Add(new TileModel() {
                Label = "Ειδήσεις",
                Url = RestAPI.UniversityNews,
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.newspaper.png", UriKind.Absolute)),
                Click = UniversityAnnounceClick,
                IsImplemented = true
            });
            UniversityButtonList.Add(new TileModel() {
                Label = "Εκδηλώσεις",
                Url = RestAPI.UniversityEvents,
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.calendar.png", UriKind.Absolute)),
                Click = UniversityAnnounceClick,
                IsImplemented = true
            });
            UniversityButtonList.Add(new TileModel(){
                Label = "Μενού Λέσχης",
                Url = RestAPI.UniversityFoodmenu,
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.food.silverware.cross.png", UriKind.Absolute)),
                Click = ShowFoodmenu,
                IsImplemented = true
            });
            UniversityButtonList.Add(new TileModel(){
                Label = "Πληροφορίες",
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.information.png", UriKind.Absolute)),
                IsImplemented = false
            });

            DeptButtonList = new ObservableCollection<TileModel>();
            DeptButtonList.Add(new TileModel(){
                Label = "Ανακοινώσεις",
                Url = RestAPI.InfDeptAnnouncementsGeneral,
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.social.uservoice.png", UriKind.Absolute)),
                Click = UniversityAnnounceClick,
                IsImplemented = true
            });
            DeptButtonList.Add(new TileModel(){
                Label = "Μαθήματα",
                Url = RestAPI.InfDeptCourses,
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.book.hardcover.open.png", UriKind.Absolute)),
                Click = CourseListClick,
                IsImplemented = true
            });
            DeptButtonList.Add(new TileModel(){
                Label = "Email",
                Url = "https://webmail.uth.gr/login.php",
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.email.hardedge.png", UriKind.Absolute)),
                Click = IELauncher,
                IsImplemented = true
            });
            DeptButtonList.Add(new TileModel(){
                Label = "Eclass",
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.calendar.png", UriKind.Absolute)),
                IsImplemented = false
            });

            if (!IsInDesignMode)
            {
                _navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
                _storageService = SimpleIoc.Default.GetInstance<IStorageService>();
                _viewService = SimpleIoc.Default.GetInstance<IViewService>();
                _dataService = SimpleIoc.Default.GetInstance<IDataService>();
            }  

            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private RelayCommand _pageLoaded;
        public RelayCommand PageLoaded
        {
            get
            {
                return _pageLoaded
                    ?? (_pageLoaded = new RelayCommand(
                        async () => 
                        {
                            bool firstLaunched = (bool)_storageService.GetSettingsEntry("FirstLaunched");
                            if (firstLaunched == false) {
                                await _viewService.ShowMessageDialog(
                                    "Η εφαρμόγή θα χρειαστεί πρόσβαση στο Internet, λόγω της πρώτης εκτέλεσης. \n\nΠαρακαλούμε βεβαιωθείτε πως υπάρχει ενεργή σύνδεση.", "Καλωσήρθατε!");
                                
                                // Try to get all courses //
                                if (await _dataService.RefreshAndSave(RestAPI.InfDeptCourses, typeof(CourseAllModel)) == null) {
                                    await _viewService.ShowMessageDialog(
                                        "Δυστυχώς δεν ήταν δυνατή η επικοινωνία με τον server.\n\nΣυνδεθείτε στο Internet και προσπαθήστε αργότερα.", "Πρόβλημα σύνδεσης");
                                    App.Current.Exit();
                                }
                                else {
                                    _storageService.SaveSettingsEntry("FirstLaunched", true);
                                    await _viewService.ShowMessageDialog("Επιτυχής σύνδεση με server! Παρακαλούμε επιλέξτε τις ρυθμίσεις που θέλετε.\n\nΠεριμένουμε τις προτάσεις σας!\n-- Developer team", "Καλώσήρθατε!");
                                    _navigationService.NavigateTo(typeof(AppSettingsView));
                                }
                            }
                        }));
            }
        }



        private RelayCommand<string> _universityAnnounceClick;
        public RelayCommand<string> UniversityAnnounceClick
        {
            get
            {
                return _universityAnnounceClick
                    ?? (_universityAnnounceClick = new RelayCommand<string>(
                        url =>
                        {
                            _navigationService.NavigateTo(
                                typeof(AnnounceListView),
                                typeof(AnnounceListViewModel),
                                url);
                        }));
            }
        }

        private RelayCommand<string> _courseListClick;
        public RelayCommand<string> CourseListClick
        {
            get
            {
                return _courseListClick
                    ?? (_courseListClick = new RelayCommand<string>(
                        url =>
                        {
                            _navigationService.NavigateTo(
                                typeof(CourseListView),
                                typeof(CourseListViewModel),
                                url);
                        }));
            }
        }

        private RelayCommand<string> _IELauncher;

        /// <summary>
        /// Gets the IELauncher.
        /// </summary>
        public RelayCommand<string> IELauncher
        {
            get
            {
                return _IELauncher
                    ?? (_IELauncher = new RelayCommand<string>(
                        async url => {
                            await Launcher.LaunchUriAsync(new Uri(url));
                        }));
            }
        }

        private RelayCommand _showSettings;

        /// <summary>
        /// Gets the ShowSettings.
        /// </summary>
        public RelayCommand ShowSettings
        {
            get
            {
                return _showSettings
                    ?? (_showSettings = new RelayCommand(
                        () =>
                        {
                            _navigationService.NavigateTo(
                                typeof(AppSettingsView)
                            );
                        }));
            }
        }

        private RelayCommand _showAboutViewCommand;

        /// <summary>
        /// Gets the ShowAboutViewCommand.
        /// </summary>
        public RelayCommand ShowAboutViewCommand
        {
            get
            {
                return _showAboutViewCommand
                    ?? (_showAboutViewCommand = new RelayCommand(
                                          () =>
                                          {
                                              _navigationService.NavigateTo(typeof(AboutView));
                                          }));
            }
        }

        private RelayCommand<string> _showFoodmenu;

        /// <summary>
        /// Gets the ShowFoodmenu.
        /// </summary>
        public RelayCommand<string> ShowFoodmenu
        {
            get
            {
                return _showFoodmenu
                    ?? (_showFoodmenu = new RelayCommand<string>(
                                           url => {
                                              _navigationService.NavigateTo(
                                                  typeof(FoodmenuView),
                                                  typeof(FoodmenuViewModel),
                                                  url);
                                          }));
            }
        }


    }
}