using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Website.Models;
using Website.ViewModels.Bookmarks;

namespace Website.Controllers
{
    [ResponseCache(Duration = 60 * 60 * 24 * 7)]
    public class BookmarksController : Controller
    {
        private readonly string _connectionString;

        public BookmarksController(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnectionString"];
        }

        public async Task<IActionResult> Index()
        {
            var bookmarkViewModels = new List<BookmarkViewModel>();

            using DbConnection conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var bookmarks = await conn.QueryAsync<Bookmark>(@"SELECT * FROM ""Bookmarks"" ORDER BY ""Id"" DESC");
            foreach (var bookmark in bookmarks)
            {
                bookmarkViewModels.Add(new BookmarkViewModel
                {
                    Title = bookmark.Title,
                    Type = (BookmarkType)bookmark.Type,
                    Author = bookmark.Author,
                    Url = bookmark.Url
                });
            }

            return View(bookmarkViewModels);
        }
    }
}
