using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTHPortal.Common
{
    public interface IViewService
    {
        Task ShowStatusBar(string text, double? progressValue);
        void ModifyStatusBar(string text, double progressValue);
        Task HideStatusBar();

        Task ShowMessageDialog(string text, string title);
    }
}
