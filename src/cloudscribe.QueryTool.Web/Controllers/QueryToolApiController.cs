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
        [Route("api/querytool/{savedQueryName?}")]
        public async Task<IActionResult> Index(string? savedQueryName)
        {
            DataTable data = new DataTable();
            ContentResult response;
            string accept = HttpContext.Request.Headers["Accept"];

            try
            {
                if(string.IsNullOrWhiteSpace(savedQueryName)) throw new Exception("Saved Query Name is required");

                var connectionString = _config.GetConnectionString("QueryToolConnectionString");
                if (string.IsNullOrWhiteSpace(connectionString)) throw new Exception("'QueryToolConnectionString' not found in config!");

                var savedQuery = await _queryToolService.LoadQueryAsync(savedQueryName);
                if (savedQuery == null) throw new Exception("A Saved Query with that name was not found");
                if (savedQuery.EnableAsApi == false) throw new Exception("This Saved Query is not enabled for API access");

                string query = savedQuery.Statement;
                bool queryHasParameters = false;
                //Get a list of needed parameters from the SQL Query, all defaulted to null
                Dictionary<string,string?> parameters = _queryToolService.ExtractParametersFromQueryString(query);
                if(parameters.Count > 0)
                {
                    queryHasParameters = true;
                    var requestParameters = HttpContext.Request.Query;
                    foreach (var item in requestParameters)
                    {
                        var key = item.Key.TrimStart('@');
                        if(parameters.ContainsKey(key))
                        {
                            if(parameters[key] != null) continue; //ignore duplicate supplied parameters, just use the first value
                            parameters[key] = item.Value;
                        }
                    }
                }

                int rowsAffected = 0;
                if(queryHasParameters)
                {
                    data = await _queryToolService.ExecuteQueryAsync(query, parameters);
                }
                else
                {
                    data = await _queryToolService.ExecuteQueryAsync(query);
                }
                rowsAffected = data.Rows.Count;
                if(queryHasParameters)
                {
                    var pText = HttpContext.Request.QueryString.Value??"?";
                    pText = pText.Remove(0, 1); // remove the leading ?
                    _log.LogInformation("QueryTool API:\nQuery Name: " + savedQueryName + "\nUserId: " + User.GetUserId() + "\nRows Affected: " + rowsAffected + "\nParameters: " + pText + "\nOutput Format: " + accept + "\nQuery: " + query);
                }
                else
                {
                    _log.LogInformation("QueryTool API:\nQuery Name: " + savedQueryName + "\nUserId: " + User.GetUserId() + "\nRows Affected: " + rowsAffected + "\nOutput Format: " + accept + "\nQuery: " + query);
                }

            }
            catch(Exception ex)
            {
                _log.LogError(ex, "QueryTool API:\nQuery Name: " + savedQueryName + "\nUserId: " + User.GetUserId()  + "\nError: " + ex.Message);
            }


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

    }
}