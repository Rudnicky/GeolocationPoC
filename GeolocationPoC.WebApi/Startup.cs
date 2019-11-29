using GeolocationPoC.Core.Interfaces.Db;
using GeolocationPoC.Core.Interfaces.Web;
using GeolocationPoC.Persistence;
using GeolocationPoC.Persistence.Repositories.Db;
using GeolocationPoC.Persistence.Repositories.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GeolocationPoC.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GeolocationDbContext>(opt => opt.UseInMemoryDatabase("GeolocationsDB"));

            services.AddControllers();

            // Let services know how to resolve dependencies
            services.AddScoped<IRequestProvider, RequestProvider>();
            services.AddScoped<IGeolocationRepository, GeolocationRepository>();
            services.AddScoped<IGeolocationDbRepository, GeolocationDbRepository>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Put title here", Description = "DotNet Core Api 3 - with swagger" }); });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
