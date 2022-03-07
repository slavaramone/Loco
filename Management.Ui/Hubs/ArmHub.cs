using AutoMapper;
using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using Management.Ui.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SharedLib.Exceptions;
using SharedLib.Extensions;
using SharedLib.Options;
using SharedLib.Security;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Management.Ui.Hubs
{
	public class ArmHub : Hub
    {
        private readonly IRequestClient<InitMapDataRequest> _initMapDataRequest;
        private readonly IRequestClient<LocoAndSensorByApiKeyRequest> _locoIdByApiKey;
        private readonly IMapper _mapper;
        private readonly ILogger<ArmHub> _logger;

        public ArmHub(IRequestClient<InitMapDataRequest> initMapDataRequest, IMapper mapper, IRequestClient<LocoAndSensorByApiKeyRequest> locoIdByApiKey, 
            ILogger<ArmHub> logger)
        {
			_locoIdByApiKey = locoIdByApiKey ?? throw new ArgumentNullException(nameof(locoIdByApiKey));
            _initMapDataRequest = initMapDataRequest ?? throw new ArgumentNullException(nameof(initMapDataRequest));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            _logger.LogInformation("Start ArmHub connection update");

            if (Context.GetHttpContext().Request.Query.TryGetValue(AuthConfigurationExtensions.AccessTokenParamName, out var apiKeyStringValues) && apiKeyStringValues.Count > 0)
            {
                var initMapDataResponse = await _initMapDataRequest.GetResponse<InitMapDataResponse>(new InitMapDataRequest());

                var mapItemIdResponse = await _locoIdByApiKey.GetResponse<LocoAndSensorByApiKeyResponse>(new LocoAndSensorByApiKeyRequest
                {
                    ApiKey = apiKeyStringValues[0]
                });

				if (mapItemIdResponse.Message.IsActive)
				{
					var initMapDataForLoco = initMapDataResponse.Message.MapItems.FindAll(x => x.Type != MapItemType.ShuntingLocomotive ||
						(x.Type == MapItemType.ShuntingLocomotive && x.Id == mapItemIdResponse.Message.MapItemId));

					var mapItemModels = _mapper.Map<List<MapItemModel>>(initMapDataForLoco);

					var locoMapItemModel = mapItemModels.Find(x => x.Type == MapItemType.ShuntingLocomotive);

					await Groups.AddToGroupAsync(Context.ConnectionId, locoMapItemModel.Id.ToString());

					var fuelLevelContract = initMapDataResponse.Message.FuelLevels.Find(x => x != null && x.FuelSensorId == mapItemIdResponse.Message.FuelSensorId);
					var fuelLevelModels = new List<FuelLevelDataModel>();
					if (fuelLevelContract != null)
					{
						var fuelLevelModel = _mapper.Map<FuelLevelDataModel>((fuelLevelContract, locoMapItemModel.Id));
						fuelLevelModels = new List<FuelLevelDataModel> { fuelLevelModel };
					}
					else
					{
						_logger.LogWarning($"Fuel level for MapItemId={locoMapItemModel.Id} not found ");
					}

					await Clients.Client(Context.ConnectionId).SendAsync(HubMethod.Coordinates, JsonSerializer.Serialize(mapItemModels, JsonSerializerCamelCaseOptions.Get()));
					await Clients.Client(Context.ConnectionId).SendAsync(HubMethod.Fuel, JsonSerializer.Serialize(fuelLevelModels, JsonSerializerCamelCaseOptions.Get()));
				}
            }
            else
            {
                throw new HubAuthException(typeof(DispatcherHub).Name);
            }

            _logger.LogInformation("End ArmHub connection update");
        }
    }
}
