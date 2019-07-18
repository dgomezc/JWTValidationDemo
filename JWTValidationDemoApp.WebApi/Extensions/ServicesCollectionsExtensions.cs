using JWTValidationDemoApp.WebApi.Extensions;
using JWTValidationDemoApp.WebApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesCollectionsExtensions
    {
        public static IServiceCollection ProtectWebApiWithDefaultJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new AuthenticationSettings();
            configuration.GetSection("AuthenticationSettings").Bind(settings);

            var tenantID = settings.TenantId;
            var audience = settings.Audience;
            var authority = $"https://login.windows.net/{tenantID}";
            var issuer = $"https://sts.windows.net/{tenantID}/";

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

                        //ValidateAudience = true,
                        //ValidAudience = audience,  //or
                        //ValidAudiences = new string[] { tenantID, audience },

                        //ValidateIssuerSigningKey = true,
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrets)),

                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                    };
                });

            return services;
        }

        public static IServiceCollection ProtectWebApiWithManualJWTValidation(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new AuthenticationSettings();
            configuration.GetSection("AuthenticationSettings").Bind(settings);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtManualAuthAuth(options => 
                {
                    options.Audience = settings.Audience;
                    options.TenantId = settings.TenantId;
                });

            return services;
        }
    }
}
