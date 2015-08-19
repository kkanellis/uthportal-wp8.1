/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:UTHPortal"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using UTHPortal.Common;

namespace UTHPortal.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<IStorageService, StorageService>();
            SimpleIoc.Default.Register<IPushService, PushService>();
            SimpleIoc.Default.Register<IViewService, ViewService>();
            SimpleIoc.Default.Register<ILoggerService, LoggerService>();

            SimpleIoc.Default.Register<AboutViewModel>();

            SimpleIoc.Default.Register<AppSettingsViewModel>();
            SimpleIoc.Default.Register<AppSettingsSelectCoursesViewModel>();

            SimpleIoc.Default.Register<AnnounceListViewModel>();
            SimpleIoc.Default.Register<AnnounceListDetailsViewModel>();
            SimpleIoc.Default.Register<CourseListViewModel>();
            SimpleIoc.Default.Register<CourseViewModel>();
            SimpleIoc.Default.Register<FoodmenuViewModel>();

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public AppSettingsViewModel AppSettings
        {
            get { return ServiceLocator.Current.GetInstance<AppSettingsViewModel>(); }
        }

        public AppSettingsSelectCoursesViewModel AppSettingsSelectCourses
        {
            get { return ServiceLocator.Current.GetInstance<AppSettingsSelectCoursesViewModel>(); }
        }

        public AboutViewModel About
        {
            get { return ServiceLocator.Current.GetInstance<AboutViewModel>(); }
        }

        public AnnounceListViewModel AnnounceList
        {
            get { return ServiceLocator.Current.GetInstance<AnnounceListViewModel>(); }
        }

        public AnnounceListDetailsViewModel AnnounceListDetails
        {
            get { return ServiceLocator.Current.GetInstance<AnnounceListDetailsViewModel>(); }
        }

        public CourseListViewModel CourseList
        {
            get { return ServiceLocator.Current.GetInstance<CourseListViewModel>(); }
        }
        
        public CourseViewModel Course
        {
            get { return ServiceLocator.Current.GetInstance<CourseViewModel>(); }
        }

        public FoodmenuViewModel Foodmenu
        {
            get { return ServiceLocator.Current.GetInstance<FoodmenuViewModel>(); }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}