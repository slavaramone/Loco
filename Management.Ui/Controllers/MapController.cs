using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLib.Filters;
using SharedLib.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Management.Ui.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[TokenAuth]
	public class MapController : ClaimsController
	{
		private readonly IMapper _mapper;
		private readonly ILogger<MapController> _logger;
		private readonly IRequestClient<Contracts.Requests.SpeedZoneRequest> _speedZoneRequest;

		public MapController(IMapper mapper, ILogger<MapController> logger, IClaimsAccessor claimsAccessor, IRequestClient<Contracts.Requests.SpeedZoneRequest> speedZoneRequest)
			: base(claimsAccessor)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_speedZoneRequest = speedZoneRequest ?? throw new ArgumentNullException(nameof(speedZoneRequest));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		/// <summary>
		/// Получение областей, отвечающих за максимальную скорость
		/// </summary>
		/// <returns></returns>
		[HttpGet("zones")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<SpeedZoneResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Zones()
		{
			try
			{
				var mtResponse = await _speedZoneRequest.GetResponse<Contracts.Responses.SpeedZoneResponse>(new Contracts.Requests.SpeedZoneRequest());
				var response = _mapper.Map<List<SpeedZoneResponse>>(mtResponse.Message.Items);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
