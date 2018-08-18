using System;
using System.Collections.Generic;
using System.Linq;
using SWOF.Api.Models;
using SWOF.Api.Models.Enums;

namespace SWOF.Api.Utils
{
	public static class ScheduleHelper
	{
		public static (IEnumerable<int> morning, IEnumerable<int> afternoon) AssignShift(IReadOnlyCollection<int> engineerIds, IReadOnlyCollection<Schedule> lastScheduledWeek)
		{
			if (lastScheduledWeek.Any())
			{
				var shifts = lastScheduledWeek
					.GroupBy(s => s.Shift)
					.ToDictionary(g => g.Key, g => g.Select(s => s.EngineerId));

				return (shifts[Shift.Afternoon].Shuffle(), shifts[Shift.Morning].Shuffle());
			}

			var shuffledEngineers = engineerIds.Shuffle();

			return (shuffledEngineers.Take(5), shuffledEngineers.Skip(5));
		}

		public static (List<Schedule> rotation, DateTime lastScheduledDate) ScheduleWeekRotation(DateTime currentDate, (IEnumerable<int> morning, IEnumerable<int> afternoon) shifts)
		{
			var rotation = new List<Schedule>();

			foreach (var engineers in shifts.morning.Zip(shifts.afternoon, (x, y) => new { Morning = x, Afternoon = y }))
			{
				rotation.Add(new Schedule
				{
					Date = currentDate.Date,
					Shift = Shift.Morning,
					EngineerId = engineers.Morning
				});

				rotation.Add(new Schedule
				{
					Date = currentDate.Date,
					Shift = Shift.Afternoon,
					EngineerId = engineers.Afternoon
				});

				if (currentDate.DayOfWeek == DayOfWeek.Friday) break;

				currentDate = currentDate.AddDays(1);
			}

			return (rotation.ToList(), currentDate);
		}
	}
}
