using AutoMapper;
using Contracts;
using Contracts.Enums;
using Management.Ui.Hubs;
using Management.Ui.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SharedLib.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Management.Ui.Consumers
{
    public class UiTrackerDataConsumer : IConsumer<UiTrackerDataMessage>
    {
        private readonly IHubContext<ArmHub> _armHubContext;
        private readonly IHubContext<DispatcherHub> _dispatcherHubContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UiTrackerDataConsumer> _logger;

        public UiTrackerDataConsumer(IHubContext<ArmHub> armHubContext, IHubContext<DispatcherHub> dispatcherHubContext, IMapper mapper, ILogger<UiTrackerDataConsumer> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _armHubContext = armHubContext ?? throw new ArgumentNullException(nameof(armHubContext));
            _dispatcherHubContext = dispatcherHubContext ?? throw new ArgumentNullException(nameof(dispatcherHubContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<UiTrackerDataMessage> context)
        {
            _logger.LogInformation("Start consuming message");

            var model = _mapper.Map<MapItemModel>(context.Message);

            var coordinates = new MapItemModel[]
            {
                model
            };


            if (model.Type == MapItemType.Shunter)
            {
                await _armHubContext.Clients.All.SendAsync(HubMethod.Coordinates, JsonSerializer.Serialize(coordinates, JsonSerializerCamelCaseOptions.Get()));
            }
            else
            {
                await _armHubContext.Clients.Group(model.Id.ToString().ToString()).SendAsync(HubMethod.Coordinates, JsonSerializer.Serialize(coordinates, JsonSerializerCamelCaseOptions.Get()));
            }

            await _dispatcherHubContext.Clients.All.SendAsync(HubMethod.Coordinates, JsonSerializer.Serialize(coordinates, JsonSerializerCamelCaseOptions.Get()));

            _logger.LogInformation("End consuming message");
        }
    }
}
