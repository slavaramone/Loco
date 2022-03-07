using System;

namespace Tracker.Wialon.Exceptions
{
	public class CsvMessageParseException : Exception
	{
		public CsvMessageParseException(string message) : base(message)
		{
		}

		public CsvMessageParseException(string message, Exception ex)
			: base(message, ex)
		{
		}
	}
}