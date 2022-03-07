namespace SharedLib.Protocols.Azimuth
{
    public class AzimuthProtocolMessage
    {
        public AzimuthProtocolMessageType Type { get; set; }

        public string TrackerId { get; set; }

        public object Payload { get; set; }
    }
}
