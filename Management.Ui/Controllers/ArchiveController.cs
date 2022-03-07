using AutoMapper;
using Contracts.Requests;
using Contracts.Responses;
using Management.Ui.Options;
using Management.Ui.Services;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLib.Filters;
using SharedLib.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Management.Ui.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[TokenAuth]
	public class ArchiveController : ClaimsController
	{
		private readonly IArchiveService _archiveService;
		private readonly IMapper _mapper;
		private readonly ILogger<ArchiveController> _logger;
		private readonly IRequestClient<LocoInfosRequest> _locoInfosClient;
		private readonly VideoOptions _videoOptions;
		private readonly Regex _fileRegex;

		public ArchiveController(IArchiveService archiveService, IMapper mapper, ILogger<ArchiveController> logger, IRequestClient<LocoInfosRequest> locoInfosClient, IClaimsAccessor claimsAccessor,
			IOptions<VideoOptions> videoOptions) : base(claimsAccessor)
		{
			_archiveService = archiveService ?? throw new ArgumentNullException(nameof(archiveService));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_locoInfosClient = locoInfosClient ?? throw new ArgumentNullException(nameof(locoInfosClient));
			_videoOptions = videoOptions.Value ?? throw new ArgumentNullException(nameof(videoOptions.Value));

			string searchRegex = _videoOptions.SearchRegex + "\\." + _videoOptions.SearchPattern.Replace("*.", string.Empty);
			_fileRegex = new Regex(searchRegex, RegexOptions.Compiled | RegexOptions.Singleline);
		}

		/// <summary>
		/// Получить ссылку на архивный видео файл
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet("video/{name}")]
		[Produces("application/json")]
		[ProducesResponseType((typeof(VideoLinkResponse)), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult VideoFromArchive([FromRoute] string name)
		{
			try
			{
				var response = new VideoLinkResponse
				{
					DownloadUrl = _archiveService.GetVideoFileDownloadUrl(name),
					ViewUrl = _archiveService.GetVideoFileViewUrl(name)

				};
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		/// <summary>
		/// Список инфо о видео файлах по фильтру
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet("video/list")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(VideoListResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> VideoList([FromQuery] VideoListRequest request)
		{
			try
			{
				var locoInfosResponse = await _locoInfosClient.GetResponse<LocoInfosResponse>(new LocoInfosRequest());

				var locos = locoInfosResponse.Message.Locos;
				if (request.LocoIds.Any())
				{
					locos = locos.Where(x => request.LocoIds.Contains(x.Id))
						.ToList();
				}
				string[] nucNumbers = GetNucNumbers(locos);
				string[] cameraNumbers = GetCameraNumbers(locos, request);

				var sort = _mapper.Map<SortFilterContract[]>(request.Sort);

				var result = _archiveService.GetVideoListFileNames(nucNumbers, cameraNumbers, request.DateTimeFrom, request.DateTimeTo, request.Skip, request.Take, sort);

				var videoList = GetVideoListItems(locoInfosResponse.Message, result.FileNames);

				var response = new VideoListResponse
				{
					VideoList = videoList,
					Total = result.Total
				};
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		private List<VideoListItem> GetVideoListItems(LocoInfosResponse locoInfosResponse, List<string> videoFileNames)
        {
			var videoList = new List<VideoListItem>();
			var fileRegex = new Regex(_videoOptions.SearchRegex, RegexOptions.Compiled | RegexOptions.Singleline);
			var locosCameras = locoInfosResponse.Locos.SelectMany(x => x.Cameras)
				.ToList();

			foreach (var videoFileName in videoFileNames)
			{
				var match = fileRegex.Match(videoFileName);

				var camera = locosCameras.Find(x => x.NucNumber.Equals(match.Groups["nucNumber"].Value) && x.Number.Equals(match.Groups["camNumber"].Value));
				var loco = locoInfosResponse.Locos.Find(x => x.Cameras.Contains(camera));

				var startDateTime = ArchiveService
					.ParseDateTimeFromDateAndTime(match.Groups["dateFrom"].Value, match.Groups["timeFrom"].Value).Value;
				var endDateTime = ArchiveService.ParseDateTimeFromDateAndTime(match.Groups["dateTo"].Value, match.Groups["timeTo"].Value) ??
								startDateTime.AddSeconds(_videoOptions.DurationSeconds);
				
				videoList.Add(new VideoListItem
				{
					LocoId = loco.Id,
					CameraPosition = camera.Position,
					Url = _archiveService.GetVideoFileDownloadUrl(Path.GetFileName(videoFileName)),
					StartDateTime = startDateTime,
					EndDateTime = endDateTime,
				});
			}
			return videoList;
        }

		private string[] GetNucNumbers(List<LocoContract> locos)
        {
			var nucNumbers = new List<string>();

			foreach (var loco in locos)
            {
				var locoNucNumbers = loco.Cameras.Select(x => x.NucNumber).ToList();
				nucNumbers.AddRange(locoNucNumbers);
			}
			return nucNumbers.Distinct().ToArray();
        }

		private string[] GetCameraNumbers(List<LocoContract> locos, VideoListRequest request)
		{
			var cameraNumbers = new List<string>();
			foreach (var loco in locos)
			{
				var cameras = loco.Cameras;
				if (request.CameraPositions.Any())
                {
					cameras = cameras.FindAll(x => request.CameraPositions.Contains(x.Position));
                }

				var locoCameraNumbers = cameras.Select(x => x.Number)
					.ToList();
				cameraNumbers.AddRange(locoCameraNumbers);
			}
			return cameraNumbers.ToArray();
		}
	}
}
