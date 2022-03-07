using Contracts.Requests;
using System;

namespace Management.Ui.Services
{
	public interface IArchiveService
    {
        string GetVideoFileDownloadUrl(string fileName);

        string GetVideoFileViewUrl(string fileName);

        VideoListFileNameResult GetVideoListFileNames(string[] numNumbers, string[] cameraNumbers, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int? skip, int? take,
            SortFilterContract[] sort);
    }
}
