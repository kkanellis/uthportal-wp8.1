using System;
using System.Collections.Generic;
using System.Reflection;
using UTHPortal.Models;

namespace UTHPortal.Common
{
    public static class RestAPI
    {
        private static List<RestAPIItem> Items;
        private static string baseUrl = "http://sfi.ddns.net/test/uthportal/api/v1/info/";

        public static void Initialize()
        {
            Items = new List<RestAPIItem>();

            #region Inf
            Items.Add(new RestAPIItem(typeof(CourseAnnounceModel), "inf/announce/", null));
            Items.Add(new RestAPIItem(typeof(CourseAllModel), "inf/courses/", "?exclude=announcements"));

            Items.Add(new RestAPIItem(typeof(CourseModel), "inf/courses/{0}", null, "({0}) {1} [{2}]", new string [] { "Info.codeSite", "Name", "Source" }));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "inf/announce/general", null, "γενικές ανακοινώσεις"));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "inf/announce/undergraduate", null, "προπτυχιακών"));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "inf/announce/academic", null, "ακαδημαϊκά νέα"));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "inf/announce/scholarships", null, "υποτροφίες"));

            #endregion

            #region Uth
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "uth/announce/news", null, "νέα πανεπιστημίου"));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "uth/announce/events", null, "εκδηλώσεις πανεπιστημίου"));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "uth/announce/foodmenu", null));
            
            #endregion
        }

        public static RestAPIItem GetItem(string collection)
        {
            return Items.Find(item => item.Collection == collection);
        }

        /// <summary>
        /// Removes the baseUrl and any (possible) parameters from the url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFullUrl(string url)
        {
            return baseUrl + url;
        }
    }

    public class RestAPIItem
    {
        public string Url { get; set; }
        public string RequestUrl { get; set; }
        public string RequestParams { get; set; }
        public string Collection { get; set; }

        public Type ModelType;
        public string[] DisplayParams { get; set; }
        public string DisplayFormat { get; set; }

        public RestAPIItem(Type modelType, string url, string requestParams, string displayFormat, string[] displayParams)
        {
            this.ModelType = modelType;
            this.Url = url;
            this.RequestUrl = RestAPI.GetFullUrl(url);
            this.RequestParams = requestParams;
            this.DisplayFormat = DisplayFormat;
            this.DisplayParams = displayParams;

            this.Collection = String.Format(Url, string.Empty).Replace('/', '.');
        }

        public RestAPIItem(Type modelType, string url, string requestParams, string displayFormat) :
            this(modelType, url, requestParams, displayFormat, null)
        { }

        public RestAPIItem(Type modelType, string url, string requestParams) :
            this(modelType, url, requestParams, null, null)
        { }

        /// <summary>
        /// Specializes the RestAPIItem from a generic url, to a specific one
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public RestAPIItem Specialize(params string [] args)
        {
            return new RestAPIItem(
                ModelType,
                string.Format(RequestUrl, args),
                RequestParams,
                DisplayFormat,
                DisplayParams
                );
        }

        /// <summary>
        /// Returns the string representation of this REST Item using the provided model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ToString(object model)
        {
            if(model.GetType() == ModelType) {

                if (DisplayParams.Length > 0) {
                    string [] displayValues = new string [DisplayParams.Length];
                    for (int i = 0; i < displayValues.Length; i++) {
                        object value = GetNestedPropertyValue(model, DisplayParams [i]);

                        if (value == null) return string.Empty;

                        displayValues [i] = value.ToString();
                    }
                    return string.Format(DisplayFormat, displayValues);
                }
                else if (DisplayFormat != null) {
                    return DisplayFormat;
                }
                else return string.Empty;
            }
            else {
                throw new InvalidCastException();
            }
        }

        /// <summary>
        /// Returns the value of the nested property of the provided object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dottedPropertyName"></param>
        /// <returns></returns>
        private object GetNestedPropertyValue(object obj, string dottedPropertyName)
        {
            foreach(string propertyName in dottedPropertyName.Split('.')) {
                if (obj == null) {
                    return null;
                }

                Type objType = obj.GetType();
                PropertyInfo info = objType.GetRuntimeProperty(propertyName);
                if (info == null) {
                    return null;
                }

                obj = info.GetValue(obj);
            }
            return obj;
        }

    }
}
