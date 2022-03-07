using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Db.Entities
{
    [Table("PrecisionGeoData")]
    public class PrecisionGeoData
    {
        public Guid Id { get; set; }

        public string TrackerId { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset TrackDate { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double? Altitude { get; set; }

        public double? Speed { get; set; }

        public double? Heading { get; set; }

        public Guid RawGeoDataId { get; set; }

        public RawGeoData RawGeoData { get; set; }
    }
}