using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(option =>
{
    option.LowercaseUrls = true;
});
builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});
builder.Services.AddResponseCaching();

builder.WebHost.UseUrls("http://localhost:5050");

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
