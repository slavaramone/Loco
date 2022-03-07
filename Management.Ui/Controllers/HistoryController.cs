using AutoMapper;
using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using Management.Ui.Services;
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
	public class HistoryController : ClaimsController
	{
		private readonly IMapper _mapper;
		private readonly ILogger<HistoryController> _logger;
		private readonly IRequestClient<StaticObjectListRequest> _staticObjectListRequest;
		private readonly IRequestClient<Contracts.Requests.LocoHistoryRequest> _locoHistoryRequest;
		public HistoryController(IArchiveService archiveService, IMapper mapper, ILogger<HistoryController> logger, IRequestClient<StaticObjectListRequest> staticObjectListRequest, 
			IClaimsAccessor claimsAccessor, IRequestClient<Contracts.Requests.LocoHistoryRequest> locoHistoryRequest) : base(claimsAccessor)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_staticObjectListRequest = staticObjectListRequest ?? throw new ArgumentNullException(nameof(staticObjectListRequest));
			_locoHistoryRequest = locoHistoryRequest ?? throw new ArgumentNullException(nameof(locoHistoryRequest));
		}

		/// <summary>
		/// Получить историю координат
		/// </summary>
		/// <returns></returns>
		[HttpGet("coordinates")]
		[Produces("application/json")]
		[ProducesResponseType((typeof(List<CoordinatesHistoryResponse>)), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Coordinates([FromQuery] LocoHistoryRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.LocoHistoryRequest>((request, LocoHistoryType.Coordinates));

				var mtResponse = await _locoHistoryRequest.GetResponse<Contracts.Responses.CoordinatesHistoryResponse>(mtRequest);

				var response = _mapper.Map<List<CoordinatesHistoryResponse>>(mtResponse.Message.Items);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получить историю топлива
		/// </summary>
		/// <returns></returns>
		[HttpGet("fuel")]
		[Produces("application/json")]
		[ProducesResponseType((typeof(List<FuelHistoryResponse>)), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Fuel([FromQuery] LocoHistoryRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.LocoHistoryRequest>((request, LocoHistoryType.Fuel));

				var mtResponse = await _locoHistoryRequest.GetResponse<Contracts.Responses.FuelHistoryResponse>(mtRequest);

				var response = _mapper.Map<List<FuelHistoryResponse>>(mtResponse.Message.Items);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получить историю уведомлений о превышении скорости
		/// </summary>
		/// <returns></returns>
		[HttpGet("notification")]
		[Produces("application/json")]
		[ProducesResponseType((typeof(List<NotificationHistoryResponse>)), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Notification([FromQuery] LocoHistoryRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.LocoHistoryRequest>((request, LocoHistoryType.Notification));

				var mtResponse = await _locoHistoryRequest.GetResponse<Contracts.Responses.NotificationHistoryResponse>(mtRequest);

				var response = _mapper.Map<List<NotificationHistoryResponse>>(mtResponse.Message.Items);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получить историю скорости
		/// </summary>
		/// <returns></returns>
		[HttpGet("speed")]
		[Produces("application/json")]
		[ProducesResponseType((typeof(List<CoordinatesHistoryResponse>)), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Speed([FromQuery] LocoHistoryRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.LocoHistoryRequest>((request, LocoHistoryType.Coordinates));

				var mtResponse = await _locoHistoryRequest.GetResponse<Contracts.Responses.CoordinatesHistoryResponse>(mtRequest);

				var response = _mapper.Map<List<SpeedHistoryResponse>>(mtResponse.Message.Items);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получить список статических объектов
		/// </summary>
		/// <returns></returns>
		[HttpGet("static")]
		[Produces("application/json")]
		[ProducesResponseType((typeof(List<StaticObjectResponse>)), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> StaticObjects()
		{
			try
			{
				var mtResponse = await _staticObjectListRequest.GetResponse<StaticObjectListResponse>(new StaticObjectListRequest());

				var response = _mapper.Map<List<StaticObjectResponse>>(mtResponse.Message.Items);
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
