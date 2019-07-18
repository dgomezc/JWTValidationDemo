using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace JWTValidationDemoApp.WebApi.Extensions
{
    public class JwtManualAuthHandler : AuthenticationHandler<JwtManualAuthOptions>
    {
        public JwtManualAuthHandler(IOptionsMonitor<JwtManualAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = GetHeaderToken();
            JwtValidator validator = new JwtValidator(token, Options.TenantId, Options.Audience);

            try
            {
                var claims = await validator.Verify();

                if (claims is null)
                {
                    return AuthenticateResult.Fail("Invalid auth key.");
                }

                // TODO: Review this return result
                var ticket = new AuthenticationTicket(claims, Options.Scheme);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }

        private string GetHeaderToken()
        {
            string authorization = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorization))
            {
                return string.Empty;
            }                

            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return authorization.Substring("Bearer ".Length).Trim();
            }

            return authorization;
        }
    }
}
