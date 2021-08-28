using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Play.Common.Settings;

namespace Play.Common.Identity
{
    public class ConfigureJwtBearOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly IConfiguration _config;

        public ConfigureJwtBearOptions(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            if (name != JwtBearerDefaults.AuthenticationScheme) return;
            var serviceSettinngs = _config.GetSection(nameof(ServiceSettings))
                                         .Get<ServiceSettings>();

            options.Authority = serviceSettinngs.Authority;
            options.Audience = serviceSettinngs.ServiceName;

            // help to be able with other claims stardstand
            options.MapInboundClaims = false;
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(options.ForwardDefault, options);
        }
    }
}
