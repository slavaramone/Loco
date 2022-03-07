using AutoMapper;
using Contracts;
using Contracts.Responses;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLib.Exceptions;
using SharedLib.Filters;
using System;
using System.Threading.Tasks;

namespace Tracker.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[ApiKeyAuth]
	public class TrackerController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ILogger<TrackerController> _logger;
		private readonly IRequestClient<TrackerDataMessage> _client;
		private readonly IPublishEndpoint _publishEndpoint;

		public TrackerController(IMapper mapper, IRequestClient<TrackerDataMessage> client, ILogger<TrackerController> logger, IPublishEndpoint publishEndpoint)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
		}

		/// <summary>
		/// Передача gps данных трекера
		/// </summary>
		/// <param name="request"></param>
		/// <param name="trackerId">Идентификатор трекера</param>
		/// <returns></returns>
		[HttpPost("track/{trackerId}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Tracker([FromBody] TrackRequest request, string trackerId)
		{
			try
			{
				var msg = _mapper.Map<TrackerDataMessage>((request, trackerId));
				var response = await _client.GetResponse<TrackerDataResponse>(msg);
				return new NoContentResult();
			}
			catch (RequestFaultException ex)
			{
				_logger.LogError(ex.Message);
				if (ex.Fault.Exceptions[0].ExceptionType != typeof(NotFoundException).FullName)
				{					
					return StatusCode(StatusCodes.Status404NotFound, ex.Message);
				}
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Передача данных ДУТ
		/// </summary>
		/// <param name="trackerId"></param>
		/// <param name="value">Идентификатор трекера</param>
		/// <returns></returns>
		[HttpPost("fuel/{trackerId}/{value}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Fuel([FromRoute] string trackerId, [FromRoute] double value)
		{
			try
			{
				await _publishEndpoint.Publish<FuelLevelDataMessage>(new
				{
					FuelLevel = value,
					TrackerId = trackerId,
					ReportDateTime = DateTimeOffset.Now
				});
				return new NoContentResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}