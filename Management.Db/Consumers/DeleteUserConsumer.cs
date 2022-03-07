using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using Management.Db.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Management.Db.Consumers
{
    public class DeleteUserConsumer : IConsumer<DeleteUserRequest>
	{
		private readonly ILogger<DeleteUserConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly IMapper _mapper;

		public DeleteUserConsumer(IManagementDbRepo repo, ILogger<DeleteUserConsumer> logger, IMapper mapper)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

        public async Task Consume(ConsumeContext<DeleteUserRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var user = await _repo.DeleteUser(context.Message.Id);

			var response = _mapper.Map<DeleteUserResponse>(user);
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
    }
}
