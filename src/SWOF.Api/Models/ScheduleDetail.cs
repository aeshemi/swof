using System;
using SWOF.Api.Models.Enums;

namespace SWOF.Api.Models
{
	public class ScheduleDetail
	{
		public DateTime StartDateTime { get; set; }

		public DateTime EndDateTime { get; set; }

		public string Name { get; set; }

		public Shift Shift { get; set; }
	}
}
