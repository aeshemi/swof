using System;
using FluentAssertions;
using SWOF.Api.Utils;
using Xunit;

namespace SWOF.Api.Tests.Utils
{
	public class DateHelpersTests
	{
		private static readonly DateTime Monday = new DateTime(2018, 8, 13);

		public static readonly object[][] WeekData =
		{
			new object[]{ new DateTime(2018, 8, 13) }, // Monday
			new object[]{ new DateTime(2018, 8, 7) }, // Tuesday
			new object[]{ new DateTime(2018, 8, 8) }, // Wednesday
			new object[]{ new DateTime(2018, 8, 9) }, // Thursday
			new object[]{ new DateTime(2018, 8, 10) }, // Friday
			new object[]{ new DateTime(2018, 8, 11) }, // Saturday
			new object[]{ new DateTime(2018, 8, 12) }, // Sunday
		};


		public static readonly object[][] ParseDateData =
		{
			new object[]{ "2018-08-21", new DateTime(2018, 8, 21) },
			new object[]{ "2020-02-30", null },
			new object[]{ "05/29/2018", null },
			new object[]{ "Friday, 29 May 2015", null },
			new object[]{ "test", null }
		};

		[Theory, MemberData(nameof(WeekData))]
		public void NextWeekdayTheory(DateTime input)
		{
			DateHelpers.NextWeekday(input).Should().Be(Monday);
		}

		[Theory, MemberData(nameof(ParseDateData))]
		public void ParseDateInputTheory(string input, DateTime? expected)
		{
			DateHelpers.ParseDateInput(input).Should().Be(expected);
		}
	}
}
