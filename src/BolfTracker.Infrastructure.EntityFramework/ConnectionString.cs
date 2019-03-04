using System.Configuration;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class ConnectionString
    {
        public static string Build()
        {
#if DEBUG
            return ConfigurationManager.ConnectionStrings["BolfTracker"].ConnectionString;
#else
            string hostname = ConfigurationManager.AppSettings["RDS_HOSTNAME"];
            string database = ConfigurationManager.AppSettings["RDS_DB_NAME"];
            string username = ConfigurationManager.AppSettings["RDS_USERNAME"];
            string password = ConfigurationManager.AppSettings["RDS_PASSWORD"];

            return $"Data Source={hostname};Initial Catalog={database};User ID={username};Password={password};";
#endif
        }
    }
}
