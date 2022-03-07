using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Db.Entities
{
    [Table("FuelLevels")]
    public class FuelLevel
    {
        public Guid Id { get; set; }

        public string TrackerId { get; set; }

        public double RawValue { get; set; }

        public DateTimeOffset CreationDateTime { get; set; }

        public DateTimeOffset ReportDateTime { get; set; }
    }
}
