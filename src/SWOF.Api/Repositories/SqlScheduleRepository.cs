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

/****************************************************************************************

Retrieves schedule details given a specific start and end date.


INPUT:

IN_dtStartDate          Start date of schedules to retrieve
IN_dtEndDate            End date of schedules to retrieve


RESULT:

Collection of schedule details based on a given date range.

****************************************************************************************/
//QQQ
                public IEnumerable<ScheduleDetail>
GetDetailByRange
               (DateTime            IN_dtStartDate,
                DateTime            IN_dtEndDate)
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
ORDER BY r.Date ASC, r.Shift ASC";                                                                  //SQLSQL

return          db.Query<ScheduleDetail>(sql, new
{
StartDate = IN_dtStartDate.ToString("yyyy-MM-dd"),

EndDate = IN_dtEndDate.ToString("yyyy-MM-dd")
});
}
}


/****************************************************************************************

Retrieves schedule with name given a specific start and end date.


INPUT:

IN_dtStartDate          Start date of schedules to retrieve
IN_dtEndDate            End date of schedules to retrieve


RESULT:

Collection of schedule with name based on a given date range.

****************************************************************************************/
//QQQ
                public IEnumerable<ScheduleWithName>
GetWithNameByRange
               (DateTime            IN_dtStartDate,
                DateTime            IN_dtEndDate)
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
ORDER BY r.Date ASC, r.Shift ASC";                                                                  //SQLSQL

return          db.Query<ScheduleWithName>(sql, new
{
StartDate = IN_dtStartDate.ToString("yyyy-MM-dd"),

EndDate = IN_dtEndDate.ToString("yyyy-MM-dd")
});
}
}


/****************************************************************************************

Retrieves the last scheduled week from the database.


RESULT:

Collection of schedules for the last scheduled week.

****************************************************************************************/
//QQQ
                public IEnumerable<Schedule>
GetLastScheduledWeek
               ()
{
using (var db = getConnection().TryOpen())
{
const string sql = @"
SELECT * FROM Schedules
WHERE Date > DATEADD(DAY, -7, (SELECT MAX(Date) FROM Schedules))
ORDER BY Date, Shift DESC";                                                                         //SQLSQL

return          db.Query<Schedule>(sql);
}
}

/****************************************************************************************

Saves scheduled rotation to the database.


INPUT:

IN_csRotation           Collection of schedules to be saved in the database

****************************************************************************************/
//QQQ
                public void
Save
               (IEnumerable<Schedule>       rotation)   // long type and would not fit in column 37
{
using (var db = getConnection().TryOpen())
{
db.Execute("INSERT INTO Schedules VALUES (@Date, @Shift, @EngineerId)"                              //SQLSQL
                                                                , rotation);
}
}
}
}
