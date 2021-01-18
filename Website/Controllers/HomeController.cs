using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Website.Models;
using Website.ViewModels.Expertise;

namespace Website.Controllers
{
    [ResponseCache(Duration = 60 * 60 * 24 * 7)]
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnectionString"];
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("sapphire-notes")]
        public ActionResult SapphireNotes()
        {
            return View();
        }

        [ResponseCache(Duration = 60 * 60 * 24 * 7)]
        [HttpGet]
        [Route("api/expertise")]
        public async Task<IActionResult> GetExpertise()
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
                combinedExpertise.Tags = group.Select(x => x.Tags.Single()).ToList();
                return combinedExpertise;
            });

            return Json(result);
        }
    }
}