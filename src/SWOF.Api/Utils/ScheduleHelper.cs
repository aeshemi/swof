using System;
using System.Collections.Generic;
using System.Linq;
using SWOF.Api.Models;
using SWOF.Api.Models.Enums;

namespace SWOF.Api.Utils
{
public static class ScheduleHelper
{

/****************************************************************************************

Helper function that assigns shifts to be either in the morning or afternoon and shuffled for randomness of days.


INPUT:

IN_ciEngineerIds        Read only collection of engineer ids to assign shift to
IN_csLastScheduledWeek  Read only collection of Schedule for engineers last scheduled week


RESULT:

Tuple containing engineer ids assigned to morning and afternoon shifts.

****************************************************************************************/
//QQQ
                public static (IEnumerable<int> morning, IEnumerable<int> afternoon)
AssignShift
               (IReadOnlyCollection<int>                IN_ciEngineerIds,        // long type and would not fit in column 37
                IReadOnlyCollection<Schedule>           IN_csLastScheduledWeek)  // long type and would not fit in column 37
{
if (IN_csLastScheduledWeek.Any())
{
var shifts = IN_csLastScheduledWeek
    .GroupBy(s => s.Shift)
    .ToDictionary(g => g.Key, g => g.Select(s => s.EngineerId));

return          (shifts[Shift.Afternoon].Shuffle(), shifts[Shift.Morning].Shuffle());
}

var             shuffledEngineers                       = IN_ciEngineerIds.Shuffle();

return          (shuffledEngineers.Take(5), shuffledEngineers.Skip(5));
}


/****************************************************************************************

Helper function to assign schedule with date and shift the engineers.


INPUT:

IN_dtCurrentDate        Current date to start iteration of scheduling shifts
IN_iShifts              Tuple of engineer ids scheduled for morning and afternoon shifts


RESULT:

Tuple with the scheduled rotation of engineers and the last scheduled date for the rotation.

****************************************************************************************/
//QQQ
                public static (List<Schedule> rotation, DateTime lastScheduledDate)
ScheduleWeekRotation
               (DateTime                                                        IN_dtCurrentDate,    // long type and would not fit in column 37
                (IEnumerable<int> morning, IEnumerable<int> afternoon)          IN_iShifts)         // long type and would not fit in column 37
{
var             rotation            = new List<Schedule>();

/*

Following loop iterates through the tuple of engineer shifts and assigns the corresponding schedule for each shift pair of engineer.

*/

foreach (var engineers in IN_iShifts.morning.Zip(IN_iShifts.afternoon, (x, y) => new { Morning = x, Afternoon = y }))
    {
    rotation.Add(new Schedule
    {
        Date = IN_dtCurrentDate.Date,
        Shift = Shift.Morning,
        EngineerId = engineers.Morning
    });

    rotation.Add(new Schedule
    {
        Date = IN_dtCurrentDate.Date,
        Shift = Shift.Afternoon,
        EngineerId = engineers.Afternoon
    });

    if (IN_dtCurrentDate.DayOfWeek == DayOfWeek.Friday) break;

    IN_dtCurrentDate = IN_dtCurrentDate.AddDays(1);
    }

return          (rotation.ToList(), IN_dtCurrentDate);
}
}
}
