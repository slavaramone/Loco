using AutoMapper;
using Contracts;
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
    public class UiFuelLevelDataConsumer : IConsumer<UiFuelLevelDataMessage>
    {
        private readonly IHubContext<ArmHub> _armHubContext;
        private readonly IHubContext<DispatcherHub> _dispatcherHubContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UiFuelLevelDataConsumer> _logger;

        public UiFuelLevelDataConsumer(IHubContext<ArmHub> armHubContext, IHubContext<DispatcherHub> dispatcherHubContext, IMapper mapper, ILogger<UiFuelLevelDataConsumer> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _armHubContext = armHubContext ?? throw new ArgumentNullException(nameof(armHubContext));
            _dispatcherHubContext = dispatcherHubContext ?? throw new ArgumentNullException(nameof(dispatcherHubContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<UiFuelLevelDataMessage> context)
        {
            _logger.LogInformation("Start consuming message");

            var model = _mapper.Map<FuelLevelDataModel>(context.Message);
            var fuelLevelModels = new FuelLevelDataModel[]
            {
                model
            };

            await _armHubContext.Clients.Group(model.LocoId.ToString().ToString()).SendAsync(HubMethod.Fuel, JsonSerializer.Serialize(fuelLevelModels, JsonSerializerCamelCaseOptions.Get()));

            await _dispatcherHubContext.Clients.All.SendAsync(HubMethod.Fuel, JsonSerializer.Serialize(fuelLevelModels, JsonSerializerCamelCaseOptions.Get()));

            _logger.LogInformation("End consuming message");
        }
    }
}