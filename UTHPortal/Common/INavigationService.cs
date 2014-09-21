using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UTHPortal.Common
{
    public interface INavigationService
    {
        void NavigateTo(Type pageType);
        void NavigateTo(Type pageType, Type statePageType, object state);
        object GetAndRemoveState(Type pageType);
        bool StateExists(Type pageType);
        void GoBack();
    }
}
