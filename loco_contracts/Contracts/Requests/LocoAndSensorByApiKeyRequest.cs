namespace Contracts.Requests
{
    /// <summary>
    /// Запрос Id локомотива по API ключу
    /// </summary>
    public class LocoAndSensorByApiKeyRequest
    {
        /// <summary>
        /// API ключ
        /// </summary>
        public string ApiKey { get; set; }
    }
}
