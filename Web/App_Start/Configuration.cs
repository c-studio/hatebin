using System.Configuration;

namespace Interactive.HateBin
{
    public static class Configuration
    {
        public static string DefaultConnectionString => ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static string EmailAddress => ConfigurationManager.AppSettings["EmailAddress"];
        public static string EmailPassword => ConfigurationManager.AppSettings["EmailPassword"];
        public static string EmailSubject => ConfigurationManager.AppSettings["EmailSubject"];
        public static string EmailServer => ConfigurationManager.AppSettings["EmailServer"];
    }
}