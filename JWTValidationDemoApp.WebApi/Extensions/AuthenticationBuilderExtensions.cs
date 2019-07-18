using JWTValidationDemoApp.WebApi.Extensions;
using System;

namespace Microsoft.AspNetCore.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddJwtManualAuthAuth(this AuthenticationBuilder builder, Action<JwtManualAuthOptions> configureOptions)
        {
            // Add custom authentication scheme with custom options and custom handler
            return builder.AddScheme<JwtManualAuthOptions, JwtManualAuthHandler>(JwtManualAuthOptions.DefaultScheme, configureOptions);
        }
    }
}
