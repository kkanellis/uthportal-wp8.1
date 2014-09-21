using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTHPortal.Common
{
    public class RestAPI
    {
        public static string baseUrl = "http://sfi.ddns.net/uthportal/";

        public static string UniversityFoodmenu = baseUrl + "uth/foodmenu";
        public static string UniversityRss = baseUrl + "uth/rss";
        public static string InfDeptAnnounce = baseUrl + "inf/announce";
        public static string InfDeptCourses = baseUrl + "inf/courses";
        public static string InfDeptCourse = InfDeptCourses + "/{0}";

        public static string UniversityRssNews = UniversityRss + "/news";
        public static string UniversityRssEvents = UniversityRss + "/events";
        public static string UniversityRssAnnouncements = UniversityRss + "/genannounce";

        public static string InfDeptGenAnnouncements = InfDeptAnnounce + "/genannounce";

        public static string Feedback = baseUrl + "/feedback";

    }
}
