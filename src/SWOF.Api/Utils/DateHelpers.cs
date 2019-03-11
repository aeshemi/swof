using System;
using System.Globalization;

namespace SWOF.Api.Utils
{
public static class DateHelpers
{
private const    string     DEFAULT_DATE_FORMAT_YYYY_MM_DD          =           "yyyy-MM-dd";

/****************************************************************************************

Helper function to return the next weekday given a DateTime input.


INPUT:

IN_dtInput              DateTime starting point to determine the next weekday


RESULT:

DateTime representing the next weekday given the DateTime input.

****************************************************************************************/
//QQQ
                public static DateTime
NextWeekday
               (DateTime            IN_dtInput)
{
var             diff                = ((int)DayOfWeek.Monday - (int)IN_dtInput.DayOfWeek + 7) % 7;

return          IN_dtInput.AddDays(diff);
}


/****************************************************************************************

Helper function to return the next weekday given a nullable DateTime input.


INPUT:

IN_dtInput              Nullable DateTime starting point to determine the next weekday


RESULT:

DateTime representing the next weekday given the DateTime input if having a value or from the current date if input is null.

****************************************************************************************/
//QQQ
                public static DateTime
NextWeekday
               (DateTime?           IN_dtInput = null)
{
return          NextWeekday(IN_dtInput ?? DateTime.UtcNow);
}


/****************************************************************************************

Helper function to return a nullable DateTime based on a date string input.


INPUT:

IN_sDateString          Date string input to be converted to DateTime


RESULT:

DateTime representing the parsed date string or null if invalid format.

****************************************************************************************/
//QQQ
                public static DateTime?
ParseDateInput
               (string              IN_sDateString)
{
if (DateTime.TryParseExact(IN_sDateString, DEFAULT_DATE_FORMAT_YYYY_MM_DD, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
{
return          dateTime;
}

return          null;
}
}
}
