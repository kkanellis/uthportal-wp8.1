namespace UTHPortal.Common
{
    public class RestAPI
    {
        public static string baseUrl = "http://sfi.ddns.net/test/uthportal/api/v1/info/";

        #region Inf 
        private static string InfDept_ = baseUrl + "inf/";
        private static string InfDeptAnnounce_ = InfDept_ + "announce/";
        public  static string InfDeptAnnounce = InfDeptAnnounce_;

        private static string InfDeptCourses_ = InfDept_ + "courses/";
        public  static string InfDeptCourses = InfDeptCourses_ + "?exclude=announcements";

        public static string InfDeptCourse = InfDeptCourses_ + "{0}";
        public static string InfDeptAnnouncementsGeneral = InfDeptAnnounce_ + "general";
        public static string InfDeptAnnouncementsUndergraduates = InfDeptAnnounce_ + "undergraduates";
        public static string InfDeptAnnouncementsAcademic = InfDeptAnnounce_ + "academic";
        public static string InfDeptAnnouncementsScholarships = InfDeptAnnounce_ + "scholarships";

        #endregion

        #region Uth
        private static string Uth = baseUrl + "uth/";
        public static string UthAnnouncements = Uth + "announce/";
        public static string UniversityNews = UthAnnouncements + "news";
        public static string UniversityEvents = UthAnnouncements + "events";
        public static string UniversityFoodmenu = Uth + "foodmenu";

        #endregion
        public static string Feedback = baseUrl + "/feedback";

    }
}
