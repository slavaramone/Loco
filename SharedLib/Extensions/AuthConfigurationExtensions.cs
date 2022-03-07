using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Options;
using SharedLib.Security;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.Extensions
{
    public static class AuthConfigurationExtensions
    {
        public const string AccessTokenParamName = "access_token";

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthorization(options =>
                {
                    var containsDebtorToken = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                        .RequireClaim(ClientApiSecurity.Claims.UserId)
                        .RequireClaim(ClientApiSecurity.Claims.UserRoles)
                        .Build();

                    options.AddPolicy(ClientApiSecurity.Policies.ContainsToken, containsDebtorToken);
                })
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var authOptions = new AuthOptions();
                    configuration.GetSection("Auth").Bind(authOptions);

                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authOptions.Key)),
                        ValidateIssuerSigningKey = true,

                        ValidateLifetime = authOptions.Lifetime.HasValue,
                        RequireExpirationTime = true,
                        RequireSignedTokens = true,

                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Query.TryGetValue(AccessTokenParamName, out var values))
                                context.Token = values.First();

                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
