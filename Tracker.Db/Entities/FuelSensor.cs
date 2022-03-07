using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Db.Entities
{
	[Table("FuelSensors")]
    public class FuelSensor
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string TrackerId { get; set; }

        public List<FuelSensorRawData> FuelSensorRawData { get; set; }
    }
}
