using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace JWTValidationDemoApp.WebApi.Extensions
{
    public class JwtValidator
    {
        private readonly string _jwt;
        private readonly string _tenantId;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly string _authorityUrl;

        public JwtValidator(string token, string tenantId, string audience)
        {
            _jwt = token;
            _tenantId = tenantId;
            _audience = audience;
            _issuer = $"https://sts.windows.net/{_tenantId}/";
            _authorityUrl = $"https://login.windows.net/{_tenantId}/.well-known/openid-configuration";
        }

        public string TenantId_ByToken
        {
            get
            {
                return new JwtSecurityToken(_jwt).Claims.FirstOrDefault(x => x.Type == "tid")?.Value ?? "no tenant!!";
            }
        }

        public async Task<ClaimsPrincipal> Verify()
        {
            var validationParameter = new TokenValidationParameters()
            {
                RequireSignedTokens = true,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidIssuer = _issuer,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKeys = await RequestSigningCertificates()
            };

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var claims = handler.ValidateToken(_jwt, validationParameter, out var token);
                return claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> RequestCertificateUrl()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_authorityUrl);
                var data = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<JObject>(data);
                return json["jwks_uri"].Value<string>();
            }
        }

        public async Task<IEnumerable<SecurityKey>> RequestSigningCertificates()
        {
            var url = await RequestCertificateUrl();
            var result = new List<SecurityKey>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var data = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<JObject>(data);
                json["keys"].Values<JObject>().ToList().ForEach(key =>
                {
                    key["x5c"].Values<string>().ToList().ForEach(cert =>
                    {
                        result.Add(ConvertToCertificate(cert));
                    });
                });
            }

            return result;
        }

        private SecurityKey ConvertToCertificate(string cert)
        {
            var c = new X509Certificate2(Convert.FromBase64String(cert));
            return new X509SecurityKey(c);
        }
    }
}
