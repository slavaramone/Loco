using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Db.Entities
{
    [Table("Locos")]
    public class Loco
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreationDateTimeUtc { get; set; }

        public Guid? MapItemId { get; set; }

		public bool IsActive { get; set; }

        public List<Camera> Cameras { get; set; }

        public List<SensorGroup> SensorGroups { get; set; }

        public List<LocoApiKey> LocoApiKeys { get; set; }

        public List<LocoVideoStream> LocoVideoStreams { get; set; }
    }
}