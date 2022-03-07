using System.ComponentModel;

namespace SharedLib.Protocols.Azimuth
{
    public enum AzimuthProtocolMessageType
    {
        [Description("Отчет о местоположении")]
        LocationReport = 1
    }
}
