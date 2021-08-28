using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Play.Catalog.Entities;
using Play.Catalog.Services;
using Play.Common.Identity;
using Play.Common.MassTransit;
using Play.Common.MongoDB;
using Play.Common.Settings;

namespace Play.Catalog
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private const string AllowedOriginSetting = "AllowedOrigin";

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
           var serviceSettinngs = _config.GetSection(nameof(ServiceSettings))
                                    .Get<ServiceSettings>();

             services.AddMongo().AddMongoRepository<Item>("Items")
                                .AddMassTransitWithRabbitMQ()
                                .AddJewtBearAuthentication();

            services.AddControllers(opts =>
            {
                opts.SuppressAsyncSuffixInActionNames = false;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Play.Catalog", Version = "v1" });
            });
            services.AddScoped<ICatalogItemService, CatalogItemService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play.Catalog v1"));
                app.UseCors(builder =>
                {
                    builder.WithOrigins(_config[AllowedOriginSetting])
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


// dotnet pack -p:PackageVersion=1.0.1 -o ..\packages\  make sure to point where you're package folder at