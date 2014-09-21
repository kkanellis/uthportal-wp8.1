using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UTHPortal.Common
{
    public class LoggerService : ILoggerService
    {
        private string delimeter = "|";

        public void Log(string message)
        {
            SaveLog(String.Empty, String.Empty, String.Empty, message);
        }

        public void Log(string title, string message)
        {
            SaveLog(title, String.Empty, String.Empty, message);
        }

        public void Log(string _class, string method, string message)
        {
            SaveLog(String.Empty, _class, method, message);
        }

        /*private async Task SaveLogSync(string title, string _class, string method, string message)
        {
            await SaveLog(title, _class, method, message);
        }*/

        private async Task SaveLog(string title, string _class, string method, string message)
        {
            try {
                StorageFolder logFolder = await ApplicationData.Current.LocalFolder
                    .CreateFolderAsync("Log", CreationCollisionOption.OpenIfExists);
                StorageFile file = await logFolder.CreateFileAsync("log.log", CreationCollisionOption.OpenIfExists);

                string data = title + delimeter +
                                _class + delimeter +
                                method + delimeter +
                                message + Environment.NewLine;

                await FileIO.AppendTextAsync(file, data);
            }
            catch (Exception Exception) {
                Debug.WriteLine("LOGGER-ERROR: " + Exception.Message);
            }
        }
    }
}
