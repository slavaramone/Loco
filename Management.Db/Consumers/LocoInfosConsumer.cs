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
	public class LocoInfosConsumer : IConsumer<LocoInfosRequest>
	{
		private readonly ILogger<LocoInfosConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly IMapper _mapper;

		public LocoInfosConsumer(IManagementDbRepo repo, ILogger<LocoInfosConsumer> logger, IMapper mapper)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task Consume(ConsumeContext<LocoInfosRequest> context)
		{
			_logger.LogInformation("Start consuming message");

			var locos = await _repo.GetLocosWithCamerasAndSensors(context.Message);
			var response = new LocoInfosResponse
			{
				Locos = _mapper.Map<List<LocoContract>>(locos)
			};			
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
	}
}
