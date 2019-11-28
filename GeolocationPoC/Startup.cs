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

namespace GeolocationPoC
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
            services.AddDbContext<GeolocationDbContext>(opt => opt.UseInMemoryDatabase("GeolocationsDB")); ;
            services.AddControllersWithViews();

            // Let services know how to resolve dependencies
            services.AddScoped<IRequestProvider, RequestProvider>();
            services.AddScoped<IGeolocationRepository, GeolocationRepository>();
            services.AddScoped<IGeolocationDbRepository, GeolocationDbRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Geolocation}/{action=Index}");
            });
        }
    }
}
