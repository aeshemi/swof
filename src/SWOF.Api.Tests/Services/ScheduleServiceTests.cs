using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using SWOF.Api.Models;
using SWOF.Api.Models.Enums;
using SWOF.Api.Repositories;
using SWOF.Api.Services;
using SWOF.Api.Utils;
using Xunit;

namespace SWOF.Api.Tests.Services
{
	public class ScheduleServiceTestsFixture
	{
		internal readonly Mock<IEngineerRepository> MockEngineerRepository;
		internal readonly Mock<IScheduleRepository> MockScheduleRepository;
		internal readonly ScheduleService ScheduleService;

		public ScheduleServiceTestsFixture()
		{
			MockEngineerRepository = new Mock<IEngineerRepository>();
			MockScheduleRepository = new Mock<IScheduleRepository>();
			ScheduleService = new ScheduleService(MockEngineerRepository.Object, MockScheduleRepository.Object);
		}
	}

	public class ScheduleServiceTests : IClassFixture<ScheduleServiceTestsFixture>, IDisposable
	{
		private readonly Mock<IEngineerRepository> mockEngineerRepository;
		private readonly Mock<IScheduleRepository> mockScheduleRepository;
		private readonly ScheduleService scheduleService;

		private readonly Engineer[] Engineers =
		{
			new Engineer { Id = 1, Firstname = "Charlot", Lastname = "Eede", IsActive = true },
			new Engineer { Id = 2, Firstname = "Natalee", Lastname = "Wigin", IsActive = true },
			new Engineer { Id = 3, Firstname = "Nap", Lastname = "Nimmo", IsActive = true },
			new Engineer { Id = 4, Firstname = "Auria", Lastname = "De Witt", IsActive = true },
			new Engineer { Id = 5, Firstname = "Burk", Lastname = "Maciaszek", IsActive = true },
			new Engineer { Id = 6, Firstname = "Hetti", Lastname = "Whatley", IsActive = true },
			new Engineer { Id = 7, Firstname = "Monti", Lastname = "Vercruysse", IsActive = true },
			new Engineer { Id = 8, Firstname = "Lethia", Lastname = "Bonefant", IsActive = true },
			new Engineer { Id = 9, Firstname = "Sharity", Lastname = "Dronsfield", IsActive = true },
			new Engineer { Id = 10, Firstname = "Nicolea", Lastname = "Appleyard", IsActive = true }
		};

		public ScheduleServiceTests(ScheduleServiceTestsFixture fixture)
		{
			mockEngineerRepository = fixture.MockEngineerRepository;
			mockScheduleRepository = fixture.MockScheduleRepository;
			scheduleService = fixture.ScheduleService;
		}

		public void Dispose()
		{
			mockEngineerRepository.Reset();
			mockScheduleRepository.Reset();
		}

		[Fact]
		public void GetCalendar()
		{
			var startDate = new DateTime(2018, 7, 29);

			scheduleService.GetCalendar(startDate);
			mockScheduleRepository.Verify(x => x.GetDetailByRange(startDate, new DateTime(2018, 9, 9)), Times.Once);
		}

		[Fact]
		public void GetList()
		{
			var startDate = new DateTime(2018, 8, 13);

			scheduleService.GetList(startDate);
			mockScheduleRepository.Verify(x => x.GetWithNameByRange(startDate, new DateTime(2018, 8, 31)), Times.Once);
		}

		[Fact]
		public void GenerateSchedule_EmptySchedule()
		{
			mockEngineerRepository.Setup(x => x.GetAllActive()).Returns(Engineers);
			mockScheduleRepository.Setup(x => x.GetLastScheduledWeek()).Returns(new List<Schedule>());

			var result = scheduleService.GenerateSchedule().ToList();

			result.Should()
				.NotBeNull()
				.And.NotBeEmpty()
				.And.HaveCount(40); // 2 shifts for 4 weeks

			result.First().Date.Should().Be(DateHelpers.NextWeekday().Date);

			mockScheduleRepository.Verify(x => x.GetLastScheduledWeek(), Times.Once);
			mockEngineerRepository.Verify(x => x.GetAllActive(), Times.Once);
			mockScheduleRepository.Verify(x => x.Save(It.IsAny<IEnumerable<Schedule>>()), Times.Once);
		}

		[Fact]
		public void GenerateSchedule_ExistingSchedule()
		{
			var lastScheduledDate = DateTime.UtcNow.AddDays(-15).Date;

			mockEngineerRepository.Setup(x => x.GetAllActive()).Returns(Engineers);
			mockScheduleRepository.Setup(x => x.GetLastScheduledWeek()).Returns(new List<Schedule>
			{
				new Schedule { Date = lastScheduledDate.AddDays(-4), Shift = Shift.Morning, EngineerId = 5 },
				new Schedule { Date = lastScheduledDate.AddDays(-4), Shift = Shift.Afternoon, EngineerId = 7 },
				new Schedule { Date = lastScheduledDate.AddDays(-3), Shift = Shift.Morning, EngineerId = 2 },
				new Schedule { Date = lastScheduledDate.AddDays(-3), Shift = Shift.Afternoon, EngineerId = 4 },
				new Schedule { Date = lastScheduledDate.AddDays(-2), Shift = Shift.Morning, EngineerId = 9 },
				new Schedule { Date = lastScheduledDate.AddDays(-2), Shift = Shift.Afternoon, EngineerId = 3 },
				new Schedule { Date = lastScheduledDate.AddDays(-1), Shift = Shift.Morning, EngineerId = 6 },
				new Schedule { Date = lastScheduledDate.AddDays(-1), Shift = Shift.Afternoon, EngineerId = 1 },
				new Schedule { Date = lastScheduledDate, Shift = Shift.Morning, EngineerId = 8 },
				new Schedule { Date = lastScheduledDate, Shift = Shift.Afternoon, EngineerId = 10 }
			});

			var result = scheduleService.GenerateSchedule().ToList();

			result.Should()
				.NotBeNull()
				.And.NotBeEmpty()
				.And.HaveCount(40); // 2 shifts for 4 weeks

			result.First().Date.Should().Be(DateHelpers.NextWeekday(lastScheduledDate));

			mockScheduleRepository.Verify(x => x.GetLastScheduledWeek(), Times.Once);
			mockEngineerRepository.Verify(x => x.GetAllActive(), Times.Once);
			mockScheduleRepository.Verify(x => x.Save(It.IsAny<IEnumerable<Schedule>>()), Times.Once);
		}
	}
}
