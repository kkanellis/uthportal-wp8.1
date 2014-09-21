using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;

namespace UTHPortal.Common
{
    public class ViewService : IViewService
    {
        public async Task ShowStatusBar(string text, double? progressValue)
        {
            var progressIndicator = StatusBar.GetForCurrentView().ProgressIndicator;
            progressIndicator.Text = text;
            progressIndicator.ProgressValue = progressValue;

            await progressIndicator.ShowAsync();
        }

        public void ModifyStatusBar(string text, double progressValue)
        {
            var progressIndicator = StatusBar.GetForCurrentView().ProgressIndicator;
            progressIndicator.Text = text;
            progressIndicator.ProgressValue = progressValue;
        }

        public async Task HideStatusBar()
        {
            var progressIndicator = StatusBar.GetForCurrentView().ProgressIndicator;
            await progressIndicator.HideAsync();
        }

        public async Task ShowMessageDialog(string text, string title)
        {
            var messageDialog = new MessageDialog(text, title);
            await messageDialog.ShowAsync();
        }

    }
}
