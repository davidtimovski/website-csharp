using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;

[ResponseCache(Duration = 60 * 60 * 24 * 7)]
[Route("my-projects")]
public class MyProjectsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("temporal")]
    public IActionResult Temporal()
    {
        return View();
    }
}
