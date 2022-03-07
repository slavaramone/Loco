using AutoMapper;
using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using Management.Ui.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SharedLib.Exceptions;
using SharedLib.Options;
using SharedLib.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Management.Ui.Hubs
{
    public class DispatcherHub : Hub
    {
        private readonly IRequestClient<InitMapDataRequest> _initMapDataRequest;
        private readonly IMapper _mapper;
        private readonly IClaimsAccessor _claimsAccessor;
        private readonly ILogger<DispatcherHub> _logger;
        private readonly IRequestClient<LocoInfosRequest> _locoInfosClient;

        public DispatcherHub(IRequestClient<InitMapDataRequest> initMapDataRequest, IMapper mapper, IClaimsAccessor claimsAccessor,
            ILogger<DispatcherHub> logger, IRequestClient<LocoInfosRequest> locoInfosClient)
        {
            _initMapDataRequest = initMapDataRequest ?? throw new ArgumentNullException(nameof(initMapDataRequest));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _claimsAccessor = claimsAccessor ?? throw new ArgumentNullException(nameof(claimsAccessor));
            _locoInfosClient = locoInfosClient ?? throw new ArgumentNullException(nameof(locoInfosClient));
        }

        public override async Task OnConnectedAsync()
        {
            await Update();

            await base.OnConnectedAsync();
        }

        //TODO: убрать когда андрей уберет в ui
        public Task Send(string msg)
        {
            return Update();
        }

        public async Task Update()
        {
            _logger.LogInformation("Start DispatcherHub connection update");

            if (_claimsAccessor.TryGetValue(ClientApiSecurity.Claims.UserId, out string userId))
            {
                var initMapDataResponse = await _initMapDataRequest.GetResponse<InitMapDataResponse>(new InitMapDataRequest());
                var mapItemModels = _mapper.Map<List<MapItemModel>>(initMapDataResponse.Message.MapItems);

                var fuelLevelModels = new List<FuelLevelDataModel>();

                var mapItemIds = initMapDataResponse.Message.MapItems.Select(x => x.Id)
                    .ToList();

                var locoMapItemModels = mapItemModels.FindAll(x => x.Type == MapItemType.ShuntingLocomotive);

                var locoInfosResponse = await _locoInfosClient.GetResponse<LocoInfosResponse>(new LocoInfosRequest
                {
                    LocoIds = mapItemIds
                });

                foreach (var locoInfo in locoInfosResponse.Message.Locos)
                {
                    Guid sensorId = locoInfo.SensorGroups[0].Sensors[0].FuelSensorId;

                    var fuelLevelContract = initMapDataResponse.Message.FuelLevels.Find(x => x != null && x.FuelSensorId.Equals(sensorId));
                    if (fuelLevelContract != null)
                    {
                        var fuelLevelModel = _mapper.Map<FuelLevelDataModel>((fuelLevelContract, locoInfo.Id));
                        fuelLevelModels.Add(fuelLevelModel);
                    }
                    else
                    {
                        _logger.LogWarning($"Fuel level for Locod={locoInfo.Id} not found ");
                    }
                }

                await Clients.Client(Context.ConnectionId).SendAsync(HubMethod.Coordinates, JsonSerializer.Serialize(mapItemModels, JsonSerializerCamelCaseOptions.Get()));
                await Clients.Client(Context.ConnectionId).SendAsync(HubMethod.Fuel, JsonSerializer.Serialize(fuelLevelModels, JsonSerializerCamelCaseOptions.Get()));
            }
            else
            {
                throw new HubAuthException(typeof(DispatcherHub).Name);
            }

            _logger.LogInformation("End DispatcherHub connection update");
        }
    }
}
