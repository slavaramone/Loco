using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Tracker.Db.Consumers
{
    public class SensorTrackerIdsConsumer : IConsumer<SensorTrackerIdsRequest>
    {
        private readonly ILogger<SensorTrackerIdsConsumer> _logger;
        private readonly ITrackerDbRepo _repo;

        public SensorTrackerIdsConsumer(ILogger<SensorTrackerIdsConsumer> logger, ITrackerDbRepo repo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task Consume(ConsumeContext<SensorTrackerIdsRequest> context)
        {
            _logger.LogInformation("Start consuming message");

            var trackerIds = await _repo.GetFuelSensorTrackerIds();

            var response = new SensorTrackerIdsResponse
            {
                TrackerIds = trackerIds
            };
            await context.RespondAsync(response);

            _logger.LogInformation("End consuming message");
        }
    }
}
