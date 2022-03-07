using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Notification.Db.Consumers
{
	public class NotificationListConsumer : IConsumer<NotificationListRequest>
    {
        private readonly INotificationDbRepo _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationListConsumer> _logger;

        public NotificationListConsumer(IMapper mapper, INotificationDbRepo repo, ILogger<NotificationListConsumer> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<NotificationListRequest> context)
        {
            _logger.LogInformation("Start consuming message");

            var result = await _repo.GetNotifications(context.Message);

            var response = _mapper.Map<NotificationListResponse>(result);
            await context.RespondAsync(response);

            _logger.LogInformation("End consuming message");
        }
    }
}
