using AutoMapper;
using Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Contracts.Commands;

namespace Tracker.Db.Consumers
{
	public class ReceiveMapItemsConsumer : IConsumer<ReceiveMapItems>
	{
		private readonly IMapper _mapper;
		private readonly ILogger<ReceiveMapItemsConsumer> _logger;
		private readonly ITrackerDbRepo _repo;

		public ReceiveMapItemsConsumer(IMapper mapper, ITrackerDbRepo repo, ILogger<ReceiveMapItemsConsumer> logger)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task Consume(ConsumeContext<ReceiveMapItems> context)
		{
			_logger.LogInformation("Start consuming message");

			var dynamicTrackIds = await _repo.GetDynamicMapItemTrackIds();
			await context.Publish<MapItemsReceived>(new
			{
				MapItemTrackIds = dynamicTrackIds
			});

			_logger.LogInformation("End consuming message");
		}
	}
}
