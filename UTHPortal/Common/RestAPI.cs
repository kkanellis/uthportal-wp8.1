namespace UTHPortal.Common
{
    public class RestAPI
    {
        public static string baseUrl = "http://sfi.ddns.net/test/uthportal/api/v1/info/";

        #region Inf 
        private static string InfDeptUrl_ = baseUrl + "inf/";
        private static string InfDeptAnnounceUrl_ = InfDeptUrl_ + "announce/";
        public static string InfDeptAnnounceUrl = InfDeptAnnounceUrl_;


        private static string InfDeptCoursesUrl_ = InfDeptUrl_ + "courses/";
        public  static string InfDeptCoursesUrl = InfDeptCoursesUrl_ + "?exclude=announcements";

        public static string InfDeptCourseStr = "({0}) {1} [{2}]"; // codeSite, name, [source] 
        public static string InfDeptCourseUrl = InfDeptCoursesUrl_ + "{0}";

        public static string InfDeptAnnouncementsGeneralStr = "γενικές ανακοινώσεις";
        public static string InfDeptAnnouncementsGeneralUrl = InfDeptAnnounceUrl_ + "general";

        public static string InfDeptAnnouncementsUndergraduatesStr = "προπτυχιακών";
        public static string InfDeptAnnouncementsUndergraduatesUrl = InfDeptAnnounceUrl_ + "undergraduates";

        public static string InfDeptAnnouncementsAcademicStr = "ακαδημαϊκά νέα";
        public static string InfDeptAnnouncementsAcademicUrl = InfDeptAnnounceUrl_ + "academic";

        public static string InfDeptAnnouncementsScholarshipsStr = "υποτροφίες";
        public static string InfDeptAnnouncementsScholarshipsUrl = InfDeptAnnounceUrl_ + "scholarships";

        #endregion

        #region Uth
        private static string UthUrl = baseUrl + "uth/";
        public static string UthAnnouncementsUrl = UthUrl + "announce/";

        public static string UniversityNewsStr = "νέα πανεπιστημίου";
        public static string UniversityNewsUrl = UthAnnouncementsUrl + "news";

        public static string UniversityEventsStr = "εκδηλώσεις πανεπιστημίου";
        public static string UniversityEventsUrl = UthAnnouncementsUrl + "events";

        public static string UniversityFoodmenuUrl = UthUrl + "foodmenu";

        #endregion

        #region Misc
        public static string FeedbackUrl = baseUrl + "/feedback";

        #endregion

    }
}
