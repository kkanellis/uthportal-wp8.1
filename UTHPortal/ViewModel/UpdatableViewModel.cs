using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using UTHPortal.Common;

namespace UTHPortal.ViewModel
{
    public abstract class UpdatableViewModel<T> : ViewModelBase
    {
        protected IDataService dataService;
        protected INavigationService navigationService;
        protected IStorageService storageService;
        protected IViewService viewService;


        /// <summary>
        /// Represents the data to be viewed.
        /// </summary>
        public T Data
        {
            get { return _data; }
            set { Set(() => Data, ref _data, value); }
        }
        protected T _data;

        /// <summary>
        /// True if the ViewModel is currently retrieving data from the web.
        /// </summary>
        public bool IsRefreshing
        {
            get { return !_isRefreshing; }
            set { Set(() => IsRefreshing, ref _isRefreshing, value); }
        }
        protected bool _isRefreshing;

        /// <summary>
        /// True if remote data were downloaded, false otherwise.
        /// </summary>
        public bool RemoteDataAvailable
        {
            get { return _remoteDataAvailable; }
            set { _remoteDataAvailable = value; }
        }
        protected bool _remoteDataAvailable;

        /// <summary>
        /// True if local data are available (from a previous successfull fetching), false otherwise.
        /// </summary>
        public bool LocalDataAvailable
        {
            get { return _localDataAvailable; }
            set { _localDataAvailable = value; }
        }
        protected bool _localDataAvailable;

        /// <summary>
        /// String representing the url used for fetching.
        /// </summary>
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    Data = default(T);
                });
            }
        }
        protected string _url;

        /// <summary>
        /// Initializes the viewmodel by locating all services needed
        /// </summary>
        public UpdatableViewModel()
        {
            if (!IsInDesignMode) {
                dataService = SimpleIoc.Default.GetInstance<IDataService>();
                navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
                storageService = SimpleIoc.Default.GetInstance<IStorageService>();
                viewService = SimpleIoc.Default.GetInstance<IViewService>();
            }
        }

        /// <summary>
        /// Gets the RefreshCommand.
        /// </summary>
        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand
                    ?? (_refreshCommand = new RelayCommand(ExecuteRefreshCommand));
            }
        }
        private RelayCommand _refreshCommand;

        /// <summary>
        /// Does the neccessary actions for the refresh of the data
        /// </summary>
        protected virtual async void ExecuteRefreshCommand()
        {
            IsRefreshing = true;

            await viewService.ShowStatusBar("Ενημέρωση...", null);

            var newData = await dataService.RefreshAndSave(
                Url,
                typeof(T)
            );

            if (newData != null) {
                Data = (T)newData;
                viewService.ModifyStatusBar("Τελευταία Ενημέρωση: " + DateTime.Now.ToString("HH:mm"), 0.0);

                RemoteDataAvailable = true;
            }
            else {
                viewService.ModifyStatusBar("Αποτυχία ενημέρωσης!", 0.0);
                RemoteDataAvailable = false;
            }

            IsRefreshing = false;

            await ValidateDisplayData();
        }

        private RelayCommand _pageLoaded;
        /// <summary>
        /// Command bind to behaviour. Executed when page has been loaded
        /// </summary>
        public RelayCommand PageLoaded
        {
            get
            {
                return _pageLoaded
                    ?? (_pageLoaded = new RelayCommand(ExecutePageLoaded));
            }
        }
        protected abstract void ExecutePageLoaded();

        /// <summary>
        /// Retrieves the local data of this view using the provided storage service.
        /// </summary>
        protected async Task GetSavedView()
        {
            string json = await storageService.GetAPIData(Url);

            Data = (T)dataService.ParseJson(json, typeof(T));

            if (Data == null) {
                Data = default(T);
                LocalDataAvailable = false;
            }
            else {
                LocalDataAvailable = true;
            }
        }

        protected virtual Task ValidateDisplayData()
        {
            // Dummy return //
            return Task.FromResult(false);
        }
    }
}
