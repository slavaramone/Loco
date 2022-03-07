using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Calculators;
using SharedLib.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Db.Consumers
{
    public class FuelLevelDataCalibrationConsumer : IConsumer<FuelLevelDataCalibrationMessage>
    {
		public const double MaxRawValue = 4095;

		private readonly IMapper _mapper;
		private readonly ILogger<FuelLevelDataCalibrationConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly ICalibrator _сalibrator;

		public FuelLevelDataCalibrationConsumer(IMapper mapper, IManagementDbRepo repo, ILogger<FuelLevelDataCalibrationConsumer> logger, ICalibrator сalibrator)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _сalibrator = сalibrator ?? throw new ArgumentNullException(nameof(сalibrator));
		}

		public async Task Consume(ConsumeContext<FuelLevelDataCalibrationMessage> context)
        {
			_logger.LogInformation("Start consuming message");

			var sensor = await _repo.GetSensorWithSensorGroup(context.Message.FuelSensorId);
            if (sensor == null)
            {
                throw new NotFoundException(context.Message.FuelSensorId.ToString());
            }

            if (sensor.SensorGroup.IsTakeAverageValue)
            {
                //TODO: сделать усреднение значения используя другие датчики группы
            }

            Guid mapItemId = await _repo.GetLocoMapItemIdById(sensor.SensorGroup.LocoId);

            var fuelCalibrations = await _repo.GetFuelLevelCalibrations(sensor.Id);
            var result = _сalibrator.Calibrate(context.Message.FuelLevel, fuelCalibrations);

            var uiTrackerDataMessage = _mapper.Map<UiFuelLevelDataMessage>((result, context.Message.ReportDateTime, mapItemId));
            await context.Publish(uiTrackerDataMessage);

            _logger.LogInformation("End consuming message");
		}
    }
}
