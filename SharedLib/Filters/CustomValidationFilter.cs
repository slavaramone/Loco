using GreenPipes;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib.Filters
{
    public class CustomValidationFilter<T> : IFilter<T> where T : class, PipeContext
    {
        public void Probe(ProbeContext context)
        {
        }

        public async Task Send(T context, IPipe<T> next)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var payload = context.GetPayload<T>();
            var validationContext = new ValidationContext(payload.GetPayload<T>());
            if (!Validator.TryValidateObject(payload, validationContext, results, true))
            {
                foreach (var error in results)
                {
                    LogManager.GetCurrentClassLogger().Error($"Validation error: {error}");
                }
            }

            await next.Send(context);
        }
    }
}
