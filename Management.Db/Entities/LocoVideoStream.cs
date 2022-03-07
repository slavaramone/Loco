using Contracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Db.Entities
{
    [Table("LocoVideoStreams")]
    public class LocoVideoStream
    {
        public Guid Id { get; set; }

        public Guid LocoId { get; set; }

        public Loco Loco { get; set; }

        public int CameraPosition { get; set; }

        [Required]
        public string Url { get; set; }

		public string UrlHd { get; set; }
	}
}
