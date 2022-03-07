namespace SharedLib.Protocols.Azimuth
{
    public interface IAzimuthMessageParser
    {
        AzimuthProtocolMessage Parse(byte[] binaryData);
    }
}
