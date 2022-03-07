using AutoMapper;
using Contracts;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Db.Consumers
{
    public class CalibrationConsumer : IConsumer<CalibrationRequest>
    {
		private readonly ILogger<CalibrationConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly IMapper _mapper;
		private readonly ICalibrator _сalibrator;

		public CalibrationConsumer(IManagementDbRepo repo, ILogger<CalibrationConsumer> logger, IMapper mapper, ICalibrator сalibrator)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_сalibrator = сalibrator ?? throw new ArgumentNullException(nameof(сalibrator));
		}

		public async Task Consume(ConsumeContext<CalibrationRequest> context)
		{
			_logger.LogInformation("Start consuming message");

			var locos = await _repo.GetLocos(isOnlyActive: false);
			var calibratedFuelLevels = new List<CalibratedFuelLevelContract>();
            foreach (var fuelLevel  in context.Message.FuelLevels)
            {
				var sensor = await _repo.GetSensorWithSensorGroup(fuelLevel.FuelSensorId);
				if (sensor != null)
                {
					var loco = locos.Find(x => x.Id == sensor.SensorGroup.LocoId);
					if (loco != null && loco.MapItemId.HasValue)
                    {
						var fuelCalibrations = await _repo.GetFuelLevelCalibrations(sensor.Id);
						double calibratedValue = fuelLevel.RawValue;
						double maxValue = FuelLevelDataCalibrationConsumer.MaxRawValue;
						if (fuelCalibrations.Any())
						{
							var result = _сalibrator.Calibrate(fuelLevel.RawValue, fuelCalibrations);
							maxValue = result.MaxValue;
							calibratedValue = result.CalibratedValue;
						}
						calibratedFuelLevels.Add(_mapper.Map<CalibratedFuelLevelContract>((fuelLevel, calibratedValue, maxValue, loco.MapItemId.Value)));
					}
				}
				else
                {
					_logger.LogError($"Sensor not found trackerId={fuelLevel.FuelSensorId}");
				}					
			}

			var response = new CalibrationResponse
			{
				CalibratedFuelLevels = calibratedFuelLevels
			};
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
    }
}
