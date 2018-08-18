using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Extensions;
using SWOF.Api.Models;
using SWOF.Api.Models.Enums;
using SWOF.Api.Utils;
using Xunit;

namespace SWOF.Api.Tests.Utils
{
	public class ScheduleHelperTests
	{
		private static readonly int[] EngineerIds = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

		private static readonly Schedule[] Rotation =
		{
			new Schedule { Date = DateTime.Parse("2018-08-13"), Shift = Shift.Morning, EngineerId = 5 },
			new Schedule { Date = DateTime.Parse("2018-08-13"), Shift = Shift.Afternoon, EngineerId = 7 },
			new Schedule { Date = DateTime.Parse("2018-08-14"), Shift = Shift.Morning, EngineerId = 2 },
			new Schedule { Date = DateTime.Parse("2018-08-14"), Shift = Shift.Afternoon, EngineerId = 4 },
			new Schedule { Date = DateTime.Parse("2018-08-15"), Shift = Shift.Morning, EngineerId = 9 },
			new Schedule { Date = DateTime.Parse("2018-08-15"), Shift = Shift.Afternoon, EngineerId = 3 },
			new Schedule { Date = DateTime.Parse("2018-08-16"), Shift = Shift.Morning, EngineerId = 6 },
			new Schedule { Date = DateTime.Parse("2018-08-16"), Shift = Shift.Afternoon, EngineerId = 1 },
			new Schedule { Date = DateTime.Parse("2018-08-17"), Shift = Shift.Morning, EngineerId = 8 },
			new Schedule { Date = DateTime.Parse("2018-08-17"), Shift = Shift.Afternoon, EngineerId = 10 }
		};

		public class EmptySchedule
		{
			private (IEnumerable<int> morning, IEnumerable<int> afternoon) shifts;

			public EmptySchedule()
			{
				shifts = ScheduleHelper.AssignShift(EngineerIds, new List<Schedule>());
			}

			[Fact]
			public void ShouldAllocateEngineerShifts()
			{
				shifts.morning.Should()
					.NotBeEmpty()
					.And.HaveCount(5)
					.And.OnlyHaveUniqueItems()
					.And.BeSubsetOf(EngineerIds)
					.And.NotIntersectWith(shifts.afternoon);

				shifts.afternoon.Should()
					.NotBeEmpty()
					.And.HaveCount(5)
					.And.OnlyHaveUniqueItems()
					.And.BeSubsetOf(EngineerIds)
					.And.NotIntersectWith(shifts.morning);
			}
		}

		public class ExistingSchedule
		{
			private (IEnumerable<int> morning, IEnumerable<int> afternoon) shifts;

			public ExistingSchedule()
			{
				shifts = ScheduleHelper.AssignShift(EngineerIds, Rotation);
			}

			[Fact]
			public void ShouldAllocateEngineerShifts()
			{
				shifts.morning.Should()
					.NotBeEmpty()
					.And.HaveCount(5)
					.And.OnlyHaveUniqueItems()
					.And.BeSubsetOf(EngineerIds)
					.And.Contain(new[] { 7, 4, 3, 1, 10 })
					.And.NotIntersectWith(shifts.afternoon);

				shifts.afternoon.Should()
					.NotBeEmpty()
					.And.HaveCount(5)
					.And.OnlyHaveUniqueItems()
					.And.BeSubsetOf(EngineerIds)
					.And.Contain(new[] { 5, 2, 9, 6, 8 })
					.And.NotIntersectWith(shifts.morning);
			}
		}

		[Fact]
		public void ScheduleWeekRotation()
		{
			var shifts = (new[] { 5, 2, 9, 6, 8 }, new[] { 7, 4, 3, 1, 10 });

			var (rotation, lastScheduledDate) = ScheduleHelper.ScheduleWeekRotation(DateTime.Parse("2018-08-13"), shifts);

			rotation.Should()
				.NotBeEmpty()
				.And.HaveCount(10)
				.And.Equal(Rotation, (x, y) => x.Date.Date == y.Date.Date && x.Shift == y.Shift && x.EngineerId == y.EngineerId);

			lastScheduledDate.Date.Should().Be(17.August(2018));
		}
	}
}
