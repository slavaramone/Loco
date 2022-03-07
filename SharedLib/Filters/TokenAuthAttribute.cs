using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SharedLib.Security;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLib.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TokenAuthAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _role;

        public TokenAuthAttribute()
        {
            _role = string.Empty;
        }

        public TokenAuthAttribute(string role)
        {
            _role = role;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsAccessor = context.HttpContext.RequestServices.GetRequiredService<IClaimsAccessor>();

            if (!claimsAccessor.TryGetValue(ClientApiSecurity.Claims.UserId, out string userId) ||
                !claimsAccessor.TryGetValue(ClientApiSecurity.Claims.UserRoles, out string userRolesStr))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRoles = userRolesStr.Split(',');
            if (!string.IsNullOrEmpty(_role) && !userRoles.Contains(_role))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
