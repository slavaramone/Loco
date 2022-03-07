using GreenPipes;
using SharedLib.Filters;

namespace SharedLib.Extensions
{
    public static class CustomValidationExtension
    {
        public static void UseCustomValidation<T>(this IPipeConfigurator<T> configurator) where T : class, PipeContext
        {
            configurator.AddPipeSpecification(new CustomValidationSpecification<T>());
        }
    }
}
