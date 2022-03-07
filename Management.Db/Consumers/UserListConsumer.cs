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
    public class UserListConsumer : IConsumer<UserListRequest>
	{
		private readonly ILogger<UserListConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly IMapper _mapper;

		public UserListConsumer(IManagementDbRepo repo, ILogger<UserListConsumer> logger, IMapper mapper)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

        public async Task Consume(ConsumeContext<UserListRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var users = await _repo.GetUsers();

			var response = new UserListResponse
			{
				Users = _mapper.Map<List<UserContract>>(users)
			};
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
    }
}
