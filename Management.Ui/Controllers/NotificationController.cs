using AutoMapper;
using Contracts.Responses;
using Management.Ui.Services;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLib.Converters;
using SharedLib.Filters;
using SharedLib.Security;
using System;
using System.Threading.Tasks;

namespace Management.Ui.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    [TokenAuth]
    public class NotificationController : ClaimsController
    {
		private readonly IExcelService _excelService;
		private readonly IMapper _mapper;
		private readonly ILogger<NotificationController> _logger;
		private readonly IRequestClient<Contracts.Requests.NotificationListRequest> _notificationListRequest;

		public NotificationController(IMapper mapper, ILogger<NotificationController> logger, IClaimsAccessor claimsAccessor,
			IRequestClient<Contracts.Requests.NotificationListRequest> notificationListRequest, IExcelService excelService)
			: base(claimsAccessor)
		{
			_excelService = excelService;
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_notificationListRequest = notificationListRequest ?? throw new ArgumentNullException(nameof(notificationListRequest));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		/// <summary>
		/// Получение списка с инфой об уведомлениях по фильтру
		/// </summary>
		/// <returns></returns>
		[HttpGet("list")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(NotificationListResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> List([FromQuery] NotificationListRequest request)
		{
			try
			{
                var mtRequest = _mapper.Map<Contracts.Requests.NotificationListRequest>(request);
                var response = await _notificationListRequest.GetResponse<NotificationListResponse>(mtRequest);
                return Ok(response.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получение xlsx файла с инфой об уведомлениях по фильтру
		/// </summary>
		/// <returns></returns>
		[HttpGet("file")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> File([FromQuery] NotificationListRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.NotificationListRequest>(request);
				var mtResponse = await _notificationListRequest.GetResponse<NotificationListResponse>(mtRequest);

				byte[] fileBytes = _excelService.CreateNotificationReport(mtResponse.Message.NotificationList);
				var ms = BytesToMemStreamConverter.Convert(fileBytes);

				return File(ms, LocoController.ExcelFileContentType, "отчет по событиям.xlsx");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
