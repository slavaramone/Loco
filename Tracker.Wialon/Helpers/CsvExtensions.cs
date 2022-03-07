using System;
using System.Globalization;
using CsvHelper;

namespace Tracker.Wialon.Helpers
{
	public static class CsvExtensions
	{
		public static Func<IReaderRow, DateTimeOffset> ExtractDateTimeOffsetFromMessage = row =>
		{
			DateTime? dt = null;
			TimeSpan? ts = null;

			var dateValue = row.GetField(0);
			if (dateValue == "NA") dt = null;
			else dt = DateTime.ParseExact(row.GetField(0), "ddMMyy", CultureInfo.InvariantCulture);

			var timeValue = row.GetField(1);
			if (timeValue == "NA") ts = null;
			else ts = TimeSpan.ParseExact(row.GetField(1), "hhmmss", CultureInfo.InvariantCulture);

			if (!dt.HasValue || !ts.HasValue)
				return DateTimeOffset.UtcNow;

			return new DateTimeOffset(dt.Value + ts.Value, new TimeSpan(0, 0, 0));
		};

		public static Func<IReaderRow, double?> ExtractDateLatitudeFromMessage = row =>
		{
			var value = row.GetField(2);
			if (value == "NA")
				return null;

			var latValue = row.GetField<string>(2);
			var NS = row.GetField<string>(3);
			var degrees = int.Parse(latValue.Substring(0, 2));
			var minutes = double.Parse(latValue.Substring(2, latValue.Length - 2), NumberStyles.AllowDecimalPoint,
				CultureInfo.InvariantCulture);

			return degrees + minutes / 60 * (NS == "N" ? 1 : -1);
		};

		public static Func<IReaderRow, double?> ExtractDateLongitudeFromMessage = row =>
		{
			var value = row.GetField(4);
			if (value == "NA")
				return null;

			var lngValue = row.GetField<string>(4);
			var EW = row.GetField<string>(5);
			var degrees = int.Parse(lngValue.Substring(0, 3));
			var minutes = double.Parse(lngValue.Substring(3, lngValue.Length - 3), NumberStyles.AllowDecimalPoint,
				CultureInfo.InvariantCulture);

			return degrees + minutes / 60 * (EW == "E" ? 1 : -1);
		};
	}
}