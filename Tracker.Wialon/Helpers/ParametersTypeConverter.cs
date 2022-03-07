using System;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon.Helpers
{
	public class ParametersTypeConverter : DefaultTypeConverter
	{
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
		{
			Dictionary<string, object> parameters = new Dictionary<string, object>();
			
			if (!string.IsNullOrWhiteSpace(text))
			{
				var paramsSplitted = text.Split(",");

				
				
				foreach (var s in paramsSplitted)
				{
					try
					{


						var data = s.Split(":");

						var name = data[0];
						var type = (AdditionalParamType) int.Parse(data[1]);
						switch (type)
						{
							case AdditionalParamType.Integer:
								parameters.Add(name, double.Parse(data[2], NumberStyles.Integer, CultureInfo.InvariantCulture));
								break;
							case AdditionalParamType.Double:
								parameters.Add(name, double.Parse(data[2], NumberStyles.Float, CultureInfo.InvariantCulture));
								break;
							case AdditionalParamType.String:
								parameters.Add(name, data[2]);
								break;
						}
					}
					catch (Exception ex)
					{
						
					}
				}
			}

			return parameters;
		}
	}
}