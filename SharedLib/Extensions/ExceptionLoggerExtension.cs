using GreenPipes;
using SharedLib.Filters;

namespace SharedLib.Extensions
{
    public static class ExceptionLoggerExtension
    {
        public static void UseExceptionLogger<T>(this IPipeConfigurator<T> configurator) where T : class, PipeContext
        {
            configurator.AddPipeSpecification(new ExceptionLoggerSpecification<T>());
        }
    }
}
