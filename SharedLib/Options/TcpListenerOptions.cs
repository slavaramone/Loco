namespace SharedLib.Options
{
    /// <summary>
    /// Опции слушателя TCP соединения
    /// </summary>
    public class TcpListenerOptions
    {
        public const string TcpListener = "TcpListener";

        /// <summary>
        /// Порт прослушивания
        /// </summary>
        public int Port { get; set; }
    }
}
