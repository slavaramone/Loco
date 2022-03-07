using AutoMapper;
using Contracts;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tracker.Db.Consumers
{
	public class StaticObjectListConsumer : IConsumer<StaticObjectListRequest>
	{
		private readonly ILogger<StaticObjectListConsumer> _logger;
		private readonly IMapper _mapper;
		private readonly ITrackerDbRepo _repo;

		public StaticObjectListConsumer(ILogger<StaticObjectListConsumer> logger, ITrackerDbRepo repo, IMapper mapper)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task Consume(ConsumeContext<StaticObjectListRequest> context)
		{
			var staticMapItems = await _repo.GetStaticMapItemsWithLatestGeoData();

			var response = new StaticObjectListResponse
			{
				Items = _mapper.Map<List<StaticMapItemContract>>(staticMapItems)
			};
			await context.RespondAsync(response);
		}
	}
}
