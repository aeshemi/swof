using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using SWOF.Api.Models;
using Xunit;

namespace SWOF.Api.Tests._Integration
{
	public class ScheduleTestsFixture
	{
		internal readonly ApiServer ApiServer;
		internal readonly IEnumerable<Schedule> GeneratedSchedules;
		internal readonly HttpResponseMessage GenerateScheduleResponse;

		public ScheduleTestsFixture(ApiServer apiServer)
		{
			ApiServer = apiServer;

			GenerateScheduleResponse = apiServer.Client.PostJson("/api/schedule/generate");

			GenerateScheduleResponse.ShouldBeHttpCreatedResult();

			GeneratedSchedules = GenerateScheduleResponse.Model<IEnumerable<Schedule>>().ToList();
		}
	}

	[Collection("ApiTest")]
	public class ScheduleTests : IClassFixture<ScheduleTestsFixture>
	{
		private readonly ApiServer apiServer;
		private readonly IEnumerable<Schedule> generatedSchedules;
		private readonly HttpResponseMessage generateScheduleResponse;

		public ScheduleTests(ScheduleTestsFixture fixture)
		{
			apiServer = fixture.ApiServer;
			generatedSchedules = fixture.GeneratedSchedules;
			generateScheduleResponse = fixture.GenerateScheduleResponse;
		}

		[Fact]
		public void Schedule_Generated()
		{
			generateScheduleResponse.Headers.Location.Should().Be($"api/schedule/{generatedSchedules.First().Date:yyyy-MM-dd}/calendar");

			generatedSchedules.Should().NotBeEmpty().And.HaveCount(40);

			var scheduleEngineers = generatedSchedules.Select(x => x.EngineerId).ToList();

			scheduleEngineers.Should().NotContain(new[] { 5, 10 });

			var previous = scheduleEngineers.GetRange(0, 10);

			previous.Should().OnlyHaveUniqueItems();

			for (var i = 1; i < 4; i++)
			{
				var next = scheduleEngineers.GetRange(10 * i, 10);

				next.Should()
					.OnlyHaveUniqueItems()
					.And.BeEquivalentTo(previous);

				next.First().Should().NotBe(previous.Last());

				previous = next;
			}
		}

		[Theory]
		[InlineData("2020-02-30")]
		[InlineData("Friday, 29 May 2015")]
		[InlineData("test")]
		public void Schedule_Calendar_InvalidInput(string input)
		{
			apiServer.Client.Get($"/api/schedule/{input}/calendar").ShouldHaveValidationErrorMessage("Start date input is invalid");
		}

		[Fact]
		public void Schedule_Calendar()
		{
			var startDate = generatedSchedules.First().Date.AddDays(7);

			var response = apiServer.Client.Get($"/api/schedule/{startDate:yyyy-MM-dd}/calendar");

			response.EnsureSuccessStatusCode();

			var calendar = response.Model<IEnumerable<ScheduleDetail>>().ToList();

			calendar.Should().NotBeEmpty().And.HaveCount(30);
			calendar.Select(x => x.StartDateTime).All(x => x >= startDate && x < startDate.AddDays(19)).Should().BeTrue(); //should only retrieve remaining 3 weeks out of 4 generated
		}

		[Theory]
		[InlineData("2020-02-30")]
		[InlineData("Friday, 29 May 2015")]
		[InlineData("test")]
		public void Schedule_List_InvalidInput(string input)
		{
			apiServer.Client.Get($"/api/schedule/{input}/list").ShouldHaveValidationErrorMessage("Start date input is invalid");
		}

		[Fact]
		public void Schedule_List()
		{
			var startDate = generatedSchedules.First().Date.AddDays(7);

			var response = apiServer.Client.Get($"/api/schedule/{startDate:yyyy-MM-dd}/list");

			response.EnsureSuccessStatusCode();

			var calendar = response.Model<IEnumerable<ScheduleWithName>>().ToList();

			calendar.Should().NotBeEmpty();
			calendar.Select(x => x.Date).All(x => x.Month == startDate.Month).Should().BeTrue();
		}
	}
}
