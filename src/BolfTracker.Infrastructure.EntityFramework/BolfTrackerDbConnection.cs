using System.Data.Common;
using System.Data.SqlClient;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class BolfTrackerDbConnection
    {
        public static DbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString.Build());
        }
    }
}
