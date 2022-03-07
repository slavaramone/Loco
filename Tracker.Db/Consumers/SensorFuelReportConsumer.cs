using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tracker.Db.Consumers
{
    public class SensorFuelReportConsumer : IConsumer<SensorFuelReportRequest>
	{
		private readonly IMapper _mapper;
		private readonly ILogger<SensorFuelReportConsumer> _logger;
		private readonly ITrackerDbRepo _repo;

		public SensorFuelReportConsumer(IMapper mapper, ITrackerDbRepo repo, ILogger<SensorFuelReportConsumer> logger)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task Consume(ConsumeContext<SensorFuelReportRequest> context)
		{
			_logger.LogInformation("Start consuming message");

			var fuelLevels = await _repo.GetFuelSensorRawDataByFilter(context.Message);

			var fuelResponse = new SensorFuelReportResponse
			{
				FuelItems = _mapper.Map<List<SensorReportFuelItemContract>>(fuelLevels)
			};
			await context.RespondAsync(fuelResponse);

			_logger.LogInformation("End consuming message");
		}
	}
}
