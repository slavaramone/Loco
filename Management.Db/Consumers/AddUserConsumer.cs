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
    public class AddUserConsumer : IConsumer<AddUserRequest>
    {
		private readonly ILogger<AddUserConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly IMapper _mapper;

		public AddUserConsumer(IManagementDbRepo repo, ILogger<AddUserConsumer> logger, IMapper mapper)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Consume(ConsumeContext<AddUserRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var user = _mapper.Map<User>(context.Message);
			await _repo.AddUser(user);

			var response = _mapper.Map<AddUserResponse>((user, context.Message));
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
    }
}
