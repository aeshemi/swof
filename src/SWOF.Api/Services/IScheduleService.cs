using System;
using System.Collections.Generic;
using SWOF.Api.Models;

namespace SWOF.Api.Services
{
public interface IScheduleService
{
IEnumerable<ScheduleDetail> GetCalendar(DateTime startDate);

IEnumerable<ScheduleWithName> GetList(DateTime startDate);

IEnumerable<Schedule> GenerateSchedule(int weeks = 4);
}
}
