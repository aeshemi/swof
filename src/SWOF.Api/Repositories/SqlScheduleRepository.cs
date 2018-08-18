using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using SWOF.Api.Models;

namespace SWOF.Api.Repositories
{
	public class SqlScheduleRepository : IScheduleRepository
	{
		private readonly Func<IDbConnection> getConnection;

		public SqlScheduleRepository(Func<IDbConnection> getConnection)
		{
			this.getConnection = getConnection;
		}

		public IEnumerable<ScheduleDetail> GetDetailByRange(DateTime startDate, DateTime endDate)
		{
			using (var db = getConnection().TryOpen())
			{
				const string sql = @"
SELECT
	CAST(r.Date AS DATETIME) + CAST(s.StartTime AS DATETIME) StartDateTime,
	CAST(r.Date AS DATETIME) + CAST(s.EndTime AS DATETIME) EndDateTime,
	r.Shift,
	CONCAT(RTRIM(e.Firstname), ' ', RTRIM(e.Lastname)) Name
FROM Schedules r
INNER JOIN Engineers e on r.EngineerId = e.Id
INNER JOIN Shifts s on r.Shift = s.Id
WHERE r.Date BETWEEN @StartDate AND @EndDate
ORDER BY r.Date ASC, r.Shift ASC";

				return db.Query<ScheduleDetail>(sql, new
				{
					StartDate = startDate.ToString("yyyy-MM-dd"),
					EndDate = endDate.ToString("yyyy-MM-dd")
				});
			}
		}

		public IEnumerable<ScheduleWithName> GetWithNameByRange(DateTime startDate, DateTime endDate)
		{
			using (var db = getConnection().TryOpen())
			{
				const string sql = @"
SELECT
	r.Date,
	r.Shift,
	CONCAT(RTRIM(e.Firstname), ' ', RTRIM(e.Lastname)) Name
FROM Schedules r
INNER JOIN Engineers e on r.EngineerId = e.Id
WHERE r.Date BETWEEN @StartDate AND @EndDate
ORDER BY r.Date ASC, r.Shift ASC";

				return db.Query<ScheduleWithName>(sql, new
				{
					StartDate = startDate.ToString("yyyy-MM-dd"),
					EndDate = endDate.ToString("yyyy-MM-dd")
				});
			}
		}

		public IEnumerable<Schedule> GetLastScheduledWeek()
		{
			using (var db = getConnection().TryOpen())
			{
				const string sql = @"
SELECT * FROM Schedules
WHERE Date > DATEADD(DAY, -7, (SELECT MAX(Date) FROM Schedules))
ORDER BY Date, Shift DESC";

				return db.Query<Schedule>(sql);
			}
		}

		public void Save(IEnumerable<Schedule> rotation)
		{
			using (var db = getConnection().TryOpen())
			{
				db.Execute("INSERT INTO Schedules VALUES (@Date, @Shift, @EngineerId)", rotation);
			}
		}
	}
}
