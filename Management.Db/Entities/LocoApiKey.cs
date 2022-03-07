using System;

namespace Management.Db.Entities
{
    public class LocoApiKey
    {
        public Guid LocoId { get; set; }

        public Loco Loco { get; set; }

        public string ApiKey { get; set; }
    }
}
