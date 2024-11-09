using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using Website.Models.Configuration;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MetricsService>();

builder.Services.AddRouting(option =>
{
    option.LowercaseUrls = true;
});
builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});
builder.Services.AddResponseCaching();

builder.Services.AddOpenTelemetry()
    .WithMetrics(opt =>
    {
        opt.AddPrometheusExporter();

        opt.AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "Website");

        opt.AddView("http.server.request.duration",
            new ExplicitBucketHistogramConfiguration
            {
                Boundaries = [ 0, 0.005, 0.01, 0.025, 0.05,
                       0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 ]
            });
    });

builder.WebHost.UseUrls("http://localhost:5050");

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection(DatabaseOptions.Section));
builder.Services.Configure<TeamSketchOptions>(
    builder.Configuration.GetSection(TeamSketchOptions.Section));
builder.Services.Configure<SapphireNotesOptions>(
    builder.Configuration.GetSection(SapphireNotesOptions.Section));

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error")
       .UseResponseCaching();
}

app.MapPrometheusScrapingEndpoint();

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

app.Run();
