using GalaSoft.MvvmLight.Ioc;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UTHPortal.Models;
using Windows.Storage;

namespace UTHPortal.Common
{
    public class StorageService : IStorageService
    {
        private readonly uint bufferSize = 4096;

        private ILoggerService loggerService;
        private ApplicationDataContainer settingsContainer;
        private Windows.Storage.Streams.Buffer buffer;

        private char[] folderSeparators = { '/' };
        private object _lock;

        public StorageService()
        {
            _lock = new object();
            buffer = new Windows.Storage.Streams.Buffer(bufferSize);
            settingsContainer = ApplicationData.Current.LocalSettings;

            loggerService = SimpleIoc.Default.GetInstance<ILoggerService>();

            /* Uncomment to clear the settings */
            /*foreach (string key in settingsContainer.Values.Keys)
            {
                settingsContainer.Values.Remove(key);
            }*/

            /* Uncomment to erase all storage files */
            /*Task task = new Task(async () =>
            {
                try {
                    var storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("RestAPI");
                    await storageFolder.DeleteAsync();
                }
                catch { }
            });
            task.Start();
            task.Wait();*/
            
        }

        private async Task SaveData(string path, string filename, string data)
        {
            StorageFolder folder = await NavigateTo(path);
            StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(file, data);
        }

        public async Task SaveAPIData(string url, string data)
        {
            /* RestAPI data are saved in files inside 'RestAPI' folder
             * with the following format:
             * eg:      http://sfi.ddns.net/uthportal/uth/rss/news
             * file:    RestAPI/uth-rss-news
             */

            if (url != null && data != null)
            {
                string path = "RestAPI";
                string filename = SanitizeUrl(url.Substring(RestAPI.baseUrl.Length));

                await SaveData(path, filename, data).ConfigureAwait(false);
            }
        }

        private async Task<string> GetData(string path, string filename)
        {
            try
            {
                StorageFolder folder = await NavigateTo(path);
                StorageFile file = await folder.GetFileAsync(filename);
                
                return (await FileIO.ReadTextAsync(file));
            }
            catch (Exception Exception){
                loggerService.Log("StorageService", "GetData", Exception.Message);
            }
            return null;
        }

        public async Task<string> GetAPIData(string url)
        {
            /* RestAPI data are saved in files inside 'RestAPI' folder
             * with the following format:
             * eg:      http://sfi.ddns.net/uthportal/uth/rss/news
             * file:    RestAPI/uth-rss-news
             */

            if (url != null)
            {
                string path = "RestAPI";
                string filename = SanitizeUrl(url.Substring(RestAPI.baseUrl.Length));

                return await GetData(path, filename).ConfigureAwait(false);
            }
            return null;
        }

        private async Task<StorageFolder> NavigateTo(string path)
        {
            StorageFolder currentFolder = ApplicationData.Current.LocalFolder;
            String[] folderPath = path.Split(folderSeparators,
                                                StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < folderPath.Length; i++)
            {
                var nextFolder = await currentFolder.CreateFolderAsync(folderPath[i],
                                                        CreationCollisionOption.OpenIfExists);
                currentFolder = nextFolder;
            }
            return currentFolder;
        }

        private string SanitizeUrl(string url)
        {
            string bannedChars = "?=,/";
            char replaceChar = '-';

            StringWriter filename = new StringWriter();
            foreach (char c in url) {
                if (bannedChars.IndexOf(c) >= 0) {
                    filename.Write(replaceChar);
                }
                else {
                    filename.Write(c);
                }
            }
            return filename.ToString();
        }

        #region Settings

        /*public void SaveSettings(AppSettingsModel model)
        {
            var properties = model.GetType().GetRuntimeProperties();
            foreach (PropertyInfo pInfo in properties)
            {
                if (pInfo.PropertyType != typeof(PropertyChangedEventHandler))
                {
                    AddOrUpdateValue(pInfo.Name, pInfo.GetValue(model));
                }
            }
        }*/

        public bool SaveSettingsEntry(string name, object value)
        {
            return AddOrUpdateValue(name, value);
        }

        /*public AppSettingsModel GetSettings()
        {
            AppSettingsModel model = new AppSettingsModel();

            var properties = model.GetType().GetRuntimeProperties();
            foreach (PropertyInfo pInfo in properties)
            {
                if (pInfo.PropertyType != typeof(PropertyChangedEventHandler) &&
                    pInfo.CanWrite)
                {
                    Type propertyType = pInfo.PropertyType;

                    var getMethod = this.GetType().GetTypeInfo().GetDeclaredMethod("GetValueOrDefault").MakeGenericMethod(propertyType);
                    pInfo.SetValue(model, getMethod.Invoke(this, new object[] {pInfo.Name}));
                }
            }

            return model;
        }*/

        public Object GetSettingsEntry(string name)
        {
            Type propertyType = typeof(AppSettingsModel).GetTypeInfo().GetDeclaredProperty(name).PropertyType;
            var method = this.GetType().GetTypeInfo().GetDeclaredMethod("GetValueOrDefault").MakeGenericMethod(propertyType);
            return method.Invoke(this, new object[] { name });
        }

        private bool AddOrUpdateValue(string key, object value)
        {
            bool valueChanged = false;
            if (value != null)
            {
                value = (string)SerializeToString(value);

                if (settingsContainer.Values.ContainsKey(key))
                {
                    if (settingsContainer.Values[key] != value)
                    {
                        settingsContainer.Values[key] = value;
                        valueChanged = true;
                    }
                }
                else
                {
                    settingsContainer.Values.Add(key, value);
                    valueChanged = true;
                }
            }
            return valueChanged;
        }

        private T GetValueOrDefault<T>(string key)
        {
            T value;
            if (settingsContainer.Values.ContainsKey(key))
            {
                value = DeserializeFromString<T>((string)settingsContainer.Values[key]);
            }
            else
            {
                value = default(T);
            }
            return value;
        }

        /** XML Helpers **/
        /* Forced to used XML Serialization cause of List<T> not being 
         * serializable by the default serializer
         */
        private string SerializeToString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        private T DeserializeFromString<T>(string xml)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                return (T)deserializer.Deserialize(reader);
            }
        }

        #endregion

    }
}
