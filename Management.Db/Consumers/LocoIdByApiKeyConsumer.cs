using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Management.Db.Consumers
{
    public class LocoMapItemIdByApiKeyConsumer : IConsumer<LocoAndSensorByApiKeyRequest>
    {
		private readonly ILogger<LocoMapItemIdByApiKeyConsumer> _logger;
		private readonly IManagementDbRepo _repo;

		public LocoMapItemIdByApiKeyConsumer(IManagementDbRepo repo, ILogger<LocoMapItemIdByApiKeyConsumer> logger)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Consume(ConsumeContext<LocoAndSensorByApiKeyRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var loco = await _repo.GetLocoByApiKey(context.Message.ApiKey);
			var response = new LocoAndSensorByApiKeyResponse
			{
				IsActive = loco.IsActive
			};
			if (loco.IsActive)
			{
				var sensorId = await _repo.GetLocoSensorId(loco.Id);
				response.MapItemId = loco.MapItemId.Value;
				response.FuelSensorId = sensorId;
			}
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
    }
}
