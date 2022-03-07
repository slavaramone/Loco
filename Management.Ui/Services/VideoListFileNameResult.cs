using System.Collections.Generic;

namespace Management.Ui.Services
{
    public class VideoListFileNameResult
    {
        public List<string> FileNames { get; set; } = new List<string>();

        public int Total { get; set; }
    }
}
