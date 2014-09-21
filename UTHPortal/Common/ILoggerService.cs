using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTHPortal.Common
{
    public interface ILoggerService
    {
        //void Log(string message);
        //void Log(string title, string message);
        void Log(string _class, string method, string message);
    }
}
