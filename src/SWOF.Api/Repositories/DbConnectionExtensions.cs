using System.Data;

namespace SWOF.Api.Repositories
{
	public static class DbConnectionExtensions
	{
		public static IDbConnection TryOpen(this IDbConnection db)
		{
			if (db.State != ConnectionState.Open) db.Open();

			return db;
		}
	}
}
