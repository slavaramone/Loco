using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Db.Entities
{
    [Table("Sensors")]
    public class Sensor
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreationDateTimeUtc { get; set; }
        
        public Guid? FuelSensorId { get; set; }

        public Guid SensorGroupId { get; set; }

        public SensorGroup SensorGroup { get; set; }

        public List<FuelLevelCalibration> FuelLevelCalibrations { get; set; }
    }
}
