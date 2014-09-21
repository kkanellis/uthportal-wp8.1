using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UTHPortal.Models;

namespace UTHPortal.Common
{
    public interface IDataService
    {
        Task<Object> Refresh(String url, Type modelType);
        Task<Object> RefreshAndSave(String url, Type modelType);
        Object ParseJson(String json, Type modelType);

        Task<bool> SendFeedback(string message);
    }
}
