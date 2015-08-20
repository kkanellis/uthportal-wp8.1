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
        Task<object> Refresh(RestAPIItem item, Type modelType);
        Task<object> RefreshAndSave(RestAPIItem item, Type modelType);
        void CancelAllRequests();

        object ParseJson(string json, Type modelType);
    }
}
