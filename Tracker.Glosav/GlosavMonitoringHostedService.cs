using Contracts.Commands;
using Contracts.Requests;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tracker.Glosav.Api.Monitoring.Models.Response;
using Tracker.Glosav.Common;
using Tracker.Glosav.Options;

namespace Tracker.Glosav
{
    public class GlosavMonitoringHostedService : MassTransitHostedService
	{
		/// <summary>
		/// Последние сообщения от глосав с координатами
		/// </summary>
		public static Dictionary<string, ApiMonitoringMessageModel> DevicesToLastMessage { get; set; } = new Dictionary<string, ApiMonitoringMessageModel>();

		private readonly GlosavMonitoringOptions _options;

		public GlosavMonitoringHostedService(ILogger<GlosavMonitoringHostedService> logger, IBusControl busControl, IOptions<GlosavMonitoringOptions> options)
			: base(busControl, logger)
		{
			_options = options?.Value ?? throw new ArgumentNullException(nameof(options));
		}

		public override async Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				await base.StartAsync(cancellationToken);

				var receiveMapItemsEndpoint = await _bus.GetSendEndpoint(new Uri("queue:Tracker.Db.ReceiveMapItems"));

				await receiveMapItemsEndpoint.Send<ReceiveMapItems>(new { }, cancellationToken).ConfigureAwait(false);

				await receiveMapItemsEndpoint.Send<SensorTrackerIdsRequest>(new { }, cancellationToken).ConfigureAwait(false);

				var schedulerEndpoint = await _bus.GetSendEndpoint(new Uri("queue:quartz"));

				await schedulerEndpoint.ScheduleRecurringSend<ReceiveGlosavDevices>(new Uri("queue:Glosav.ScheduledReceiveGlosavDevices"),
						new ReceiveGlosavDevicesSchedule(_options.ReceiveGlosavDevicesCronExpression), new { }, cancellationToken)
					.ConfigureAwait(false);

				await schedulerEndpoint.ScheduleRecurringSend<ReceiveGlosavFuel>(new Uri("queue:Glosav.ScheduledReceiveGlosavFuel"),
						new ReceiveGlosavFuelSchedule(_options.ReceiveGlosavFuelCronExpression), new { }, cancellationToken)
					.ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				throw;
			}
		}
	}
}
