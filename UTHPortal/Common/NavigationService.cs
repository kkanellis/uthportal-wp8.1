using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.Phone.UI.Input;

namespace UTHPortal.Common
{
    public class NavigationService : INavigationService
    {
        private Frame _mainFrame;
        private Dictionary<Type, object> savedStates;
        private Object _stateLock;

        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
            savedStates = new Dictionary<Type, object>();
            _stateLock = new Object();

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (_mainFrame.CanGoBack)
            {
                GoBack();
                e.Handled = true;
            }
        }

        public void NavigateTo(Type pageType)
        {
            _mainFrame.Navigate(pageType);
        }

        public void NavigateTo(Type pageType, Type statePageType, object state)
        {
            lock (_stateLock)
            {
                savedStates[statePageType] = state;
            }
            _mainFrame.Navigate(pageType);
        }

        public object GetAndRemoveState(Type pageType)
        {
            Object state = savedStates[pageType];
            RemoveState(pageType);
            return state;
        }

        public bool StateExists(Type pageType)
        {
            return (savedStates.ContainsKey(pageType));
        }
        
        private void RemoveState(Type key) {
            lock (_stateLock)
            {
                savedStates.Remove(key);
            }
        }

        public void GoBack()
        {
            if (_mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
            }
        }
    }
}
