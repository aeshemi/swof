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

		public IEnumerable<ScheduleDetail> GetCalendar(DateTime startDate)
		{
			return scheduleRepository.GetDetailByRange(startDate, startDate.AddDays(42)); // 35/42 days displayed in calendar month
		}

		public IEnumerable<ScheduleWithName> GetList(DateTime startDate)
		{
			return scheduleRepository.GetWithNameByRange(startDate,
				new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)));
		}

		public IEnumerable<Schedule> GenerateSchedule(int weeks = 4)
		{
			var lastScheduledWeek = scheduleRepository.GetLastScheduledWeek().ToList();

			var lastScheduledDate = lastScheduledWeek.LastOrDefault()?.Date;

			var engineerIds = engineerRepository.GetAllActive().Select(x => x.Id).ToList();

			var engineerRotation = new List<Schedule>();

			for (var i = 0; i < weeks; i++)
			{
				var startingDate = DateHelpers.NextWeekday(lastScheduledDate);

				var shifts = ScheduleHelper.AssignShift(engineerIds, lastScheduledWeek);

				(lastScheduledWeek, lastScheduledDate) = ScheduleHelper.ScheduleWeekRotation(startingDate, shifts);

				engineerRotation.AddRange(lastScheduledWeek);
			}

			scheduleRepository.Save(engineerRotation);

			return engineerRotation;
		}
	}
}
