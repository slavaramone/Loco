using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Management.Db.Consumers
{
    public class ShunterListConsumer : IConsumer<ShunterListRequest>
	{
		private readonly ILogger<LocoInfosConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly IMapper _mapper;

		public ShunterListConsumer(IManagementDbRepo repo, ILogger<LocoInfosConsumer> logger, IMapper mapper)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

        public async Task Consume(ConsumeContext<ShunterListRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var shunters = await _repo.GetShunters();
			var response = new ShunterListResponse
			{
				ShunterList = _mapper.Map<List<ShunterListItemContract>>(shunters)
			};
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
    }
}
