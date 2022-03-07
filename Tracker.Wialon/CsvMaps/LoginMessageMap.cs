using CsvHelper.Configuration;
using Tracker.Wialon.Messages;

namespace Tracker.Wialon.CsvMaps
{
	public sealed class LoginMessageMap : ClassMap<LoginMessage>
	{
		public LoginMessageMap()
		{
			Map(m => m.Protocol).Index(0);
			Map(m => m.DeviceId).Index(1);
			Map(m => m.Password).Index(2);
			Map(m => m.Crc16).Index(3);
		}
	}
}