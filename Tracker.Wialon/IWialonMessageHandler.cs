using System;
using CsvHelper;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon
{
	public interface IWialonMessageHandler<out T>
		where T : BaseMessage
	{
		public T GetMessage(string ascii);

		public void HandleMessage(string ascii, Action<T> action);
	}
}