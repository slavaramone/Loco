namespace SharedLib.Options
{
    /// <summary>
    /// Опции брокера сообщений RabbitMQ
    /// </summary>
    public class RabbitMQOptions
    {                
        public const string RabbitMQ = "RabbitMQ";

        /// <summary>
        /// Uri брокера с инфо по подключению (пример amqp://guest:guest@localhost)
        /// </summary>
        public string Uri { get; set; }
    }
}
