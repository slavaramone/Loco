using System;
using System.Threading.Tasks;
using Tracker.Glosav.Api.Reports.Models.Request;
using Tracker.Glosav.Api.Reports.Models.Response;

namespace Tracker.Glosav.Api.Reports.Client
{
	public interface IGlosavReportsClient
	{
		Task<ApiFuelReportResponse> GetFuelReport(ApiFuelReportRequestQuery query, ApiFuelReportRequestPayload payload);

		Task<ApiCompositionFuelReportResponse> GetCompositionFuelReport(ApiCompositionFuelReportRequestQuery query);
	}
}
