using AutoMapper;
using Contracts;
using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLib.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Tracker.Db.Entities;

namespace Tracker.Db.Consumers
{
    public class TrackerDataConsumer : IConsumer<TrackerDataMessage>
	{
		/// <summary>
		/// Словарь mapItemId локомотива ко времени получения последних координат
		/// </summary>
		public static ConcurrentDictionary<Guid, DateTimeOffset> MapItemIdToDto { get; set; } = new ConcurrentDictionary<Guid, DateTimeOffset>();

		private readonly PeriodOptions _periodOptions;
		private readonly ILogger<TrackerDataConsumer> _logger;
		private readonly ITrackerDbRepo _repo;
		private readonly IMapper _mapper;

		public TrackerDataConsumer(IMapper mapper, ITrackerDbRepo repo, ILogger<TrackerDataConsumer> logger, IOptions<PeriodOptions> periodOptions)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_periodOptions = periodOptions.Value ?? throw new ArgumentNullException(nameof(periodOptions.Value));
		}

		public async Task Consume(ConsumeContext<TrackerDataMessage> context)
		{
			_logger.LogInformation("Start consuming message");

			var mapItem = await _repo.GetMapItemByTrackerId(context.Message.TrackerId);
			if (mapItem == null)
			{
				throw new NotFoundException(context.Message.TrackerId);
			}

			bool isNewMapItem = false;
			if (MapItemIdToDto.TryGetValue(mapItem.Id, out DateTimeOffset latestTrackDateTime))
            {
				MapItemIdToDto[mapItem.Id] = context.Message.TrackDate;
			}
			else
            {
				MapItemIdToDto.TryAdd(mapItem.Id, context.Message.TrackDate);
				isNewMapItem = true;
			}
			
			if ((isNewMapItem || context.Message.TrackDate - latestTrackDateTime >= new TimeSpan(0, 0, _periodOptions.TrackerDataRefreshSeconds))
				&& DateTimeOffset.Now - context.Message.TrackDate < new TimeSpan(0, 0, _periodOptions.ArchiveDataDelaySeconds))
            {
				var uiTrackerDataMessage = _mapper.Map<UiTrackerDataMessage>((context.Message, mapItem));
				await context.Publish(uiTrackerDataMessage);
			}

			var entity = _mapper.Map<RawGeoData>((context.Message, mapItem.Id));
			Guid id = await _repo.AddRawGeoPoint(entity);

			await context.RespondAsync(new TrackerDataResponse
			{
				Id = id
			});
			_logger.LogInformation("End consuming message");
		}
	}
}
