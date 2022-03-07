using AutoMapper;
using Contracts;
using Management.Ui.Hubs;
using Management.Ui.Models;
using Management.Ui.Options;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLib.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Management.Ui.Consumers
{
	public class NotificationMessageConsumer : IConsumer<NotificationMessage>, IConsumer<SpeedExceededNotificationMessage>
	{
		private readonly IHubContext<ArmHub> _armHubContext;
		private readonly IHubContext<DispatcherHub> _dispatcherHubContext;
		private readonly IMapper _mapper;
		private readonly ILogger<NotificationMessageConsumer> _logger;
		private readonly NotificationOptions _notificationOptions;

		public NotificationMessageConsumer(IHubContext<ArmHub> armHubContext, IHubContext<DispatcherHub> dispatcherHubContext,
			IMapper mapper, ILogger<NotificationMessageConsumer> logger,
			IOptions<NotificationOptions> notificationOptions)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_armHubContext = armHubContext ?? throw new ArgumentNullException(nameof(armHubContext));
			_dispatcherHubContext = dispatcherHubContext ?? throw new ArgumentNullException(nameof(dispatcherHubContext));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_notificationOptions = notificationOptions.Value ?? throw new ArgumentNullException(nameof(notificationOptions.Value));
		}

		private async Task PublishToClients(ConsumeContext<NotificationMessage> context, IEnumerable<NotificationModel> notifications)
		{
			await _armHubContext.Clients.Group(context.Message.LocoId.ToString()).SendAsync(HubMethod.Notifications,
				JsonSerializer.Serialize(notifications, JsonSerializerCamelCaseOptions.Get()), context.CancellationToken);

			await _dispatcherHubContext.Clients.All.SendAsync(HubMethod.Notifications,
				JsonSerializer.Serialize(notifications, JsonSerializerCamelCaseOptions.Get()), context.CancellationToken);
		}

		public async Task Consume(ConsumeContext<NotificationMessage> context)
		{
			_logger.LogInformation("Start consuming message");

			var notifications = new[]
			{
				_mapper.Map<NotificationModel>((context.Message, _notificationOptions.LifetimeSeconds))
			};

			await PublishToClients(context, notifications);

			_logger.LogInformation("End consuming message");
		}

		public async Task Consume(ConsumeContext<SpeedExceededNotificationMessage> context)
		{
			_logger.LogInformation("Start consuming message");

			var notifications = new[]
			{
				_mapper.Map<NotificationModel>((context.Message, _notificationOptions.LifetimeSeconds))
			};

			await PublishToClients(context, notifications);

			_logger.LogInformation("End consuming message");
		}
	}
}