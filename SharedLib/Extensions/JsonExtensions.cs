using System.IO;
using System.Text;
using System.Text.Json;

namespace SharedLib.Extensions
{
	public static class JsonExtensions
	{
		public static string ToJsonString(this JsonDocument jdoc)
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
				{
					jdoc.WriteTo(writer);
					writer.Flush();
					return Encoding.UTF8.GetString(stream.ToArray());
				}				
			}
		}
	}
}
