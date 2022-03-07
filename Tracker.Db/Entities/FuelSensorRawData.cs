using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Db.Entities
{
    [Table("FuelSensorRawData")]
    public class FuelSensorRawData
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreationDateTime { get; set; }

        public DateTimeOffset ReportDateTime { get; set; }

        public double RawValue { get; set; }

        public Guid FuelSensorId { get; set; }

        public FuelSensor FuelSensor { get; set; }
    }
}
