using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLib.Exceptions;
using SharedLib.Options;
using SharedLib.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contracts.Exceptions;

namespace Management.Ui.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ILogger<AuthController> _logger;
		private readonly IMapper _mapper;
		private readonly JwtFactory _jwtFactory;
		private readonly AuthOptions _authOptions;
		private readonly IRequestClient<Contracts.Requests.AuthRequest> _authRequestclient;


		public AuthController(ILogger<AuthController> logger, IMapper mapper, JwtFactory jwtFactory, IOptions<AuthOptions> authOptions,
			IRequestClient<Contracts.Requests.AuthRequest> authRequestclient)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_jwtFactory = jwtFactory ?? throw new ArgumentNullException(nameof(jwtFactory));
			_authOptions = authOptions.Value ?? throw new ArgumentNullException(nameof(authOptions.Value));
			_authRequestclient = authRequestclient ?? throw new ArgumentNullException(nameof(authRequestclient));
		}

		/// <summary>
		/// Получение токена авторизации
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("token")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Token([FromBody] AuthRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.AuthRequest>(request);

				var mtResponse = await _authRequestclient.GetResponse<Contracts.Responses.AuthResponse>(mtRequest);

				var userIdClaim = new Claim(ClientApiSecurity.Claims.UserId, mtResponse.Message.UserId.ToString());

				var userRolesClaim = new Claim(ClientApiSecurity.Claims.UserRoles, string.Join(",", mtResponse.Message.UserRoles));

				string token = _jwtFactory.Create(_authOptions.Key, userIdClaim, userRolesClaim);

				var response = new AuthResponse
				{
					Token = token,
					ExpirationDateTime = DateTime.Now + _authOptions.Lifetime.Value
				};

				_logger.LogInformation($"Auth token issued: UserId={mtResponse.Message.UserId}");

				return Ok(response);
			}
			catch (RequestFaultException ex) when (ex.Fault.Exceptions.First().ExceptionType == typeof(UserNotFoundException).FullName)
			{
				_logger.LogError(ex.Message);

				return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}