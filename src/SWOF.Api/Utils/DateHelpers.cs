using System;
using System.Globalization;

namespace SWOF.Api.Utils
{
	public static class DateHelpers
	{
		private const string dateFormat = "yyyy-MM-dd";

		public static DateTime NextWeekday(DateTime input)
		{
			var diff = ((int)DayOfWeek.Monday - (int)input.DayOfWeek + 7) % 7;

			return input.AddDays(diff);
		}

		public static DateTime NextWeekday(DateTime? input = null)
		{
			return NextWeekday(input ?? DateTime.UtcNow);
		}

		public static DateTime? ParseDateInput(string dateString)
		{
			if (DateTime.TryParseExact(dateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
			{
				return dateTime;
			}

			return null;
		}
	}
}
