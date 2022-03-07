using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Db.Entities
{
    [Table("Shunters")]
    public class Shunter
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreationDateTimeUtc { get; set; }

        public Guid? MapItemId { get; set; }
    }
}
