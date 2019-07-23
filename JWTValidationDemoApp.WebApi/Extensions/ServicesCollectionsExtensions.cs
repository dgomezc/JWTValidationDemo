using JWTValidationDemoApp.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesCollectionsExtensions
    {
        public static IServiceCollection ProtectWebApiWithJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new AuthenticationSettings();
            configuration.GetSection("AuthenticationSettings").Bind(settings);

            var tenantID = settings.TenantId;
            var audience = settings.Audience;
            var authority = $"https://login.windows.net/{tenantID}";
            var issuer = $"https://sts.windows.net/{tenantID}/";
            var scope = settings.Scope;

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = authority;
                    options.Audience = audience;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            // TODO:
                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = context =>
                        {
                            // TODO:
                            return Task.CompletedTask;
                        }
                    };
                });

            // Add Authorization with claim policy
            services.AddAuthorization(config =>
            {
                config.AddPolicy("SampleClaimPolicy", policy => policy.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", scope));
            });

            return services;
        }
    }
}
