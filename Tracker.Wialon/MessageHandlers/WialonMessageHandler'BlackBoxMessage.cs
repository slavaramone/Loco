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
	public class BlackBoxMessageHandler : WialonMessageHandler<BlackBoxMessage>
	{
		private readonly CsvConfiguration _configuration;

		public BlackBoxMessageHandler(ILogger<WialonMessageHandler<BlackBoxMessage>> logger, CsvConfiguration csvConfiguration)
			: base(logger)
		{
			_configuration = csvConfiguration;
		}

		public override BlackBoxMessage GetMessage(string ascii)
		{
			var bbMessage = new BlackBoxMessage()
			{
				Payload = ascii,
				Messages = new List<ExtendedDataMessage>()
			};

			var extendedDataMessagesStr = ascii.Split("|").ToList();

			var messagesCount = extendedDataMessagesStr.Count;

			extendedDataMessagesStr = extendedDataMessagesStr.Take(messagesCount - 1).ToList();

			foreach (var extendedDataMessage in extendedDataMessagesStr)
			{
				using (var reader = new StringReader(extendedDataMessage))
				using (var csv = new CsvReader(reader, _configuration))
				{
					var record = csv.GetRecords<ExtendedDataMessage>().SingleOrDefault();

					if (record == null)
						throw new CsvMessageParseException($"Сообщение не может быть разобрано как CSV, data:{extendedDataMessage}");
					bbMessage.Messages.Add(record);
				}

				this.Logger.LogTrace($"Blackbox message extended: {extendedDataMessage}");
			}

			return bbMessage;
		}
	}
}