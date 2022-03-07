using AutoMapper;
using Contracts;
using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker.Db.Consumers
{
	public class LocoChartConsumer : IConsumer<DateAxisChartRequest>
	{
		private readonly IMapper _mapper;
		private readonly ILogger<LocoChartConsumer> _logger;
		private readonly ITrackerDbRepo _repo;
		private readonly IRequestClient<LocoInfosRequest> _locoInfosClient;

		public LocoChartConsumer(IMapper mapper, ITrackerDbRepo repo, ILogger<LocoChartConsumer> logger, IRequestClient<LocoInfosRequest> locoInfosClient)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_locoInfosClient = locoInfosClient ?? throw new ArgumentNullException(nameof(locoInfosClient));
		}

		public async Task Consume(ConsumeContext<DateAxisChartRequest> context)
		{
			_logger.LogInformation("Start consuming message");

			var chartItemContracts = new List<LocoChartItemContract>();
			var minuteTimeSpan = new TimeSpan(0, 1, 0);
			DateTimeOffset currentDate = context.Message.StartDateTime.RoundUp(minuteTimeSpan);
			switch (context.Message.Type)
			{
				case ChartType.Speed:
					var speedChartItems = await _repo.GetSpeedChartItems(context.Message.LocoId, currentDate, context.Message.EndDateTime);
					chartItemContracts = _mapper.Map<List<LocoChartItemContract>>(speedChartItems);
					break;

				case ChartType.Fuel:
					var locoInfosResponse = await _locoInfosClient.GetResponse<LocoInfosResponse>(new LocoInfosRequest
					{
						LocoIds = new List<Guid> { context.Message.LocoId }
					});
					Guid sensorId = locoInfosResponse.Message.Locos[0].SensorGroups[0].Sensors[0].FuelSensorId;
					var fuelChartItems = await _repo.GetFuelChartItems(sensorId, currentDate, context.Message.EndDateTime);
					chartItemContracts = _mapper.Map<List<LocoChartItemContract>>(fuelChartItems);
					break;
				default:
					break;
			}

			var msg = new DateAxisChartResponse
			{
				ChartItems = chartItemContracts
			};
			await context.RespondAsync(msg);

			_logger.LogInformation("End consuming message");
		}
	}
}