using Contracts;
using Contracts.Enums;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedLib;
using SharedLib.Options;
using System.Text;

namespace Notification.Tcp
{
    public class TcpListenerService : TcpListener
	{
		private const string RealTimeEventQuery = "realtime-event";

		private readonly IPublishEndpoint _publishEndpoint;

		public TcpListenerService(ILogger<TcpListenerService> logger, IOptions<TcpListenerOptions> tcpListenerOptions, IPublishEndpoint publishEndpoint) 
			: base(tcpListenerOptions, logger)
        {
			_publishEndpoint = publishEndpoint;
		}

		public override void ProcessReceivedBytes(StateObject state, int bytesRead)
		{
			state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
			string content = state.sb.ToString();

			var model = JsonConvert.DeserializeObject<VideoNotificationModel>(content);
			if (model.Query.Equals(RealTimeEventQuery))
            {
				foreach (var type in model.Types)
				{
					_publishEndpoint.Publish<NotificationMessage>(new
					{
						Severity = Severity.Warning,
						Type = type
					}).Wait();
				}
			}
		}
	}
}
