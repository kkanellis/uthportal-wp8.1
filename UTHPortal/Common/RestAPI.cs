using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTHPortal.Common
{
    public class RestAPI
    {
        public static string baseUrl = "http://sfi.ddns.net/test/uthportal/api/v1/info/";

        private static string InfDept = baseUrl + "inf/";
        public static string InfDeptAnnounce = InfDept + "announce/";
        public static string InfDeptCourses = InfDept + "courses/";
        public static string InfDeptCourse = InfDeptCourses + "{0}";
        public static string InfDeptAnnouncementsGeneral = InfDeptAnnounce + "general";
        public static string InfDeptAnnouncementsUndergraduates = InfDeptAnnounce + "undergraduates";
        public static string InfDeptAnnouncementsAcademic = InfDeptAnnounce + "academic";
        public static string InfDeptAnnouncementsScholarships = InfDeptAnnounce + "scholarships";

        private static string Uth = baseUrl + "uth/";
        public static string UthAnnouncements = Uth + "announce/";
        public static string UniversityNews = UthAnnouncements + "news";
        public static string UniversityEvents = UthAnnouncements + "events";
        public static string UniversityFoodmenu = Uth + "foodmenu";

        public static string Feedback = baseUrl + "/feedback";

    }
}
