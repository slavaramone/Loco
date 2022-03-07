namespace Tracker.Wialon.Messages
{
	public class LoginMessage : BaseMessage, ICrc16
	{
		public string Protocol { get; set; }
		public string DeviceId { get; set; }
		public string Password { get; set; }
		public string Crc16 { get; set; }
	}
}