using CsvHelper.Configuration;
using Tracker.Wialon.Helpers;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon.CsvMaps
{
	public sealed class ExtendedDataMessageMap : ClassMap<ExtendedDataMessage>
	{
		public ExtendedDataMessageMap()
		{
			Map(m => m.TrackDateTimeUtc)
				.Index(0, 1)
				.ConvertUsing(CsvExtensions.ExtractDateTimeOffsetFromMessage);
			Map(m => m.Latitude).Index(2)
				.ConvertUsing(CsvExtensions.ExtractDateLatitudeFromMessage);
			Map(m => m.Longitude).Index(4)
				.ConvertUsing(CsvExtensions.ExtractDateLongitudeFromMessage);
			;
			Map(m => m.Speed).Index(6).TypeConverter<IntTypeConverter>();
			Map(m => m.Heading).Index(7).TypeConverter<IntTypeConverter>();
			Map(m => m.Altitude).Index(8).TypeConverter<IntTypeConverter>();
			Map(m => m.Satellites).Index(9).TypeConverter<IntTypeConverter>();
			Map(m => m.HDOP).Index(10).Optional();
			Map(m => m.Inputs).Index(11).TypeConverter<IntTypeConverter>().Optional();
			Map(m => m.Outputs).Index(12).TypeConverter<IntTypeConverter>().Optional();
			Map(m => m.Adc).Index(13).Optional();
			Map(m => m.Ibutton).Index(14).Optional();
			Map(m => m.Parameters).Index(15).TypeConverter<ParametersTypeConverter>().Optional();
		}
	}
}