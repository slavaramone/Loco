using System;

namespace SharedLib.Protocols.Azimuth
{
    public class AzimuthLocationReportPayload
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double? Altitude { get; set; }

        public double? Speed { get; set; }

        public double? Heading { get; set; }

        public DateTime ReportDate { get; set; }
    }
}
