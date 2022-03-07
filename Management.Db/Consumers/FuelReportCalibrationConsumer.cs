using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Calculators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Management.Db.Consumers
{
    public class FuelReportCalibrationConsumer : IConsumer<FuelReportCalibrationRequest>
    {
		private readonly ILogger<FuelReportCalibrationConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly ICalibrator _сalibrator;

		public FuelReportCalibrationConsumer(IManagementDbRepo repo, ILogger<FuelReportCalibrationConsumer> logger, ICalibrator сalibrator)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_сalibrator = сalibrator ?? throw new ArgumentNullException(nameof(сalibrator));
		}

		public async Task Consume(ConsumeContext<FuelReportCalibrationRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var sensorIdToFuelCalibrations = new Dictionary<Guid, Dictionary<double, double>>();
			var sensors = await _repo.GetSensorsWithSensorGroup();
			var calibratedFuelItems = new List<LocoReportFuelItemContract>();
			foreach (var item in context.Message.FuelItems)
            {
				var sensor = sensors.Find(x => x.FuelSensorId == item.FuelSensorId);
				if (sensor != null)
                {
					Dictionary<double, double> sensorFuelCalibrations;
					if (!sensorIdToFuelCalibrations.ContainsKey(sensor.Id))
					{
						sensorFuelCalibrations = await _repo.GetFuelLevelCalibrations(sensor.Id);
						sensorIdToFuelCalibrations.Add(sensor.Id, sensorFuelCalibrations);
					}
					else
					{
						sensorFuelCalibrations = sensorIdToFuelCalibrations[sensor.Id];
					}

					var result = _сalibrator.Calibrate(item.RawValue, sensorFuelCalibrations);

					calibratedFuelItems.Add(new LocoReportFuelItemContract
					{
						LocoId = sensor.SensorGroup.LocoId,
						CalibratedValue = result.CalibratedValue,
						ReportDateTime = item.ReportDateTime
					});
				}
			}
			await context.RespondAsync(new FuelReportCalibrationResponse
			{
				CalibratedFuelItems = calibratedFuelItems
			});

			_logger.LogInformation("End consuming message");
		}
    }
}
