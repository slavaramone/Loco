using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Tracker.Wialon.CsvMaps;
using Tracker.Wialon.Exceptions;
using Tracker.Wialon.Helpers;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon.MessageHandlers
{
	public class ExtendedDataMessageHandler : WialonMessageHandler<ExtendedDataMessage>
	{
		private readonly CsvConfiguration _configuration;

		public ExtendedDataMessageHandler(ILogger<WialonMessageHandler<ExtendedDataMessage>> logger, CsvConfiguration csvConfiguration)
			: base(logger)
		{
			_configuration = csvConfiguration;
		}

		public override ExtendedDataMessage GetMessage(string ascii)
		{
			using (var reader = new StringReader(ascii))
			using (var csv = new CsvReader(reader, _configuration))
			{
				var record = csv.GetRecords<ExtendedDataMessage>().SingleOrDefault();

				if (record == null)
					throw new CsvMessageParseException($"Сообщение не может быть разобрано как CSV, data:{ascii}");
				return record;
			}
		}
	}
}