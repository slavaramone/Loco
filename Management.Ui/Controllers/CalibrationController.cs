using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLib.Filters;
using SharedLib.Security;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Management.Ui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TokenAuth]
    public class CalibrationController : ClaimsController
    {
		private readonly ILogger<CalibrationController> _logger;
		private readonly IRequestClient<UploadCalibrationRequest> _uploadCalibrationClient;
		private readonly IMapper _mapper;

		public CalibrationController(ILogger<CalibrationController> logger, IClaimsAccessor claimsAccessor, IRequestClient<UploadCalibrationRequest> uploadCalibrationClient,
			IMapper mapper) : base(claimsAccessor)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_uploadCalibrationClient = uploadCalibrationClient ?? throw new ArgumentNullException(nameof(uploadCalibrationClient));
		}

		/// <summary>
		/// Загрузка xlsx файла с откалиброванными показаниями ДУТ
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("upload")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Upload([FromForm] UploadCalibrationFileRequest request)
		{
			try
			{
				Stream calibrationFileStream = request.File.OpenReadStream();
				byte[] calibrationBytes;
				using (BinaryReader br = new BinaryReader(calibrationFileStream))
				{
					calibrationBytes = br.ReadBytes((int)calibrationFileStream.Length);
				}
				var mtRequest = _mapper.Map<UploadCalibrationRequest>((request, calibrationBytes));
				var response = await _uploadCalibrationClient.GetResponse<UploadCalibrationResponse>(mtRequest);

				return Ok(response.Message.IsCompletedSuccessfuly);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
