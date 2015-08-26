using GalaSoft.MvvmLight.Ioc;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UTHPortal.Models;
using Windows.Storage;
using Windows.Storage.Streams;

namespace UTHPortal.Common
{
    public class StorageService : IStorageService
    {
        private const string restFolder = "RestAPI"; 

        private ILoggerService loggerService;
        private ApplicationDataContainer settingsContainer;


        public StorageService()
        {
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

        #region RestApi
        public async Task SaveJSON(RestAPIItem item, string data)
        {
            /* RestAPI data are saved in files inside 'RestAPI' folder
             * with the following format:
             * eg:      http://sfi.ddns.net/uthportal/uth/announce/news
             * file:    RestAPI/uth-rss-news
             */

            if (!string.IsNullOrEmpty(item.Url) && !string.IsNullOrEmpty(data)) {
                await SaveData(restFolder, item.Filename, data).ConfigureAwait(false);
            }
        }

        private async Task SaveData(string path, string filename, string data)
        {
            try {
                StorageFolder folder = await NavigateToFolder(path);
                StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

                await FileIO.WriteTextAsync(file, data, UnicodeEncoding.Utf8);
            }
            catch (Exception Exception) {
                loggerService.Log("StorageService", "GetData", Exception.Message);
            }
        }

        public async Task<string> LoadJSON(RestAPIItem item)
        {
            /* RestAPI data are saved in files inside 'RestAPI' folder
             * with the following format:
             * eg:      http://sfi.ddns.net/uthportal/uth/rss/news
             * file:    RestAPI/uth-rss-news
             */

            if (!string.IsNullOrEmpty(item.Url)) {
                return await LoadData(restFolder, item.Filename).ConfigureAwait(false);
            }
            return null;
        }

        private async Task<string> LoadData(string path, string filename)
        {
            try
            {
                StorageFolder folder = await NavigateToFolder(path);
                StorageFile file = await folder.GetFileAsync(filename);
                
                return (await FileIO.ReadTextAsync(file));
            }
            catch (Exception Exception){
                loggerService.Log("StorageService", "GetData", Exception.Message);
            }
            return null;
        }

        private async Task<StorageFolder> NavigateToFolder(string path)
        {
            StorageFolder currentFolder = ApplicationData.Current.LocalFolder;
            string [] folderPath = path.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < folderPath.Length; i++)
            {
                var nextFolder = await currentFolder.CreateFolderAsync(
                                                        folderPath[i],
                                                        CreationCollisionOption.OpenIfExists
                                                        );
                currentFolder = nextFolder;
            }
            return currentFolder;
        }

        #endregion

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

        public bool SetSettingsEntry(string key, object value)
        {
            bool valueChanged = false;
            if (value != null) {
                value = SerializeToString(value);

                if (settingsContainer.Values.ContainsKey(key)) {
                    if (settingsContainer.Values [key] != value) {
                        settingsContainer.Values [key] = value;
                        valueChanged = true;
                    }
                }
                else {
                    settingsContainer.Values.Add(key, value);
                    valueChanged = true;
                }
            }
            return valueChanged;
        }

        public object GetSettingsEntry(string name)
        {
            Type propertyType = typeof(AppSettingsModel).GetTypeInfo().GetDeclaredProperty(name).PropertyType;
            var method = this.GetType().GetTypeInfo().GetDeclaredMethod("GetValueOrDefault").MakeGenericMethod(propertyType);
            return method.Invoke(this, new object[] { name });
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

        /// <summary>
        /// Serializes object to string using BinaryFormatter
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string SerializeToString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            string serializedString = string.Empty;

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                serializedString = writer.ToString();
            }
            return serializedString;
        }

        /// <summary>
        /// Deserializes object from string using BinaryFormatter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        private T DeserializeFromString<T>(string xml)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            T deserializedObject = default(T);

            using (StringReader reader = new StringReader(xml))
            {
                deserializedObject = (T)deserializer.Deserialize(reader);
            }
            return deserializedObject;
        }

        #endregion

    }
}
