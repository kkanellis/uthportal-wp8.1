using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTHPortal.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace UTHPortal.Models
{
    public class MainListItem : ObservableObject
    {
        public string Label { get; set; }
        public BitmapImage ImageSource { get; set; }
        public RestAPIItem Info { get; set; }

        public RelayCommand<RestAPIItem> Click { get; set; }
        public bool IsImplemented { get; set; }
    }
}
