using Contracts.Responses;
using OfficeOpenXml;
using SharedLib.Extensions;
using System.Collections.Generic;

namespace Management.Ui.Services
{
    public class ExcelService : IExcelService
	{
		private const string ReportDateFormat = "dd-MM-yyyy HH:mm:ss";
		private const string NotificationReportSheetName = "Уведомления";

		public byte[] CreateNotificationReport(List<NotificationListItemContract> notificationList)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add(NotificationReportSheetName);
				worksheet.Cells[1, 1].Value = "Дата";
				worksheet.Cells[1, 2].Value = "Важность";
				worksheet.Cells[1, 3].Value = "Сообщение";

				int currentRow = 2;
				foreach (var item in notificationList)
				{
					worksheet.Cells[currentRow, 1].Value = item.CreationDateTime.ToString(ReportDateFormat);
					worksheet.Cells[currentRow, 2].Value = item.Severity.GetDescription();
					worksheet.Cells[currentRow, 3].Value = item.Message;

					currentRow++;
				}
				return package.GetAsByteArray();
			}
		}

		public byte[] CreateCoordReport(List<LocoReportCoordItemContract> coords)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage())
			{
                var worksheet = package.Workbook.Worksheets.Add(NotificationReportSheetName);

				worksheet.Column(1).Width = 40;
				worksheet.Column(2).Width = 20;
				worksheet.Column(3).Width = 11;
				worksheet.Column(4).Width = 11;

				worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Дата";
                worksheet.Cells[1, 3].Value = "Широта";
                worksheet.Cells[1, 4].Value = "Долгота";
                worksheet.Cells[1, 5].Value = "Высота";
                worksheet.Cells[1, 6].Value = "Скорость";

                int currentRow = 2;
                foreach (var item in coords)
                {
					worksheet.Cells[currentRow, 1].Value = item.LocoId;
                    worksheet.Cells[currentRow, 2].Value = item.TrackDateTime.ToString(ReportDateFormat);
                    worksheet.Cells[currentRow, 3].Value = item.Latitude;
                    worksheet.Cells[currentRow, 4].Value = item.Longitude;
                    worksheet.Cells[currentRow, 5].Value = item.Altitude;
                    worksheet.Cells[currentRow, 6].Value = item.Speed;

                    currentRow++;
                }
                return package.GetAsByteArray();
			}
		}

		public byte[] CreateFuelReport(List<LocoReportFuelItemContract> fuels)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add(NotificationReportSheetName);

				worksheet.Column(1).Width = 40;
				worksheet.Column(2).Width = 20;
				worksheet.Column(3).Width = 11;

				worksheet.Cells[1, 1].Value = "Id";
				worksheet.Cells[1, 2].Value = "Дата";
				worksheet.Cells[1, 3].Value = "Значение";

				int currentRow = 2;
				foreach (var item in fuels)
				{
					worksheet.Cells[currentRow, 1].Value = item.LocoId;
					worksheet.Cells[currentRow, 2].Value = item.ReportDateTime.ToString(ReportDateFormat);
					worksheet.Cells[currentRow, 3].Value = item.CalibratedValue;

					currentRow++;
				}
				return package.GetAsByteArray();
			}
		}
	}
}
