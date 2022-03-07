using System;

namespace Management.Ui.Models
{
    public class FuelLevelDataModel
    {
        public Guid LocoId { get; set; }

        public double CurrentValue { get; set; }

        public DateTimeOffset ReportDateTime { get; set; }
    }
}
