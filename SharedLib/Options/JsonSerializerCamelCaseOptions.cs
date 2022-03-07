using System.Text.Json;

namespace SharedLib.Options
{
    /// <summary>
    /// Опция json сериализатора для сериализации в camel case
    /// </summary>
    public static class JsonSerializerCamelCaseOptions
    {
        /// <summary>
        /// Получить опцию сериализации в camel case
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerOptions Get()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            return options;
        }
    }
}
