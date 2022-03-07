using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLib;
using SharedLib.Options;
using SharedLib.Protocols.Azimuth;
using System;
using System.Text;

namespace Tracker.Tcp
{
    public class TcpListenerService : TcpListener
	{
		private readonly IAzimuthMessageParser _azimuthMessageParser;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IMapper _mapper;

		public TcpListenerService(ILogger<TcpListenerService> logger, IOptions<TcpListenerOptions> tcpListenerOptions, IAzimuthMessageParser azimuthMessageParser,
			IPublishEndpoint publishEndpoint, IMapper mapper) : base(tcpListenerOptions, logger)
        {
			_azimuthMessageParser = azimuthMessageParser;
			_publishEndpoint = publishEndpoint;
			_mapper = mapper;
		}

		public override void ProcessReceivedBytes(StateObject state, int bytesRead)
		{
			var protocolMessage = _azimuthMessageParser.Parse(state.buffer);
			if (protocolMessage.Type == AzimuthProtocolMessageType.LocationReport)
			{
				var locationReportMessage = (AzimuthLocationReportPayload)protocolMessage.Payload;

				var trackerDataMessage = _mapper.Map<TrackerDataMessage>((locationReportMessage, protocolMessage.TrackerId));

				_publishEndpoint.Publish(trackerDataMessage);
			}
		}
	}
}
