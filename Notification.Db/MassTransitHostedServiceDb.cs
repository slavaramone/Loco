using Contracts;
using GeoLibrary.Model;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Notification.Db
{
	public class MassTransitHostedServiceDb : MassTransitHostedService
    {	
		/// <summary>
		/// Список объектов на карте, обновляется при старте
		/// </summary>
		public static List<MapItemContract> MapItems { get; set; } = new List<MapItemContract>();

		/// <summary>
		/// Словарь trackerId локомотива к предыдущим его координатам на карте
		/// </summary>
		public static ConcurrentDictionary<string, Point> LocoTrackerIdToMapPoint { get; set; } = new ConcurrentDictionary<string, Point>();

		public MassTransitHostedServiceDb(IBusControl bus, ILogger<MassTransitHostedService> logger) : base(bus, logger)
		{
		}
	}
}
