using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Diagnostics;

using UTHPortal.Models;
using System.Threading;
using GalaSoft.MvvmLight.Ioc;

namespace UTHPortal.Common
{
    public class DataService : IDataService
    {
        private readonly TimeSpan timeoutMilliSecs;

        private IStorageService storageService;
        private ILoggerService loggerService;

        public DataService()
        {
            timeoutMilliSecs = new TimeSpan(0, 0, 15);

            storageService = SimpleIoc.Default.GetInstance<IStorageService>();
            loggerService = SimpleIoc.Default.GetInstance<ILoggerService>();
        }

        public async Task<object> Refresh(RestAPIItem item, Type modelType)
        {
            if (IsNetworkAvailable())
            {
                var json = await GetData(item.RequestUrl, item.RequestParams).ConfigureAwait(false);
                return ParseJson(json, modelType);
            }
            return null;
        }

        public async Task<object> RefreshAndSave(RestAPIItem item, Type modelType)
        {
            if (IsNetworkAvailable())
            {
                var json = await GetData(item.RequestUrl, item.RequestParams).ConfigureAwait(false);

                await storageService.SaveJSON(item, json);
                return ParseJson(json, modelType);
            }
            return null;
        }

        /*
        private async Task<bool> PostData(string url, FormUrlEncodedContent content)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = timeoutMilliSecs;
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode) {
                        return true;
                    }
                }
                catch (Exception Exception) {
                    loggerService.Log("DataService", "PostData", Exception.Message);
                }
            }
            return false;
        }
        */

        private async Task<string> GetData(string url, string parameters)
        {
            string json = null;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var requestUrl = url;
                    if (!string.IsNullOrWhiteSpace(parameters)) {
                        requestUrl += parameters;
                    }

                    client.Timeout = timeoutMilliSecs;
                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();

                    json = await response.Content.ReadAsStringAsync();
                }
                catch (Exception Exception)
                {
                    loggerService.Log("DataService", "GetData", Exception.Message);
                }
            }

            return json;
        }

        public void CancelAllRequests()
        {
            throw new NotImplementedException();
        }


        public object ParseJson(string json, Type modelType)
        {
            object data = null;
            try
            {
                data = JsonConvert.DeserializeObject(json, modelType);
            }
            catch (Exception Exception){
                loggerService.Log("DataService", "ParseJson", Exception.Message);
            }
            return data;
        }

        private bool IsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        /*
        public async Task<bool> SendFeedback(string message)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("message", message));

            return await PostData(RestAPI.FeedbackUrl, new FormUrlEncodedContent(values));
        }
        */

    }
}
