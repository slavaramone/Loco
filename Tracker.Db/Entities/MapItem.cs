using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Db.Entities
{
    [Table("MapItems")]
    public class MapItem
    {
        public Guid Id { get; set; }

        public MapItemType Type { get; set; }

        [Required]
        public string TrackerId { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public bool IsStatic { get; set; }

        public List<RawGeoData> RawGeoData { get; set; }
    }
}
