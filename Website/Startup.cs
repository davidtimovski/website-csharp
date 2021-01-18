using System;
using System.Globalization;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DavidTimovskiWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(option =>
            {
                option.LowercaseUrls = true;
            });
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
            services.AddApplicationInsightsTelemetry()
                    .AddResponseCaching();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                var telemetryConfiguration = app.ApplicationServices.GetService<TelemetryConfiguration>();
                telemetryConfiguration.DisableTelemetry = true;

                app.UseDeveloperExceptionPage()
                   .UseStaticFiles();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error")
                   .UseStaticFiles(new StaticFileOptions
                   {
                       ServeUnknownFileTypes = true,
                       OnPrepareResponse = ctx =>
                       {
                           const int yearInSeconds = 12 * 30 * 24 * 60 * 60;
                           ctx.Context.Response.Headers.Append("Cache-Control", $"public,max-age={yearInSeconds}");
                           ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddMonths(12).ToString("R", CultureInfo.InvariantCulture));
                       }
                   }).UseResponseCaching();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "Blog with id and slug",
                    "blog/{id}/{slug}",
                    new { controller = "Blog", action = "Index" }
                );
                routes.MapRoute(
                    "Blog with id",
                    "blog/{id}",
                    new { controller = "Blog", action = "Index" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
