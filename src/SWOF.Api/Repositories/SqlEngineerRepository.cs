using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using SWOF.Api.Models;

namespace SWOF.Api.Repositories
{
	public class SqlEngineerRepository : IEngineerRepository
	{
		private readonly Func<IDbConnection> getConnection;

		public SqlEngineerRepository(Func<IDbConnection> getConnection)
		{
			this.getConnection = getConnection;
		}

		public IEnumerable<Engineer> GetAllActive()
		{
			using (var db = getConnection().TryOpen())
			{
				return db.GetList<Engineer>(new { IsActive = true });
			}
		}
	}
}
