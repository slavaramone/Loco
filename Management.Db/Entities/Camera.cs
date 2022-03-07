using Contracts.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Db.Entities
{
    [Table("Cameras")]
    public class Camera
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreationDateTimeUtc { get; set; }

        public int Position { get; set; }

        public string NucNumber { get; set; }

        public string Number { get; set; }

        public Guid? LocoId { get; set; }

        public Loco Loco { get; set; }
    }
}
