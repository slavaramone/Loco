using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Db.Entities
{
    [Table("RawGeoData")]
    public class RawGeoData
    {
        public Guid Id { get; set; }

        public Guid MapItemId { get; set; }

        public MapItem MapItem { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset TrackDate { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double? Altitude { get; set; }

        public double? Speed { get; set; }

        public double? Heading { get; set; }

        public List<PrecisionGeoData> PrecisionGeoData { get; set; }
    }
}
