using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Fluent;
using Tracker.Wialon.CsvMaps;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon.MessageHandlers
{
	public abstract class WialonMessageHandler<T> : IWialonMessageHandler<T> where T : BaseMessage
	{
		protected ILogger<WialonMessageHandler<T>> Logger { get; }

		protected WialonMessageHandler(ILogger<WialonMessageHandler<T>> logger)
		{
			this.Logger = logger;
		}

		public abstract T GetMessage(string ascii);

		public void HandleMessage(string ascii, Action<T> action)
		{
			var msg = GetMessage(ascii);

			Logger.LogTrace($"HandledMessage: {JsonConvert.SerializeObject(msg, Formatting.Indented)}");

			action(msg);
		}
	}
}