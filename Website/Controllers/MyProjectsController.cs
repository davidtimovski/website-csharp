using Microsoft.AspNetCore.Mvc;
using Website.Services;

namespace Website.Controllers;

[ResponseCache(Duration = Constants.ResponseCacheDuration)]
[Route("my-projects")]
public class MyProjectsController(MetricsService metricsService) : Controller
{
    private readonly MetricsService _metricsService = metricsService;

    public IActionResult Index()
    {
        _metricsService.LogHit("/my-projects");

        return View();
    }

    [Route("temporal")]
    public IActionResult Temporal()
    {
        _metricsService.LogHit("/my-projects/temporal");

        return View();
    }
}
