using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tracker.Db.Consumers
{
    public class LocoCoordReportConsumer : IConsumer<LocoCoordReportRequest>
    {
		private readonly IMapper _mapper;
		private readonly ILogger<LocoCoordReportConsumer> _logger;
		private readonly ITrackerDbRepo _repo;

		public LocoCoordReportConsumer(IMapper mapper, ITrackerDbRepo repo, ILogger<LocoCoordReportConsumer> logger)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task Consume(ConsumeContext<LocoCoordReportRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var rawGeoData = await _repo.GetRawGeoDataByFilter(context.Message);
			var coordResponse = new LocoCoordReportResponse
			{
				CoordItems = _mapper.Map<List<LocoReportCoordItemContract>>(rawGeoData)
			};
			await context.RespondAsync(coordResponse);

			_logger.LogInformation("End consuming message");
		}
    }
}
