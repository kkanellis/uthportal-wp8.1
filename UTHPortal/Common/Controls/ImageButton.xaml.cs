using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UTHPortal.Models;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UTHPortal
{
    public sealed partial class ImageButton : UserControl
    {
        /*private ImageButtonModel _data;
        public ImageButtonModel Data
        {
            get { return _data; }
            set { Set(() => Data, ref _data, value); }
        }*/

        public ImageButton()
        {
            //data = new ImageButtonModel();

            this.InitializeComponent();
        }

    }
}
