using Contracts.Responses;
using System.Collections.Generic;

namespace Management.Ui.Services
{
	public interface IExcelService
	{
		byte[] CreateNotificationReport(List<NotificationListItemContract> notificationList);

		byte[] CreateCoordReport(List<LocoReportCoordItemContract> coords);

		byte[] CreateFuelReport(List<LocoReportFuelItemContract> fuels);
	}
}
