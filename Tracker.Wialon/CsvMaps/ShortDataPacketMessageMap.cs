using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.AspNetCore.Identity;
using Tracker.Wialon.Helpers;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon.CsvMaps
{
	public class ShortDataPacketMessageMap : ClassMap<ShortDataPacketMessage>
	{
		public ShortDataPacketMessageMap()
		{
			Map(m => m.TrackDateTimeUtc)
				.ConvertUsing(CsvExtensions.ExtractDateTimeOffsetFromMessage);
			Map(m => m.Latitude)
				.ConvertUsing(CsvExtensions.ExtractDateLatitudeFromMessage);
			Map(m => m.Longitude)
				.ConvertUsing(CsvExtensions.ExtractDateLongitudeFromMessage);
			;
			Map(m => m.Speed).Index(6).TypeConverter<IntTypeConverter>();
			Map(m => m.Heading).Index(7).TypeConverter<IntTypeConverter>();
			Map(m => m.Altitude).Index(8).TypeConverter<IntTypeConverter>();
			Map(m => m.Satellites).Index(9).TypeConverter<IntTypeConverter>();
		}
	}
}