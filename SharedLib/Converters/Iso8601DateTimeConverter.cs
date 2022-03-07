using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedLib.Converters
{
    public class Iso8601DateTimeConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return DateTime.ParseExact(reader.GetString(), "O", CultureInfo.InvariantCulture);
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString("O"));
		}
	}
}
