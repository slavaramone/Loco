using AutoMapper;
using Contracts.Enums;
using Contracts.Requests;
using Management.Ui.Services;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLib.Converters;
using SharedLib.Filters;
using SharedLib.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Ui.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[TokenAuth]
	public class LocoController : ClaimsController
    {
		private const int MaxChartDateRangeDays = 14;

		public const string ExcelFileContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

		private readonly IMapper _mapper;
		private readonly IExcelService _excelService;
		private readonly ILogger<ArchiveController> _logger;
		private readonly IRequestClient<LocoListRequest> _locoListRequest;
		private readonly IRequestClient<LocoInfosRequest> _locoInfosClient;
		private readonly IRequestClient<LocoVideoStreamRequest> _locoVideoStreamClient;
		private readonly IRequestClient<Contracts.Requests.LocoCoordReportRequest> _locoCoordReportClient;
		private readonly IRequestClient<SensorFuelReportRequest> _sensorFuelReportClient;
		private readonly IRequestClient<FuelReportCalibrationRequest> _fuelReportCalibrationRequest;
		private readonly IRequestClient<Contracts.Requests.DateAxisChartRequest> _locoChartClient;
		private readonly IRequestClient<Contracts.Requests.UpdateLocoRequest> _updateLocoRequest;

		public LocoController(ILogger<ArchiveController> logger, IClaimsAccessor claimsAccessor, IRequestClient<LocoListRequest> locoListRequest, IRequestClient<LocoInfosRequest> locoInfosClient,
			IRequestClient<Contracts.Requests.LocoCoordReportRequest> locoCoordReportClient, IRequestClient<SensorFuelReportRequest> sensorFuelReportClient, 
			IRequestClient<FuelReportCalibrationRequest> fuelReportCalibrationRequest, IRequestClient<LocoVideoStreamRequest> locoVideoStreamClient, IExcelService excelService, IMapper mapper,
			IRequestClient<Contracts.Requests.DateAxisChartRequest> locoChartClient, IRequestClient<Contracts.Requests.UpdateLocoRequest> updateLocoRequest)
			: base(claimsAccessor)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_locoListRequest = locoListRequest ?? throw new ArgumentNullException(nameof(locoListRequest));
			_locoInfosClient = locoInfosClient ?? throw new ArgumentNullException(nameof(locoInfosClient));
			_locoCoordReportClient = locoCoordReportClient ?? throw new ArgumentNullException(nameof(locoCoordReportClient));
			_sensorFuelReportClient = sensorFuelReportClient ?? throw new ArgumentNullException(nameof(sensorFuelReportClient));
			_fuelReportCalibrationRequest = fuelReportCalibrationRequest ?? throw new ArgumentNullException(nameof(fuelReportCalibrationRequest));
			_locoVideoStreamClient = locoVideoStreamClient ?? throw new ArgumentNullException(nameof(locoVideoStreamClient));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_locoChartClient = locoChartClient ?? throw new ArgumentNullException(nameof(locoChartClient));
			_updateLocoRequest = updateLocoRequest ?? throw new ArgumentNullException(nameof(updateLocoRequest));
		}

		/// <summary>
		/// Получение списка с инфой о локо
		/// </summary>
		/// <param name="isOnlyActive">Возврат толькр активных локо</param>
		/// <returns></returns>
		[HttpGet("list")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<LocoListItem>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> List([FromQuery] bool isOnlyActive = false)
		{
			try
			{
				var mtResponse = await _locoListRequest.GetResponse<Contracts.Responses.LocoListResponse>(new LocoListRequest
				{
					IsOnlyActive = isOnlyActive
				});

				var response = _mapper.Map<List<LocoListItem>>(mtResponse.Message.LocoList);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получение камер на локомотиве
		/// </summary>
		/// <param name="locoId"></param>
		/// <returns></returns>
		[HttpGet("{locoId}/camera")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Cameras([FromRoute] Guid locoId)
		{
			try
			{
				var locoInfosResponse = await _locoInfosClient.GetResponse<Contracts.Responses.LocoInfosResponse>(new LocoInfosRequest
				{
					LocoIds = new List<Guid> { locoId }
				});

				var response = locoInfosResponse.Message.Locos[0].Cameras
					.Select(x => x.Position)
					.ToList();

				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получение ссылок на видеопотоки с камер локомотива
		/// </summary>
		/// <returns></returns>
		[HttpGet("stream/{locoId}")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<LocoVideoStreamResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> VideoStream([FromRoute] Guid locoId)
		{
			try
			{
				var mtResponse = await _locoVideoStreamClient.GetResponse<Contracts.Responses.LocoVideoStreamResponse>(new LocoVideoStreamRequest
				{
					LocoId = locoId
				});

				var response = _mapper.Map<List<LocoVideoStreamResponse>>(mtResponse.Message.VideoStreams);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получение данных для построения графика дата-скорость
		/// </summary>
		/// <param name="locoId"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet("{locoId}/chart/speed")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<DateAxisChartResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> LocoChartSpeed([FromRoute] Guid locoId, [FromQuery] DateAxisChartRequest request)
		{
			try
			{
				ValidateChartDateRange(request);

				var mtRequest = _mapper.Map<Contracts.Requests.DateAxisChartRequest>(request, opts =>
				{
					opts.Items["Type"] = ChartType.Speed;
					opts.Items["LocoId"] = locoId;
				});
				var mtResponse = await _locoChartClient.GetResponse<Contracts.Responses.DateAxisChartResponse>(mtRequest);

				var response = _mapper.Map<List<DateAxisChartResponse>>(mtResponse.Message.ChartItems);
				return Ok(response);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получение данных для построения графика дата-уровень топлива
		/// </summary>
		/// <param name="locoId"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet("{locoId}/chart/fuel")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<DateAxisChartResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> LocoChartFuel([FromRoute] Guid locoId, [FromQuery] DateAxisChartRequest request)
		{
			try
			{
				ValidateChartDateRange(request);

				var mtRequest = _mapper.Map<Contracts.Requests.DateAxisChartRequest>(request, opts =>
				{
					opts.Items["Type"] = ChartType.Fuel;
					opts.Items["LocoId"] = locoId;
				});
				var mtResponse = await _locoChartClient.GetResponse<Contracts.Responses.DateAxisChartResponse>(mtRequest);

				var response = _mapper.Map<List<DateAxisChartResponse>>(mtResponse.Message.ChartItems);
				return Ok(response);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получение отчета коорд лококотивам (данные отсортированы по дате desc)
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet("report/coord")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<LocoCoordReportResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ReportCoord([FromQuery] LocoCoordReportRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.LocoCoordReportRequest>(request);
				var mtResponse = await _locoCoordReportClient.GetResponse<Contracts.Responses.LocoCoordReportResponse>(request);

				var response = _mapper.Map<List<LocoCoordReportResponse>>(mtResponse.Message.CoordItems);
				return Ok(response);
				
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получение отчета коорд локомотивам в формате Excel (данные отсортированы по дате desc)
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet("report/coord/file")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ReportCoordFile([FromQuery] LocoCoordReportRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.LocoCoordReportRequest>(request);
				var mtResponse = await _locoCoordReportClient.GetResponse<Contracts.Responses.LocoCoordReportResponse>(request);

				byte[] fileBytes = _excelService.CreateCoordReport(mtResponse.Message.CoordItems);
				var ms = BytesToMemStreamConverter.Convert(fileBytes);

				return File(ms, ExcelFileContentType, "отчет по координатам.xlsx");

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получение отчета ДУТ локомотивов (данные отсортированы по дате asc)
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet("report/fuel")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<LocoReportFuelItemResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ReportFuel([FromQuery] LocoFuelReportRequest request)
		{
			try
            {
                var fuelReportCalibration = await GetFuelReportCalibrationResponse(request);

                var response = _mapper.Map<List<LocoReportFuelItemResponse>>(fuelReportCalibration.CalibratedFuelItems);
                return Ok(response);
            }
            catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Получение отчета ДУТ локомотивов в формате Excel (данные отсортированы по дате asc)
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet("report/fuel/file")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<LocoReportFuelItemResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ReportFuelFile([FromQuery] LocoFuelReportRequest request)
		{
			try
			{
				var fuelReportCalibration = await GetFuelReportCalibrationResponse(request);

				byte[] fileBytes = _excelService.CreateFuelReport(fuelReportCalibration.CalibratedFuelItems);
				var ms = BytesToMemStreamConverter.Convert(fileBytes);

				return File(ms, ExcelFileContentType, "отчет показаний ДУТ.xlsx");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Обновление названия и активности локо
		/// </summary>
		/// <param name="locoId"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPut("{locoId}")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(UpdateLocoResponse), StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateLoco([FromRoute] Guid locoId, [FromBody] UpdateLocoRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.UpdateLocoRequest>((request, locoId));
				var mtResponse = await _updateLocoRequest.GetResponse<Contracts.Responses.UpdateLocoResponse>(mtRequest);

				var response = _mapper.Map<UpdateLocoResponse>(mtResponse.Message);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				throw;
			}
		}

		private async Task<Contracts.Responses.FuelReportCalibrationResponse> GetFuelReportCalibrationResponse(LocoFuelReportRequest request)
        {
            var locoInfosResponse = await _locoInfosClient.GetResponse<Contracts.Responses.LocoInfosResponse>(new LocoInfosRequest
            {
                LocoIds = request.LocoIds
            });

            var fuelSensorIds = locoInfosResponse.Message.Locos.SelectMany(x => x.SensorGroups)
                .SelectMany(x => x.Sensors)
                .Select(x => x.FuelSensorId)
                .ToList();
			if (fuelSensorIds.Any())
            {
				var mtRequest = _mapper.Map<SensorFuelReportRequest>((request, fuelSensorIds));

				var mtResponse = await _sensorFuelReportClient.GetResponse<Contracts.Responses.SensorFuelReportResponse>(mtRequest);

				var calibrationRequest = new FuelReportCalibrationRequest
				{
					FuelItems = mtResponse.Message.FuelItems
				};
				var calibrationResponse = await _fuelReportCalibrationRequest.GetResponse<Contracts.Responses.FuelReportCalibrationResponse>(calibrationRequest);
				return calibrationResponse.Message;
			}

			return new Contracts.Responses.FuelReportCalibrationResponse
			{
				CalibratedFuelItems = new List<Contracts.Responses.LocoReportFuelItemContract>()
			};
		}

		private void ValidateChartDateRange(DateAxisChartRequest request)
        {
			if (request.EndDateTime - request.StartDateTime > new TimeSpan(MaxChartDateRangeDays, 0, 0, 0))
			{
				throw new Exception("Период графика должен быть меньше 2х недель");
			}
		}
    }
}
