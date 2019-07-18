using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace JWTValidationDemoApp.WebApi.Extensions
{
    public class JwtManualAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        public string Scheme => DefaultScheme;

        public string Audience { get; set; }

        public string TenantId { get; set; }
    }
}
