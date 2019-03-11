using System;
using System.Collections.Generic;
using SWOF.Api.Models;

namespace SWOF.Api.Repositories
{
public interface IScheduleRepository
{
IEnumerable<ScheduleDetail> GetDetailByRange(DateTime startDate, DateTime endDate);

IEnumerable<ScheduleWithName> GetWithNameByRange(DateTime startDate, DateTime endDate);

IEnumerable<Schedule> GetLastScheduledWeek();

void Save(IEnumerable<Schedule> rotation);
}
}
