using System;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.Commands;
using Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Tracker.Glosav.Consumers
{
	/// <summary>
	/// Потребитель реализует функциональность обработки события получения конфигурации запрашиваемых устройств из Glosav-кластера
	/// Сама конфигурация производится в Handler через UseLatest, целью же данного потребителя является отправка команды на
	/// получение данных по устройствам из Glosav-кластера
	/// </summary>
	public class MapItemsReceivedConsumer : IConsumer<MapItemsReceived>
	{
		private readonly IMapper _mapper;
		private readonly ILogger<MapItemsReceivedConsumer> _logger;

		public MapItemsReceivedConsumer(IMapper mapper, ILogger<MapItemsReceivedConsumer> logger)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Consume(ConsumeContext<MapItemsReceived> context)
		{
			var receiveGlosavDevicesEndpoint = await context.GetSendEndpoint(new Uri("queue:Glosav.ReceiveGlosavDevices"));
			await receiveGlosavDevicesEndpoint.Send<ReceiveGlosavDevices>(new { });

			var receiveGlosavFuelEndpoint = await context.GetSendEndpoint(new Uri("queue:Glosav.ReceiveGlosavFuel"));
			await receiveGlosavFuelEndpoint.Send<ReceiveGlosavFuel>(new { });
		}
	}
}
