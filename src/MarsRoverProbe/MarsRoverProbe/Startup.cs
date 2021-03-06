using MarsRoverProbe.Data;
using MarsRoverProbe.Infrastructure;
using MarsRoverProbe.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using System;

namespace MarsRoverProbe
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
            services.AddTransient<IMarsRoverPhotosService, MarsRoverPhotosService>();
            services.AddTransient<IStorage, FileStorage>();
            services.AddTransient<IProgressLogger, SignalRProgressLogger>();

            services.Configure<AppSetting>(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddRefitClient<INasaApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.nasa.gov/mars-photos/api/v1"));

            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddSignalR();
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<LogHub>("/loghub");
            });
        }
    }
}
