using Contracts.Enums;
using System;

namespace Management.Ui.Models
{
    public class MapItemModel
    {
		public Guid Id { get; set; }

		public MapItemType Type { get; set; }

		public string Name { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public double? Altitude { get; set; }

        public double? Speed { get; set; }

        public double? Heading { get; set; }

        public string TrackerId { get; set; }

        public DateTimeOffset TrackDateTime { get; set; }
    }
}
