using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTHPortal.Models;

namespace UTHPortal.Common
{
    public interface IStorageService
    {
        Task<string> GetAPIData(string url);
        Task SaveAPIData(string url, string data);

        object GetSettingsEntry(string name);
        bool SaveSettingsEntry(string name, object value);
    }
}
