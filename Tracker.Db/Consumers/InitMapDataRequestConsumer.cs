using AutoMapper;
using Contracts;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tracker.Db.Consumers
{
    public class InitMapDataRequestConsumer : IConsumer<InitMapDataRequest>
    {
		private readonly ILogger<InitMapDataRequestConsumer> _logger;
		private readonly ITrackerDbRepo _repo;
		private readonly IMapper _mapper;

		public InitMapDataRequestConsumer(IMapper mapper, ITrackerDbRepo repo, ILogger<InitMapDataRequestConsumer> logger)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Consume(ConsumeContext<InitMapDataRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var mapItemWithGeoData = await _repo.GetMapItemsWithLatestGeoData();
			var latestFuelLevels = await _repo.GetLatestFuelSensorRawData();

			var msg = new InitMapDataResponse
			{
				MapItems = _mapper.Map<List<MapItemContract>>(mapItemWithGeoData),
				FuelLevels = _mapper.Map<List<FuelLevelContract>>(latestFuelLevels)
			};
			await context.RespondAsync(msg);

			_logger.LogInformation("End consuming message");
		}
    }
}
