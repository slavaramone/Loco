using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLib.Filters;
using SharedLib.Security;
using System;
using System.Threading.Tasks;

namespace Management.Ui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TokenAuth]
    public class ShunterController : ClaimsController
    {
		private readonly ILogger<ArchiveController> _logger;
		private readonly IRequestClient<ShunterListRequest> _shunterListRequest;

		public ShunterController(ILogger<ArchiveController> logger, IClaimsAccessor claimsAccessor, IRequestClient<ShunterListRequest> shunterListRequest)
			: base(claimsAccessor)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_shunterListRequest = shunterListRequest;
		}

		/// <summary>
		/// Получение списка с инфой о составителях
		/// </summary>
		/// <returns></returns>
		[HttpGet("list")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(ShunterListResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> List()
		{
			try
			{
				var response = await _shunterListRequest.GetResponse<ShunterListResponse>(new ShunterListRequest());
				return Ok(response.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
