using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

using MvcMiniProfiler;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class BolfTrackerDbConnection
    {
        public static DbConnection GetProfiledConnection()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BolfTracker"].ConnectionString);

            // Wrap the connection with a profiling connection that tracks timings
            return new MvcMiniProfiler.Data.ProfiledDbConnection(connection, MiniProfiler.Current);
        }
    }
}
