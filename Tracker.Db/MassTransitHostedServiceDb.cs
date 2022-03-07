using Contracts.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib;
using System.Collections.Generic;

namespace Tracker.Db
{
	public class MassTransitHostedServiceDb : MassTransitHostedService
	{
		/// <summary>
		/// Список локомотивов, обновляется при старте сервиса
		/// </summary>
		public static List<LocoContract> Locos { get; set; } = new List<LocoContract>();

		public MassTransitHostedServiceDb(IBusControl bus, ILogger<MassTransitHostedService> logger) : base(bus, logger)
		{
		}
	}
}
