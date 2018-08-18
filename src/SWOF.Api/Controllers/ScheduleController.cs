using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SWOF.Api.Filters;
using SWOF.Api.Models;
using SWOF.Api.Services;

namespace SWOF.Api.Controllers
{
	[Produces("application/json")]
	[Route("api/schedule")]
	public class ScheduleController : Controller
	{
		private readonly IScheduleService scheduleService;

		public ScheduleController(IScheduleService scheduleService)
		{
			this.scheduleService = scheduleService;
		}

		/// <summary>
		/// Retrieves schedule rotation for calendar (42 items from the start date)
		/// </summary>
		/// <param name="startDate">Start of the schedule rotations to retrieve in "yyyy-MM-dd" format</param>
		/// <response code="200">Returns the schedule rotation list for 42 items from the start date provided</response>
		/// <response code="400">If startDate input is invalid</response>
		// GET api/schedule/2018-08-16/calendar
		[HttpGet("{startDate}/calendar")]
		[ValidateStartDateInput]
		[ProducesResponseType(typeof(IEnumerable<ScheduleDetail>), 200)]
		[ProducesResponseType(400)]
		public IActionResult GetCalendar(DateTime startDate)
		{
			return new OkObjectResult(scheduleService.GetCalendar(startDate));
		}

		/// <summary>
		/// Retrieves schedule rotation for monthly list (start date up to end of the month)
		/// </summary>
		/// <param name="startDate">Start of the schedule rotations to retrieve in "yyyy-MM-dd" format</param>
		/// <response code="200">Returns the schedule rotation list for the month from the start date provided</response>
		/// <response code="400">If startDate input is invalid</response>
		// GET api/schedule/2018-08-16/list
		[HttpGet("{startDate}/list")]
		[ValidateStartDateInput]
		[ProducesResponseType(typeof(IEnumerable<ScheduleWithName>), 200)]
		[ProducesResponseType(400)]
		public IActionResult GetList(DateTime startDate)
		{
			return new OkObjectResult(scheduleService.GetList(startDate));
		}

		/// <summary>
		/// Generates schedule rotation for 4 weeks after the last scheduled rotation in store
		/// </summary>
		/// <response code="201">Returns list of newly created schedule rotations</response>
		// POST api/schedule/generate
		[HttpPost("generate")]
		[ProducesResponseType(typeof(IEnumerable<Schedule>), 201)]
		public IActionResult Post()
		{
			var rotation = scheduleService.GenerateSchedule();

			return new CreatedResult($"api/schedule/{rotation.First().Date:yyyy-MM-dd}/calendar", rotation);
		}
	}
}
