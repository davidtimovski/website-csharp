using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Website.Models;
using Website.Services;
using Website.ViewModels.Expertise;
using Website.ViewModels.Home;

namespace Website.Controllers;

public class HomeController(IConfiguration configuration, MetricsService metricsService) : Controller
{
    private readonly IConfiguration _configuration = configuration;
    private readonly MetricsService _metricsService = metricsService;

    [ResponseCache(Duration = Constants.ResponseCacheDuration)]
    public IActionResult Index()
    {
        _metricsService.HitsCounter.Add(1, new KeyValuePair<string, object?>(MetricsService.RouteTag, "/"));

        return View();
    }

    [Route("sapphire-notes")]
    public IActionResult SapphireNotes()
    {
        _metricsService.HitsCounter.Add(1, new KeyValuePair<string, object?>(MetricsService.RouteTag, "/sapphire-notes"));

        var viewModel = _configuration.GetSection("SapphireNotes").Get<SapphireNotesViewModel>();
        return View(viewModel);
    }

    [Route("team-sketch")]
    public IActionResult TeamSketch()
    {
        _metricsService.HitsCounter.Add(1, new KeyValuePair<string, object?>(MetricsService.RouteTag, "/team-sketch"));

        var viewModel = _configuration.GetSection("TeamSketch").Get<TeamSketchViewModel>();
        return View(viewModel);
    }

    [ResponseCache(Duration = Constants.ResponseCacheDuration)]
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
            combinedExpertise.Tags.AddRange(group.Select(x => x.Tags.Single()));
            return combinedExpertise;
        });

        return Json(result);
    }
}
