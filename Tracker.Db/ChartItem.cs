using System;

namespace Tracker.Db
{
    public class ChartItem
    {
        public DateTimeOffset TrackDate { get; set; }

		public double Value { get; set; }

		public double MaxValue { get; set; }

		public double MinValue { get; set; }
	}
}
