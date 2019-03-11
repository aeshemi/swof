using System;
using System.Collections.Generic;
using System.Linq;
using SWOF.Api.Models;
using SWOF.Api.Repositories;
using SWOF.Api.Utils;

namespace SWOF.Api.Services
{
public class ScheduleService : IScheduleService
{
private readonly IEngineerRepository engineerRepository;

private readonly IScheduleRepository scheduleRepository;

public ScheduleService(IEngineerRepository engineerRepository, IScheduleRepository scheduleRepository)
{
this.engineerRepository = engineerRepository;

this.scheduleRepository = scheduleRepository;
}

/****************************************************************************************

Retrieves schedule details for a duration of a calendar month displayed based on a given start date.


INPUT:

IN_dtStartDate          Start date of schedules to be retrieved


RESULT:

Collection of schedule details to be displayed in a UI calendar based on the given start date.

****************************************************************************************/
//QQQ
                public IEnumerable<ScheduleDetail>
GetCalendar
               (DateTime            IN_dtStartDate)
{
return          scheduleRepository.GetDetailByRange(IN_dtStartDate, IN_dtStartDate.AddDays(42)); // 35/42 days displayed in calendar month
}


/****************************************************************************************

Retrieves schedule with name for the remaining days of the calendar month from a given start date.


INPUT:

IN_dtStartDate          Start date of schedules to be retrieved


RESULT:

Collection of schedule with name for the remaining days of the calendar month from a given start date.

****************************************************************************************/
//QQQ
                public IEnumerable<ScheduleWithName>
GetList
               (DateTime            IN_dtStartDate)
{
return          scheduleRepository.GetWithNameByRange(IN_dtStartDate,
                    new DateTime(IN_dtStartDate.Year, IN_dtStartDate.Month, DateTime.DaysInMonth(IN_dtStartDate.Year, IN_dtStartDate.Month)));
}


/****************************************************************************************

Generates schedules based on the last scheduled week in the database.


INPUT:

IN_iWeeks              Number of weeks to generate which defaults to 4


RESULT:

Generated schedules.

****************************************************************************************/
//QQQ
                public IEnumerable<Schedule>
GenerateSchedule
               (int                 IN_iWeeks = 4)
{
var             lastScheduledWeek   = scheduleRepository.GetLastScheduledWeek().ToList();

var             lastScheduledDate   = lastScheduledWeek.LastOrDefault()?.Date;

var             engineerIds         = engineerRepository.GetAllActive().Select(x => x.Id).ToList();

var             engineerRotation    = new List<Schedule>();

/*

Following loop generates schedules for the number of weeks provided.

*/

for (var i = 0; i < IN_iWeeks; i++)
    {
    var startingDate = DateHelpers.NextWeekday(lastScheduledDate);

    var shifts = ScheduleHelper.AssignShift(engineerIds, lastScheduledWeek);

    (lastScheduledWeek, lastScheduledDate) = ScheduleHelper.ScheduleWeekRotation(startingDate, shifts);

    engineerRotation.AddRange(lastScheduledWeek);
    }

scheduleRepository.Save(engineerRotation);

return          engineerRotation;
}
}
}
