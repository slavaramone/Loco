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
    [TokenAuth("SuperAdmin")]
	public class SuperAdminController : ClaimsController
    {
		private readonly IMapper _mapper;
		private readonly ILogger<SuperAdminController> _logger;
		private readonly IRequestClient<Contracts.Requests.AddUserRequest> _addUserRequest;
		private readonly IRequestClient<Contracts.Requests.DeleteUserRequest> _deleteUserRequest;
		private readonly IRequestClient<Contracts.Requests.UpdateUserRequest> _updateUserRequest;
		private readonly IRequestClient<Contracts.Requests.UserListRequest> _userListRequest;

		public SuperAdminController(IMapper mapper, ILogger<SuperAdminController> logger, IClaimsAccessor claimsAccessor, IRequestClient<Contracts.Requests.AddUserRequest> addUserRequest,
			IRequestClient<Contracts.Requests.DeleteUserRequest> deleteUserRequest, IRequestClient<Contracts.Requests.UpdateUserRequest> updateUserRequest, 
			IRequestClient<Contracts.Requests.UserListRequest> userListRequest)
			: base(claimsAccessor)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_addUserRequest = addUserRequest ?? throw new ArgumentNullException(nameof(addUserRequest));
			_deleteUserRequest = deleteUserRequest ?? throw new ArgumentNullException(nameof(deleteUserRequest));
			_updateUserRequest = updateUserRequest ?? throw new ArgumentNullException(nameof(updateUserRequest));
			_userListRequest = userListRequest ?? throw new ArgumentNullException(nameof(userListRequest));

		}

		/// <summary>
		/// Получение списка с инфой о локо
		/// </summary>
		/// <returns></returns>
		[HttpGet("users")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<UserListResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UserList()
		{
			try
			{
				var mtResponse = await _userListRequest.GetResponse<Contracts.Responses.UserListResponse>(new Contracts.Requests.UserListRequest());
				var response = _mapper.Map<List<UserListResponse>>(mtResponse.Message.Users);
				
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Добавление пользователя
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("add/user")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(AddUserResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.AddUserRequest>(request);
				var mtResponse = await _addUserRequest.GetResponse<Contracts.Responses.AddUserResponse>(mtRequest);

				var response = _mapper.Map<AddUserResponse>(mtResponse.Message);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Добавление пользователя
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpDelete("user")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(AddUserResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.DeleteUserRequest>(request);
				var mtResponse = await _deleteUserRequest.GetResponse<Contracts.Responses.DeleteUserResponse>(request);

				var response = _mapper.Map<DeleteUserResponse>(mtResponse.Message);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Обновление пароля и активация/деактивация пользователя
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPut("user")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(List<UpdateUserResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
		{
			try
			{
				var mtRequest = _mapper.Map<Contracts.Requests.UpdateUserRequest>(request);
				var mtResponse = await _updateUserRequest.GetResponse<Contracts.Responses.UpdateUserResponse>(mtRequest);

				var response = _mapper.Map<UpdateUserResponse>(mtResponse.Message);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				throw;
			}
		}
	}
}
