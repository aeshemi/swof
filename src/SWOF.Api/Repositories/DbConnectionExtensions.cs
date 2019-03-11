using System.Data;

namespace SWOF.Api.Repositories
{
public static class DbConnectionExtensions
{

/****************************************************************************************

Extension to check if database connection is already open and opens it if it is not.


INPUT:

IN_db                   Database connection to check

****************************************************************************************/
//QQQ
                public static IDbConnection
TryOpen
               (this IDbConnection  db)
{
if (db.State != ConnectionState.Open) db.Open();

return          db;
}
}
}
