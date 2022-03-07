using System;

namespace SharedLib.Protocols.Azimuth
{
    public class AzimuthMessageParser : IAzimuthMessageParser
    {
        public AzimuthProtocolMessage Parse(byte[] binaryData)
        {
            return new AzimuthProtocolMessage
            {
                Type = AzimuthProtocolMessageType.LocationReport,
                TrackerId = "0x3456",
                Payload = new AzimuthLocationReportPayload
                {
                    Altitude = 10,
                    Heading = 345.6,
                    Latitude = 45.66,
                    Longitude = 56.77,
                    ReportDate = DateTime.Now,
                    Speed = 13.4
                }
            };
        }
    }
}
