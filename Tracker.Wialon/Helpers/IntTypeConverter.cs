using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Tracker.Wialon.Helpers
{
	public class IntTypeConverter : DefaultTypeConverter
	{
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
		{
			int value;
			if (int.TryParse(text, out value))
				return value;
			return default(int?);
		}
	}
}