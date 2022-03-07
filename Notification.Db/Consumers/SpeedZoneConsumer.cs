using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.Db.Consumers
{
	public class SpeedZoneConsumer : IConsumer<SpeedZoneRequest>
	{
		private readonly INotificationDbRepo _repo;
		private readonly IMapper _mapper;
		private readonly ILogger<SpeedZoneConsumer> _logger;

		public SpeedZoneConsumer(IMapper mapper, INotificationDbRepo repo, ILogger<SpeedZoneConsumer> logger)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Consume(ConsumeContext<SpeedZoneRequest> context)
		{
			_logger.LogInformation("Start consuming message");

			var result = await _repo.GetSpeedZones();

			var response = new SpeedZoneResponse
			{
				Items = _mapper.Map<List<SpeedZoneResponseItem>>(result)
			};
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
	}
}
