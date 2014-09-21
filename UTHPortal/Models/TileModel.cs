using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace UTHPortal.Models
{
    public class TileModel : ObservableObject
    {
        public string Label { get; set; }
        public BitmapImage ImageSource { get; set; }
        public string Url { get; set; }

        public RelayCommand<string> Click { get; set; }
        public bool IsImplemented { get; set; }
    }
}
