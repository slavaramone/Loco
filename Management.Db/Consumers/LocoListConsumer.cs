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
    public class LocoListConsumer : IConsumer<LocoListRequest>
	{
		private readonly ILogger<LocoInfosConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly IMapper _mapper;

		public LocoListConsumer(IManagementDbRepo repo, ILogger<LocoInfosConsumer> logger, IMapper mapper)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

        public async Task Consume(ConsumeContext<LocoListRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var locos = await _repo.GetLocos(context.Message.IsOnlyActive);
			var response = new LocoListResponse
			{
				LocoList = _mapper.Map<List<LocoListItemContract>>(locos)
			};
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
    }
}
