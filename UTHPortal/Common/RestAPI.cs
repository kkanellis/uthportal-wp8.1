using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using System.Runtime.Serialization;
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

            Items.Add(new RestAPIItem(typeof(CourseModel), "inf/courses/{0}", null, "{0} ", new string [] { "Info.Name" }));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "inf/announce/general", null, "γενικές ανακοινώσεις"));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "inf/announce/undergraduate", null, "προπτυχιακών"));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "inf/announce/academic", null, "ακαδημαϊκά νέα"));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "inf/announce/scholarships", null, "υποτροφίες"));

            #endregion

            #region Uth
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "uth/announce/news", null, "νέα πανεπιστημίου"));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "uth/announce/events", null, "εκδηλώσεις πανεπιστημίου"));
            Items.Add(new RestAPIItem(typeof(AnnounceModel), "uth/foodmenu", null));
            
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
        public string Filename { get; set; }

        public string ModelTypeName { get; set; }

        public string[] DisplayParams { get; set; }
        public string DisplayFormat { get; set; }

        public RestAPIItem() { }

        public RestAPIItem(Type modelType, string url, string requestParams, string displayFormat, string[] displayParams)
        {
            this.ModelTypeName = modelType.AssemblyQualifiedName;

            this.Url = url;
            this.RequestUrl = RestAPI.GetFullUrl(url);
            this.RequestParams = requestParams;
            this.DisplayFormat = displayFormat;
            this.DisplayParams = displayParams;

            this.Collection = GetCollection(url);
            this.Filename = Collection.Replace('.', '-');
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
                Type.GetType(ModelTypeName),
                string.Format(Url, args),
                RequestParams,
                DisplayFormat,
                DisplayParams
                );
        }

        

        private string GetCollection(string url)
        {
            // Distinguish uth / and /id
            if (url.EndsWith("/")) {
                // Remove last '/'
                url = url.Substring(0, url.Length - 1);
            }
            else {
                url = string.Format(url, string.Empty);
            }

            return url.Replace('/', '.');
        }
    }
}
