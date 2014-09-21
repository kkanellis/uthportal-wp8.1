using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTHPortal.Common;

namespace UTHPortal.ViewModel
{
    public abstract class UpdatableViewModel<T> : ViewModelBase
    {
        protected IDataService _dataService;
        protected INavigationService _navigationService;
        protected IStorageService _storageService;
        protected IViewService _viewService;

        /* Binding properties */
        protected T _data;
        public T Data
        {
            get { return _data; }
            set { Set(() => Data, ref _data, value); }
        }

        protected bool _isBusy;
        public bool IsBusy
        {
            get { return !_isBusy; }
            set { Set(() => IsBusy, ref _isBusy, value); }
        }
        /* ****************** */

        /* Simple properties */
        protected bool _refreshedSuccessfull;
        public bool RefreshedSuccessfull
        {
            get { return _refreshedSuccessfull; }
            set { _refreshedSuccessfull = value; }
        }

        protected bool _savedViewAvailable;
        public bool SavedViewAvailable
        {
            get { return _savedViewAvailable; }
            set { _savedViewAvailable = value; }
        }
        /* ****************** */

        protected string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Data = default(T);
                });
            }
        }

        /// <summary>
        /// Initializes the viewmodel by retrieving all services needed
        /// </summary>
        public UpdatableViewModel()
        {
            if (!IsInDesignMode)
            {
                _dataService = SimpleIoc.Default.GetInstance<IDataService>();
                _navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
                _storageService = SimpleIoc.Default.GetInstance<IStorageService>();
                _viewService = SimpleIoc.Default.GetInstance<IViewService>();
            }
        }

        private RelayCommand _refreshCommand;
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
        /// <summary>
        /// Does the neccessary actions for the refresh of the data
        /// </summary>
        protected virtual async void ExecuteRefreshCommand()
        {
            IsBusy = true;

            await _viewService.ShowStatusBar("Ενημέρωση...", null);

            var newData = await _dataService.RefreshAndSave(
                Url,
                typeof(T));


            if (newData != null)
            {
                Data = (T)newData;
                _viewService.ModifyStatusBar("Τελευταία Ενημέρωση: " + DateTime.Now.ToString("HH:mm"), 0.0);

                RefreshedSuccessfull = true;
            }
            else
            {
                _viewService.ModifyStatusBar("Αποτυχία ενημέρωσης!", 0.0);
                RefreshedSuccessfull = false;
            }

            IsBusy = false;

            ValidateDisplayData();
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
        /// Retrieves the saved data of the view using the provided Storage service.
        /// </summary>
        protected async Task GetSavedView()
        {
            string json = await _storageService.GetAPIData(Url);
            
            Data = (T)_dataService.ParseJson(json, typeof(T));
            
            if (Data == null) {
                Data = default(T);
                SavedViewAvailable = false;
            }
            else {
                SavedViewAvailable = true;
            }
        }

        protected virtual void ValidateDisplayData() { }
    }
}
