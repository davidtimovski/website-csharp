using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using Website.Models;
using Website.Models.Configuration;
using Website.Services;
using Website.ViewModels.Expertise;
using Website.ViewModels.Home;

namespace Website.Controllers;

public class HomeController : Controller
{
    private readonly IOptionsMonitor<SapphireNotesOptions> _sapphireNotesOptions;
    private readonly IOptionsMonitor<TeamSketchOptions> _teamSketchOptions;
    private readonly string _connectionString;
    private readonly MetricsService _metricsService;

    public HomeController(
        IOptionsMonitor<SapphireNotesOptions> sapphireNotesOptions,
        IOptionsMonitor<TeamSketchOptions> teamSketchOptions,
        IOptions<DatabaseOptions> databaseOptions,
        MetricsService metricsService)
    {
        _sapphireNotesOptions = sapphireNotesOptions;
        _teamSketchOptions = teamSketchOptions;
        _connectionString = databaseOptions.Value.DefaultConnectionString;
        _metricsService = metricsService;
    }

    [ResponseCache(Duration = Constants.ResponseCacheDuration)]
    public IActionResult Index()
    {
        _metricsService.LogHit("/");

        return View();
    }

    [Route("sapphire-notes")]
    public IActionResult SapphireNotes()
    {
        _metricsService.LogHit("/sapphire-notes");

        var options = _sapphireNotesOptions.CurrentValue;
        var viewModel = new SapphireNotesViewModel
        {
            Version = options.Version,
            ReleaseDate = options.ReleaseDate,
            WindowsFileSize = options.WindowsFileSize,
            DebianUbuntu64FileSize = options.DebianUbuntu64FileSize,
            DebianUbuntuARMFileSize = options.DebianUbuntuARMFileSize,
        };

        return View(viewModel);
    }

    [Route("team-sketch")]
    public IActionResult TeamSketch()
    {
        _metricsService.LogHit("/team-sketch");

        var options = _teamSketchOptions.CurrentValue;
        var viewModel = new TeamSketchViewModel
        {
            Version = options.Version,
            ReleaseDate = options.ReleaseDate,
            WindowsFileSize = options.WindowsFileSize,
            DebianUbuntu64FileSize = options.DebianUbuntu64FileSize,
            DebianUbuntuARMFileSize = options.DebianUbuntuARMFileSize,
        };

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

        using DbConnection conn = new NpgsqlConnection(_connectionString);
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
