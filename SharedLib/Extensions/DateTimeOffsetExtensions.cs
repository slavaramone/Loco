using System;
using System.Collections.Generic;

namespace SharedLib.Extensions
{
	public static class DateTimeOffsetExtensions
    {
		public static  DateTimeOffset RoundUp(this DateTimeOffset dateTimeOff, TimeSpan d)
		{
			var dt = new DateTime((dateTimeOff.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dateTimeOff.DateTime.Kind);
			var roundedDto = new DateTimeOffset(dt, dateTimeOff.Offset);
			return roundedDto;
		}

		public static IEnumerable<DateTimeOffset> EachDateIntervalStepTo(this DateTimeOffset from, DateTimeOffset to, TimeSpan interval)
		{
			for (var day = from.Date; day.Date <= to.Date; day = day.Add(interval))
				yield return day;
		}
	}
}
