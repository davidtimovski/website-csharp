using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Website.Models;
using Website.ViewModels.Expertise;
using Website.ViewModels.Home;

namespace Website.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [ResponseCache(Duration = 60 * 60 * 24 * 7)]
    public IActionResult Index()
    {
        return View();
    }

    [Route("sapphire-notes")]
    public IActionResult SapphireNotes()
    {
        var viewModel = _configuration.GetSection("SapphireNotes").Get<SapphireNotesViewModel>();
        return View(viewModel);
    }

    [Route("team-sketch")]
    public IActionResult TeamSketch()
    {
        var viewModel = _configuration.GetSection("TeamSketch").Get<TeamSketchViewModel>();
        return View(viewModel);
    }

    [ResponseCache(Duration = 60 * 60 * 24 * 7)]
    [HttpGet]
    [Route("api/expertise")]
    public async Task<JsonResult> GetExpertise()
    {
        string query = @"SELECT e.*, t.""Name""
                            FROM ""Expertise"" AS e
                            INNER JOIN ""ExpertiseTags"" AS et ON e.""Id"" = et.""ExpertiseId""
                            INNER JOIN ""Tags"" AS t ON et.""TagId"" = t.""Id""";

        using DbConnection conn = new NpgsqlConnection(_configuration["ConnectionStrings:DefaultConnectionString"]);
        await conn.OpenAsync();

        var result = await conn.QueryAsync<ExpertiseDto, Tag, ExpertiseDto>(query, (expertise, tag) =>
        {
            expertise.Tags.Add(tag.Name);
            return expertise;
        }, null, splitOn: "Name");

        result = result.GroupBy(x => x.Id).Select(group =>
        {
            var combinedExpertise = group.First();
            combinedExpertise.Tags = group.Select(x => x.Tags.Single()).ToList();
            return combinedExpertise;
        });

        return Json(result);
    }
}
