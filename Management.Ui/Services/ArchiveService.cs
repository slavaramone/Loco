using Contracts.Requests;
using Management.Ui.Controllers;
using Management.Ui.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Management.Ui.Services
{
	public class ArchiveService : IArchiveService
	{
		private const string DateTimeStringExact = "yyyyMMddHHmmss";

		private readonly VideoOptions _videoOptions;
		private readonly Regex _fileRegex;
		private readonly ILogger<ArchiveService> _logger;

		public ArchiveService(IOptions<VideoOptions> videoOptions, ILogger<ArchiveService> logger)
		{
			_videoOptions = videoOptions.Value ?? throw new ArgumentNullException(nameof(videoOptions.Value));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			string searchRegex = _videoOptions.SearchRegex + "\\." + _videoOptions.SearchPattern.Replace("*.", string.Empty);
			_fileRegex = new Regex(searchRegex, RegexOptions.Compiled | RegexOptions.Singleline);
		}

		public string GetVideoFileDownloadUrl(string fileName)
		{
			var url = string.Empty;
			var match = _fileRegex.Match(fileName);
			if (match.Success)
            {
				url = string.Format(_videoOptions.DownloadLinkUrlFormat, match.Groups["nucNumber"].Value, match.Groups["camNumber"].Value, fileName.Substring(6, 2), fileName);
			}
			return url;
		}

		public string GetVideoFileViewUrl(string fileName)
		{
			return GetVideoFileDownloadUrl(fileName);
		}

		public VideoListFileNameResult GetVideoListFileNames(string[] nucNumbers, string[] cameraNumbers, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int? skip, int? take, 
			SortFilterContract[] sort)
		{			
			var allFiles = Directory.GetFiles(_videoOptions.VideoArchiveDir, _videoOptions.SearchPattern, SearchOption.AllDirectories);

			_logger.LogInformation($"Total video files {allFiles.Count()} with search pattern {_videoOptions.SearchPattern} in dir {_videoOptions.VideoArchiveDir}");

			var allVideos = (from file in allFiles
				let match = _fileRegex.Match(file)
				let startDateTime = ParseDateTimeFromDateAndTime(match.Groups["dateFrom"].Value, match.Groups["timeFrom"].Value)
				let endDateTime = ParseDateTimeFromDateAndTime(match.Groups["dateTo"].Value, match.Groups["timeTo"].Value)
				where match.Success
				select new VideoFileRegexResult
				{
					StartDateTime = startDateTime,
					EndDateTime = endDateTime ?? startDateTime.Value.AddSeconds(_videoOptions.DurationSeconds),
					Nuc = match.Groups["nucNumber"].Value,
					Cam = match.Groups["camNumber"].Value,
					FileName = file
				})
				.AsQueryable();

			allVideos = allVideos.OrderBy(x => x.StartDateTime);

			_logger.LogInformation($"Videos count after linq query {allVideos.Count()}");
			_logger.LogInformation($"Total nuc's {nucNumbers?.Count() ?? 0}");
			_logger.LogInformation($"Total cam's {cameraNumbers?.Count() ?? 0}");

			if (nucNumbers?.Any() ?? false)
			{
				allVideos = allVideos.Where(x => nucNumbers.Contains(x.Nuc, StringComparer.InvariantCultureIgnoreCase));
			}
			else
			{
				return new VideoListFileNameResult();
			}

			if (cameraNumbers?.Any() ?? false)
			{
				allVideos = allVideos.Where(x => cameraNumbers.Contains(x.Cam, StringComparer.InvariantCultureIgnoreCase));
			}
			else
			{
				return new VideoListFileNameResult();
			}

			var videosList = allVideos.ToList();

			_logger.LogInformation($"Total video files {videosList.Count()} after filtering");

			VideoFileRegexResultComparer comparer = new VideoFileRegexResultComparer();
			if (dateFrom.HasValue)
			{
				var firstVideo = allVideos.FirstOrDefault(x => x.StartDateTime > dateFrom);
				int index = videosList.FindIndex(x => comparer.Equals(firstVideo, x));
				if (index < 0)
                {
					return new VideoListFileNameResult();
				}
				videosList = videosList.Skip(index > 0 ? index : 0).ToList();
			}

			if (dateTo.HasValue)
			{
				var lastVideo = allVideos.LastOrDefault(x => x.StartDateTime < dateTo);
				int index = videosList.FindIndex(x => comparer.Equals(lastVideo, x));
				videosList = videosList.Take(index > 0 ? index + 1 : 0).ToList();
			}

			int total = videosList.Count;

			if (sort?.Length > 0)
			{
				videosList = SortVideoListItemList(videosList, sort);
			}

			if (skip.HasValue)
            {
				videosList = videosList.Skip(skip.Value).ToList();
			}

			if (take.HasValue)
			{
				videosList = videosList.Take(take.Value).ToList();
			}

			return new VideoListFileNameResult
			{
				FileNames = videosList.Select(x => x.FileName).ToList(),
				Total = total
			};
		}

		public List<VideoFileRegexResult> SortVideoListItemList(List<VideoFileRegexResult> videoList, SortFilterContract[] sort)
        {
			var query = videoList.AsQueryable();
			var firstSort = sort[0];

			var orderedQuery = OrderBy(query, firstSort);

			foreach (var sortItem in sort.Skip(1))
				orderedQuery = ThenBy(orderedQuery, sortItem);

			return orderedQuery.ToList();
		}

		public static DateTime GetFileDateTime(string filePath, DateTime? providedDate)
		{
			string fileTime = Path.GetFileName(filePath).Substring(9, 6);
			int fileHours = int.Parse(fileTime.Substring(0, 2));
			int fileMinutes = int.Parse(fileTime.Substring(2, 2));
			int fileSeconds = int.Parse(fileTime.Substring(4, 2));

			if (providedDate != null)
			{
                return new DateTime(providedDate.Value.Year, providedDate.Value.Month, providedDate.Value.Day, fileHours, fileMinutes, fileSeconds);
			}

			string fileDate = Path.GetFileName(filePath).Substring(0, 8);
			int fileYears = int.Parse(fileDate.Substring(0, 4));
			int fileMonths = int.Parse(fileDate.Substring(4, 2));
			int fileDays = int.Parse(fileDate.Substring(6, 2));

			return new DateTime(fileYears, fileMonths, fileDays, fileHours, fileMinutes, fileSeconds);
		}

		public static DateTime? ParseDateTimeFromDateAndTime(string date, string time)
		{
			if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(time))
			{
				return null;
			}
			return DateTime.ParseExact(date + time, DateTimeStringExact, CultureInfo.InvariantCulture);
		}

		private IOrderedQueryable<VideoFileRegexResult> OrderBy(IQueryable<VideoFileRegexResult> query, SortFilterContract sort)
		{
			IOrderedQueryable<VideoFileRegexResult> orderedQuery;

			if (sort.By.Equals("startDateTime", StringComparison.OrdinalIgnoreCase))
				orderedQuery = sort.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
					? query.OrderByDescending(e => e.StartDateTime)
					: query.OrderBy(e => e.StartDateTime);
			else if (sort.By.Equals("endDateTime", StringComparison.OrdinalIgnoreCase))
				orderedQuery = sort.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
					? query.OrderByDescending(e => e.EndDateTime)
					: query.OrderBy(e => e.EndDateTime);
			else
				orderedQuery = query.OrderBy(e => e);

			return orderedQuery;
		}

		private IOrderedQueryable<VideoFileRegexResult> ThenBy(IOrderedQueryable<VideoFileRegexResult> query, SortFilterContract sort)
		{
			if (sort.By.Equals("startDateTime", StringComparison.OrdinalIgnoreCase))
				query = sort.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
					? query.ThenByDescending(e => e.StartDateTime)
					: query.ThenBy(e => e.StartDateTime);
			else if (sort.By.Equals("endDateTime", StringComparison.OrdinalIgnoreCase))
				query = sort.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)
					? query.ThenByDescending(e => e.EndDateTime)
					: query.ThenBy(e => e.EndDateTime);

			return query;
		}
	}
}
