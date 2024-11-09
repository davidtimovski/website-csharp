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
using Website.ViewModels.Bookmarks;

namespace Website.Controllers;

[ResponseCache(Duration = Constants.ResponseCacheDuration)]
public class BookmarksController : Controller
{
    private readonly string _connectionString;
    private readonly MetricsService _metricsService;
    
    public BookmarksController(IOptions<DatabaseOptions> databaseOptions, MetricsService metricsService)
    {
        _connectionString = databaseOptions.Value.DefaultConnectionString;
        _metricsService = metricsService;
    }

    public async Task<IActionResult> Index()
    {
        _metricsService.LogHit("/bookmarks");

        using DbConnection conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var bookmarks = await conn.QueryAsync<Bookmark>(@"SELECT * FROM ""Bookmarks"" ORDER BY ""Id"" DESC");
        var bookmarkViewModels = bookmarks.Select(x => new BookmarkViewModel
        {
            Title = x.Title,
            Type = (BookmarkType)x.Type,
            Author = x.Author,
            Url = x.Url
        }).ToList();

        return View(bookmarkViewModels);
    }
}
