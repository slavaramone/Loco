using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SharedLib.Security
{
    public sealed class ClaimsAccessor : IClaimsAccessor
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsAccessor(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public bool TryGetValue(string type, out string value)
        {
            var claim = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(c => c.Type == type);

            value = claim?.Value;

            return claim != null;
        }
    }
}
