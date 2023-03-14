using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using cloudscribe.QueryTool.Services;
using Microsoft.AspNetCore.Authorization;
using cloudscribe.Web.Common.Serialization;
using cloudscribe.Core.Identity;
using System.Text;
using Microsoft.Extensions.Localization;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace cloudscribe.QueryTool.Web
{
    [Authorize(Policy = "QueryToolApiPolicy")]
    public partial class QueryToolApiController : Controller
    {
        public QueryToolApiController(
            ILogger<QueryToolController> logger,
            IQueryTool queryTool,
            IStringLocalizer<QueryToolResources> sr,
            IConfiguration config
            )
        {
            _log                = logger;
            _queryToolService   = queryTool;
            _sr                 = sr;
            _config             = config;
        }

        private readonly ILogger            _log;
        private readonly IQueryTool         _queryToolService;
        private readonly IStringLocalizer   _sr;
        private readonly IConfiguration     _config;

        [HttpGet]
        [Route("api/querytool/{savedQueryName}")]
        public async Task<IActionResult> Index(string savedQueryName)
        {
            try
            {
                var connectionString = _config.GetConnectionString("QueryToolConnectionString");
                if (string.IsNullOrWhiteSpace(connectionString)) return NotFound();
                var savedQuery = await _queryToolService.LoadQueryAsync(savedQueryName);
                if (savedQuery == null) return NotFound();
                if (savedQuery.EnableAsApi == false) return NotFound();

                string query = savedQuery.Statement;
                int rowsAffected = 0;
                DataTable data = await _queryToolService.ExecuteQueryAsync(query);
                if (data != null) rowsAffected = data.Rows.Count;

                ContentResult response;
                string accept = HttpContext.Request.Headers["Accept"];
                _log.LogInformation("QueryTool: API Query Name: " + savedQueryName + " UserId:" + User.GetUserId() + " Query: " + query + " Rows Affected: " + rowsAffected + " Output Format: " + accept);

                switch(accept)
                {
                    case "text/csv":
                        var csv = data.ToCsv(); // custom extension method from cloudscribe.Web.Common.Extensions
                        var date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        UTF8Encoding encoding = new UTF8Encoding(true);
                        byte[] contentAsBytes = encoding.GetBytes(csv);
                        var withBOM = Encoding.UTF8.GetPreamble().Concat(contentAsBytes).ToArray();
                        var result = new FileContentResult(withBOM, "text/csv");
                        result.FileDownloadName = $"export_{date}.csv";
                        return result;

                    case "text/xml":
                    case "application/xml":
                        var list1 = await _queryToolService.DataTableToDictionaryList(data);
                        var json = "{ \"Row\": \n" + Newtonsoft.Json.JsonConvert.SerializeObject(list1) + "\n}";
                        var xmlResult = Newtonsoft.Json.JsonConvert.DeserializeXNode(json, "Results");
                        response = new ContentResult() {
                            Content = xmlResult.ToString(),
                            ContentType = "application/xml",
                            StatusCode = 200
                        };
                        return response;

                    case "text/json":
                    case "application/json":
                    default:
                        var list2 = await _queryToolService.DataTableToDictionaryList(data);
                        var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(list2, Newtonsoft.Json.Formatting.Indented);
                        response = new ContentResult() {
                            Content = jsonResult,
                            ContentType = "application/json",
                            StatusCode = 200
                        };
                        return response;
                }
            }
            catch(Exception ex)
            {
                _log.LogError(ex, "QueryTool: API Query Name: " + savedQueryName + " UserId:" + User.GetUserId() + " Error: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}