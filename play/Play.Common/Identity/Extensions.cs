using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Identity
{
    public static class Extensions
    {
        public static AuthenticationBuilder 
            AddJewtBearAuthentication(this IServiceCollection services)
        {
            return services.ConfigureOptions<ConfigureJwtBearOptions>()
                  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer();
            ;
        }
   }
}
