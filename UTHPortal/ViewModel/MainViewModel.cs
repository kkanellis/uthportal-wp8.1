﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.ObjectModel;
using UTHPortal.Common;
using UTHPortal.Models;
using UTHPortal.Views;
using Windows.UI.Xaml.Media.Imaging;
using System;
using Windows.System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

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
        private INavigationService navigationService;
        private IStorageService storageService;
        private IViewService viewService;
        private IDataService dataService;

        public List<AnnounceEx> Newsfeed
        {
            get { return _newsfeed; }
            set { Set(() => Newsfeed, ref _newsfeed, value); }
        }
        private List<AnnounceEx> _newsfeed;


        public ObservableCollection<MainListItem> UniversityButtonList
        {
            get { return _universityButtonList; }
            set { Set(() => UniversityButtonList, ref _universityButtonList, value); }
        }
        private ObservableCollection<MainListItem> _universityButtonList;

        public ObservableCollection<MainListItem> DeptButtonList
        {
            get { return _deptButtonList; }
            set { Set(() => DeptButtonList, ref _deptButtonList, value); }
        }
        private ObservableCollection<MainListItem> _deptButtonList;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            UniversityButtonList = new ObservableCollection<MainListItem>();

            UniversityButtonList.Add(new MainListItem() {
                Label = "Ανακοινώσεις",
                Info = RestAPI.GetItem("uth.announce.news"),
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.social.uservoice.png", UriKind.Absolute)),
                Click = UniversityAnnounceClick,
                IsImplemented = true
            });
            UniversityButtonList.Add(new MainListItem() {
                Label = "Ειδήσεις",
                Info = RestAPI.GetItem("uth.announce.news"),
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.newspaper.png", UriKind.Absolute)),
                Click = UniversityAnnounceClick,
                IsImplemented = true
            });
            UniversityButtonList.Add(new MainListItem() {
                Label = "Εκδηλώσεις",
                Info = RestAPI.GetItem("uth.announce.events"),
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.calendar.png", UriKind.Absolute)),
                Click = UniversityAnnounceClick,
                IsImplemented = true
            });
            UniversityButtonList.Add(new MainListItem(){
                Label = "Μενού Λέσχης",
                Info = RestAPI.GetItem("uth.foodmenu"),
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.food.silverware.cross.png", UriKind.Absolute)),
                Click = ShowFoodmenu,
                IsImplemented = true
            });
            UniversityButtonList.Add(new MainListItem(){
                Label = "Πληροφορίες",
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.information.png", UriKind.Absolute)),
                IsImplemented = false
            });

            DeptButtonList = new ObservableCollection<MainListItem>();
            DeptButtonList.Add(new MainListItem(){
                Label = "Ανακοινώσεις",
                Info = RestAPI.GetItem("inf.announce.general"),
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.social.uservoice.png", UriKind.Absolute)),
                Click = UniversityAnnounceClick,
                IsImplemented = true
            });
            DeptButtonList.Add(new MainListItem(){
                Label = "Μαθήματα",
                Info = RestAPI.GetItem("inf.courses"),
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/appbar.book.hardcover.open.png", UriKind.Absolute)),
                Click = CourseListClick,
                IsImplemented = true
            });
            /*DeptButtonList.Add(new TileModel(){
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
            });*/

            if (!IsInDesignMode)
            {
                navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
                storageService = SimpleIoc.Default.GetInstance<IStorageService>();
                viewService = SimpleIoc.Default.GetInstance<IViewService>();
                dataService = SimpleIoc.Default.GetInstance<IDataService>();
            }  
        }

        public RelayCommand PageLoaded
        {
            get
            {
                return _pageLoaded
                    ?? (_pageLoaded = new RelayCommand(
                        async () => 
                        {
                            bool firstLaunched = (bool)storageService.GetSettingsEntry("FirstLaunched");
                            if (firstLaunched == false) {
                                await viewService.ShowMessageDialog(
                                    "Η εφαρμόγή θα χρειαστεί πρόσβαση στο Internet, λόγω της πρώτης εκτέλεσης. \n\nΠαρακαλούμε βεβαιωθείτε πως υπάρχει ενεργή σύνδεση.", "Καλωσήρθατε!");

                                // Try to get all courses //
                                var restItem = RestAPI.GetItem("inf.courses");
                                if (await dataService.RefreshAndSave(restItem, typeof(CourseAllModel)) == null) {
                                    await viewService.ShowMessageDialog(
                                        "Δυστυχώς δεν ήταν δυνατή η επικοινωνία με τον server.\n\nΣυνδεθείτε στο Internet και προσπαθήστε αργότερα.", "Πρόβλημα σύνδεσης");
                                    App.Current.Exit();
                                }
                                else {
                                    storageService.SetSettingsEntry("FirstLaunched", true);
                                    await viewService.ShowMessageDialog("Επιτυχής σύνδεση με server! Παρακαλούμε επιλέξτε τις ρυθμίσεις που θέλετε.\n\nΠεριμένουμε τις προτάσεις σας!\n-- Developer team", "Καλώσήρθατε!");
                                    storageService.SetSettingsEntry("NotificationItems", new List<RestAPIItem>());
                                    navigationService.NavigateTo(typeof(AppSettingsView));
                                }
                            }
                            else {
                                generateNewsFeed();
                            }

                        }));
            }
        }
        private RelayCommand _pageLoaded;


        private async void generateNewsFeed()
        {
            await viewService.ShowStatusBar(string.Empty, null);

            var notificationItems = (List<RestAPIItem>)storageService.GetSettingsEntry(AppSettingsModel.notificationItemsStr);

            Newsfeed = new List<AnnounceEx>();
            foreach (var item in notificationItems) {
                Type modelType = Type.GetType(item.ModelTypeName);

                string json = await storageService.LoadJSON(item);

                var model = Convert.ChangeType(dataService.ParseJson(json, modelType), modelType);
                var getAnnouncementsMethod = modelType.GetTypeInfo().GetDeclaredMethod("GetAnnouncements");
                var entries = (List<AnnounceEx>)getAnnouncementsMethod.Invoke(model, new object [] { item.DisplayFormat, item.DisplayParams });

                Newsfeed.AddRange(entries);
            }

            Newsfeed = Newsfeed.OrderBy(announce => announce.Date).Reverse().ToList();

            await viewService.HideStatusBar();
        }


        public RelayCommand<RestAPIItem> UniversityAnnounceClick
        {
            get
            {
                return _universityAnnounceClick
                    ?? (_universityAnnounceClick = new RelayCommand<RestAPIItem>(
                        url =>
                        {
                            navigationService.NavigateTo(
                                typeof(AnnounceListView),
                                typeof(AnnounceListViewModel),
                                url);
                        }));
            }
        }
        private RelayCommand<RestAPIItem> _universityAnnounceClick;

        public RelayCommand<RestAPIItem> CourseListClick
        {
            get
            {
                return _courseListClick
                    ?? (_courseListClick = new RelayCommand<RestAPIItem>(
                        info =>
                        {
                            navigationService.NavigateTo(
                                typeof(CourseListView),
                                typeof(CourseListViewModel),
                                info);
                        }));
            }
        }
        private RelayCommand<RestAPIItem> _courseListClick;


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
        private RelayCommand<string> _IELauncher;


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
                            navigationService.NavigateTo(
                                typeof(AppSettingsView)
                            );
                        }));
            }
        }
        private RelayCommand _showSettings;


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
                                              navigationService.NavigateTo(typeof(AboutView));
                                          }));
            }
        }
        private RelayCommand _showAboutViewCommand;


        /// <summary>
        /// Gets the ShowFoodmenu.
        /// </summary>
        public RelayCommand<RestAPIItem> ShowFoodmenu
        {
            get
            {
                return _showFoodmenu
                    ?? (_showFoodmenu = new RelayCommand<RestAPIItem>(
                                           url => {
                                              navigationService.NavigateTo(
                                                  typeof(FoodmenuView),
                                                  typeof(FoodmenuViewModel),
                                                  url);
                                          }));
            }
        }
        private RelayCommand<RestAPIItem> _showFoodmenu;


    }
}