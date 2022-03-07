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
    public class LocoVideoStreamConsumer : IConsumer<LocoVideoStreamRequest>
    {
        private readonly ILogger<LocoVideoStreamConsumer> _logger;
        private readonly IManagementDbRepo _repo;
        private readonly IMapper _mapper;

        public LocoVideoStreamConsumer(IMapper mapper, IManagementDbRepo repo, ILogger<LocoVideoStreamConsumer> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<LocoVideoStreamRequest> context)
        {
            _logger.LogInformation("Start consuming message");

            var videoStreams = await _repo.GetLocoVideoStreams(context.Message.LocoId);

            var response = new LocoVideoStreamResponse
            {
                VideoStreams = _mapper.Map<List<LocoVideoStreamContract>>(videoStreams)
            };
            await context.RespondAsync(response);

            _logger.LogInformation("End consuming message");
        }
    }
}
