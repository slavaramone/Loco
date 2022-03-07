using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tracker.Wialon.CsvMaps;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon.MessageHandlers
{
	public class CsvWialonMessageHandler<T> : IWialonMessageHandler<T> where T : BaseMessage
	{
		private CsvConfiguration _configuration;
		private ILogger<CsvWialonMessageHandler<T>> _logger;

		public CsvWialonMessageHandler(ILogger<CsvWialonMessageHandler<T>> logger, CsvConfiguration csvConfiguration)
		{
			_configuration = csvConfiguration ?? throw new ArgumentNullException(nameof(csvConfiguration));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public virtual T GetMessage(string ascii)
		{
			using (var reader = new StringReader(ascii))
			using (var csv = new CsvReader(reader, _configuration))
			{
				T record = null;

				if (string.IsNullOrWhiteSpace(ascii)) return null;

				record = csv.GetRecords<T>().SingleOrDefault();

				record.Payload = ascii;

				return record;
			}
		}

		public void HandleMessage(string ascii, Action<T> action)
		{
			var msg = GetMessage(ascii);

			_logger.LogTrace($"HandledMessage: {JsonConvert.SerializeObject(msg, Formatting.Indented)}");

			action(msg);
		}
	}
}