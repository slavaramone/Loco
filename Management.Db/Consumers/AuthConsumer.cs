using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Management.Db.Consumers
{
	public class AuthConsumer : IConsumer<AuthRequest>
	{
		private readonly ILogger<AuthConsumer> _logger;
		private readonly IManagementDbRepo _repo;
		private readonly IMapper _mapper;

		public AuthConsumer(IManagementDbRepo repo, ILogger<AuthConsumer> logger, IMapper mapper)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

        public async Task Consume(ConsumeContext<AuthRequest> context)
        {
			_logger.LogInformation("Start consuming message");

			var user = await _repo.GetUserWithRoles(context.Message.Login, context.Message.Password);
			
			var response = _mapper.Map<AuthResponse>(user);
			
			await context.RespondAsync(response);

			_logger.LogInformation("End consuming message");
		}
    }
}
