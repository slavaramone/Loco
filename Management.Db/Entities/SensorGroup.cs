using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Db.Entities
{
    [Table("SensorGroups")]
    public class SensorGroup
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreationDateTimeUtc { get; set; }

        public bool IsTakeAverageValue { get; set; }

        public Guid LocoId { get; set; }

        public Loco Loco { get; set; }

        public List<Sensor> Sensors { get; set; }
    }
}
