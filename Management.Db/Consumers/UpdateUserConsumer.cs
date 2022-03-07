using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Management.Db.Consumers
{
    public class UpdateUserConsumer : IConsumer<UpdateUserRequest>
    {
		private readonly ILogger<UpdateUserConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly IMapper _mapper;

		public UpdateUserConsumer(IManagementDbRepo repo, ILogger<UpdateUserConsumer> logger, IMapper mapper)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Consume(ConsumeContext<UpdateUserRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var msg = context.Message;
			var user = await _repo.UpdateUser(msg.Id, msg.IsActive, msg.NewPassword);

			var response = _mapper.Map<UpdateUserResponse>(user);
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
    }
}
