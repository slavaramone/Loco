using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Management.Db.Consumers
{
	public class UpdateLocoConsumer : IConsumer<UpdateLocoRequest>
	{
		private readonly ILogger<UpdateLocoConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly IMapper _mapper;

		public UpdateLocoConsumer(IManagementDbRepo repo, ILogger<UpdateLocoConsumer> logger, IMapper mapper)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Consume(ConsumeContext<UpdateLocoRequest> context)
		{
			_logger.LogInformation("Start consuming message");

			var msg = context.Message;
			var loco = await _repo.UpdateLoco(msg.Id, msg.Name, msg.IsActive);

			var response = _mapper.Map<UpdateLocoResponse>(loco);
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
	}
}
