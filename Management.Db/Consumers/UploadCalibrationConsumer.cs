using Contracts.Requests;
using Contracts.Responses;
using Management.Db.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using SharedLib.Calculators;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Management.Db.Consumers
{
    public class UploadCalibrationConsumer : IConsumer<UploadCalibrationRequest>
	{
		private readonly ILogger<UploadCalibrationConsumer> _logger;
		private readonly IManagementDbRepo _repo;

		public UploadCalibrationConsumer(IManagementDbRepo repo, ILogger<UploadCalibrationConsumer> logger)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

        public async  Task Consume(ConsumeContext<UploadCalibrationRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			using (MemoryStream memStream = new MemoryStream())
			{
				BinaryFormatter binForm = new BinaryFormatter();
				memStream.Write(context.Message.FileBytes, 0, context.Message.FileBytes.Length);
				memStream.Seek(0, SeekOrigin.Begin);

				await _repo.DeleteCalibrations(context.Message.LeftSensorId);
				await _repo.DeleteCalibrations(context.Message.RightSensorId);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using (ExcelPackage package = new ExcelPackage(memStream))
				{
					var worksheet = package.Workbook.Worksheets[context.Message.WorksheetName];
					int currentRow = context.Message.StartRow;
					while (true)
					{
						if (worksheet.Cells[currentRow, context.Message.StartCol].Value == null)
						{
							break;
						}
						string leftCalibratedValue = worksheet.Cells[currentRow, context.Message.StartCol].Value.ToString();
						string leftRawValue = worksheet.Cells[currentRow, context.Message.StartCol + 1].Value.ToString();
						string rightCalibratedValue = worksheet.Cells[currentRow, context.Message.StartCol + 2].Value.ToString();
						string rightRawValue = worksheet.Cells[currentRow, context.Message.StartCol + 3].Value.ToString();

						var leftSensorCalibration = new FuelLevelCalibration
						{
							FuelLevelSensorId = context.Message.LeftSensorId,
							RawValue = double.Parse(leftRawValue),
							CalibratedValue = double.Parse(leftCalibratedValue)
						};
						await _repo.AddFuelLevelCalibration(leftSensorCalibration);

						var rightSensorCalibration = new FuelLevelCalibration
						{
							FuelLevelSensorId = context.Message.RightSensorId,
							RawValue = double.Parse(rightRawValue),
							CalibratedValue = double.Parse(rightCalibratedValue)
						};
						await _repo.AddFuelLevelCalibration(rightSensorCalibration);

						currentRow++;
					}
				}
			}

            var response = new UploadCalibrationResponse
			{
				IsCompletedSuccessfuly = true
			};
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
    }
}
