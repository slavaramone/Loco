using System;

namespace Management.Ui.Services
{
    public class VideoFileRegexResult
    {
		public DateTime? StartDateTime { get; set; }

		public DateTime? EndDateTime { get; set; }

		public string Nuc { get; set; }

		public string Cam { get; set; }

		public string FileName { get; set; }
	}
}
