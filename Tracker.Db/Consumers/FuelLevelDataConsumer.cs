using AutoMapper;
using Contracts;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Db.Entities;

namespace Tracker.Db.Consumers
{
	public class FuelLevelDataConsumer : IConsumer<FuelLevelDataMessage>
	{
		private readonly IMapper _mapper;
		private readonly ILogger<FuelLevelDataConsumer> _logger;
		private readonly ITrackerDbRepo _repo;
		private readonly IRequestClient<LocoInfosRequest> _locoInfosClient;

		public FuelLevelDataConsumer(IMapper mapper, ITrackerDbRepo repo, ILogger<FuelLevelDataConsumer> logger,
			IRequestClient<LocoInfosRequest> locoInfosClient)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_locoInfosClient = locoInfosClient ?? throw new ArgumentNullException(nameof(locoInfosClient));
		}

		public async Task Consume(ConsumeContext<FuelLevelDataMessage> context)
		{
			_logger.LogInformation("Start consuming message");

			if (!MassTransitHostedServiceDb.Locos.Any())
			{
				var locoInfosResponse = await _locoInfosClient.GetResponse<LocoInfosResponse>(new LocoInfosRequest());
				MassTransitHostedServiceDb.Locos = locoInfosResponse.Message.Locos;
			}

			var fuelSensor = await _repo.GetFuelSensor(context.Message.TrackerId);
			if (fuelSensor == null)
			{
				throw new Exception($"Датчик топлива с Id: {context.Message.TrackerId} не найден");
			}

			Guid? locoMapItemId = TryGetLocoMapItemId(fuelSensor.Id);
			if (!locoMapItemId.HasValue)
			{
				var locoInfosResponse = await _locoInfosClient.GetResponse<LocoInfosResponse>(new LocoInfosRequest());
				MassTransitHostedServiceDb.Locos = locoInfosResponse.Message.Locos;

				locoMapItemId = TryGetLocoMapItemId(fuelSensor.Id);
			}

			if (locoMapItemId.HasValue)
			{
				var uiTrackerDataMessage = _mapper.Map<UiFuelLevelDataMessage>((context.Message, locoMapItemId.Value));
				await context.Publish(uiTrackerDataMessage);
			}

			var fuelSensorRawData = _mapper.Map<FuelSensorRawData>((context.Message, fuelSensor.Id));
			await _repo.AddFuelSensorRawData(fuelSensorRawData);

			_logger.LogInformation("End consuming message");
		}

		private Guid? TryGetLocoMapItemId(Guid fuelSensorId)
		{
			Guid? locoMapItemId = null;
			foreach (var loco in MassTransitHostedServiceDb.Locos)
			{
				foreach (var sensorGroup in loco.SensorGroups)
				{
					var sensor = sensorGroup.Sensors.Find(x => x.FuelSensorId == fuelSensorId);
					if (sensor != null)
					{
						locoMapItemId = loco.Id;
						break;
					}
				}
			}
			return locoMapItemId;
		}
	}
}