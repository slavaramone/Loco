using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Db.Entities
{
    [Table("FuelLevelCalibrations")]
    public class FuelLevelCalibration
    {
        public Guid Id { get; set; }

        public double RawValue { get; set; }

        public double CalibratedValue { get; set; }

        public Guid FuelLevelSensorId { get; set; }

        public Sensor FuelLevelSensor { get; set; }
    }
}
