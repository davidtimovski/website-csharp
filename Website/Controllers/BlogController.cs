using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Website.Models;
using Website.Services;
using Website.ViewModels.Blog;

namespace Website.Controllers;

public class BlogController(IConfiguration configuration, MetricsService metricsService) : Controller
{
    private readonly string _connectionString = configuration["ConnectionStrings:DefaultConnectionString"]!;
    private readonly MetricsService _metricsService = metricsService;

    public async Task<IActionResult> Index(int? id)
    {
        _metricsService.LogHit(id is null ? "/blog" : $"/blog/{id}");

        PostViewModel postViewModel;

        string query = @"SELECT * FROM ""Posts"" WHERE ""Status"" = 1 ORDER BY ""DateCreated"" DESC";
        if (id.HasValue)
        {
            query = @"SELECT * FROM ""Posts"" WHERE ""Id"" = @Id AND ""Status"" = 1";
        }

        using DbConnection conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var post = await conn.QueryFirstAsync<Post>(query, new { Id = id });
        postViewModel = new PostViewModel
        {
            Title = post.Title,
            Body = post.Body,
            Date = post.DateCreated.ToString("dd MMMM yyyy")
        };

        // Get previous and next posts
        var previousPost = await conn.QueryFirstOrDefaultAsync<Post>(@"SELECT ""Id"", ""Title"" FROM ""Posts""
                                                                           WHERE ""Status"" = 1 AND ""DateCreated"" < @DateCreated
                                                                           ORDER BY ""DateCreated"" DESC",
                                                                       new { post.DateCreated });
        if (previousPost != null)
        {
            postViewModel.PreviousPostId = previousPost.Id;
            postViewModel.PreviousPostTitle = previousPost.Title;
        }
        var nextPost = await conn.QueryFirstOrDefaultAsync<Post>(@"SELECT ""Id"", ""Title"" FROM ""Posts""
                                                                       WHERE ""Status"" = 1 AND ""DateCreated"" > @DateCreated
                                                                       ORDER BY ""DateCreated""",
                                                                   new { post.DateCreated });
        if (nextPost != null)
        {
            postViewModel.NextPostId = nextPost.Id;
            postViewModel.NextPostTitle = nextPost.Title;
        }

        return View(postViewModel);
    }
}
