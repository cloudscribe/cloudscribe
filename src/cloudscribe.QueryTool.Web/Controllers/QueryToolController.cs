using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using cloudscribe.QueryTool.Services;
using Microsoft.AspNetCore.Authorization;
using cloudscribe.Core.Identity;
using cloudscribe.Web.Common.Extensions;
using System.Text;
using Microsoft.Extensions.Localization;
using System.Data;
using cloudscribe.Web.Common.Serialization;
using Microsoft.Extensions.Configuration;

namespace cloudscribe.QueryTool.Web
{
    [Authorize(Policy = "QueryToolAdminPolicy")]
    public partial class QueryToolController : Controller
    {
        public QueryToolController(
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
        [Route("querytool")]
        public async Task<IActionResult> Index()
        {
            var model = new QueryToolViewModel();
            try
            {
                var connectionString = _config.GetConnectionString("QueryToolConnectionString");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    model.ErrorMessage = _sr["Connection string \"QueryToolConnectionString\" not found in config!"];
                    return View(model);
                }
                var tables = await _queryToolService.GetTableList();
                model.TableNames = DataTableToSelectList(tables, "TableName", "TableName");
                if(string.IsNullOrWhiteSpace(model.Table)) model.Table = model.TableNames.FirstOrDefault()?.Value;

                if (!string.IsNullOrWhiteSpace(model.Table))
                {
                    var fields = await _queryToolService.GetColumnList(model.Table);
                    model.ColumnNames = DataTableToSelectList(fields, "ColumnName", "ColumnDataType", true);
                }
                var queries = await _queryToolService.GetSavedQueriesAsync();
                var textFields = new List<string> { "Name", "Statement", "EnableAsApi" };
                model.SavedQueryNames = SavedQueriesToSelectList(queries, "Name", textFields);
                return View(model);
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
                _log.LogError(ex, "QueryTool: Error in Index()");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(QueryToolViewModel model)
        {
            if(model == null) model = new QueryToolViewModel();
            if(model.Query == null) model.Query = string.Empty;
            if(model.Columns == null) model.Columns = new List<string>();

            var connectionString = _config.GetConnectionString("QueryToolConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                model.ErrorMessage = _sr["Connection string \"QueryToolConnectionString\" not found in config!"];
                return View(model);
            }

            bool queryWillReturnResults = false;
            bool queryIsValid = true;

            model.hasQuery = false;
            var query = model.Query.Trim();
            if (query.Length > 0) model.hasQuery = true;
            if (model.hasQuery)
            {
                if(!string.IsNullOrWhiteSpace(model.HighlightText)) query = model.HighlightText.Trim();
            }

            switch(model.Command)
            {
                case "query":
                case "export":
                case "save":
                case "delete":
                    if(query.Length > 0)
                    {
                        queryIsValid = false;
                        if (query.StartsWith("select ", StringComparison.InvariantCultureIgnoreCase))
                        {
                            queryWillReturnResults = true;
                            queryIsValid = true;
                        }
                        else if (query.StartsWith("insert into ", StringComparison.InvariantCultureIgnoreCase))
                        {
                            queryIsValid = true;
                        }
                        else if (query.StartsWith("update ", StringComparison.InvariantCultureIgnoreCase))
                        {
                            queryIsValid = true;
                        }
                        else if (query.StartsWith("delete from ", StringComparison.InvariantCultureIgnoreCase))
                        {
                            queryIsValid = true;
                        }
                    }
                    break;
            }

            if(!queryIsValid)
            {
                model.ErrorMessage = _sr["Invalid query! Only SELECT, INSERT, UPDATE, and DELETE are allowed."];
            }


            try
            {
                DataTable tableList = await _queryToolService.GetTableList();
                model.TableNames = DataTableToSelectList(tableList, "TableName", "TableName");

                if(string.IsNullOrWhiteSpace(model.Table))
                {
                    var table = model.TableNames.FirstOrDefault().Value;
                }

                if (!string.IsNullOrWhiteSpace(model.Table))
                {
                    var fields = await _queryToolService.GetColumnList(model.Table);
                    model.ColumnNames = DataTableToSelectList(fields, "ColumnName", "ColumnDataType", true);
                }

                switch (model.Command)
                {
                    case "clear":
                        // query= string.Empty;
                        model.Query = string.Empty;
                        model.Data = null;
                        model.RowsAffected = null;
                        break;

                    case "query":
                    case "export":
                        if (queryIsValid)
                        {
                            if(queryWillReturnResults)
                            {
                                model.Data = await _queryToolService.ExecuteQueryAsync(query);
                                if (model.Data != null) model.RowsAffected = model.Data.Rows.Count;
                            }
                            else
                            {
                                model.RowsAffected = await _queryToolService.ExecuteNonQueryAsync(query);
                            }
                            _log.LogInformation("QueryTool: UserId:" + User.GetUserId() + " Query: " + query + " Rows Affected: " + model.RowsAffected);
                        }
                        break;

                    case "create_select":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            var q = "select ";
                            if(model.Columns.Count == 0) q += "* ";
                            else
                            {
                                foreach (string c in model.Columns)
                                {
                                    q += c + ", ";
                                }
                            }
                            q = q.TrimEnd().TrimEnd(',') + " from " + model.Table + " ";
                            model.Query = q;
                        }
                        break;

                    case "create_update":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            if(model.Columns.Count > 0)
                            {
                                var q = "update " + model.Table + " set ";
                                foreach (string c in model.Columns)
                                {
                                    q += c + " = '', ";
                                }
                                q = q.TrimEnd().TrimEnd(',') + " where ";
                                model.Query = q;
                            } else model.WarningMessage = _sr["You must select at least one column from the 'Columns' list!"];
                        }
                        break;

                    case "create_insert":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            if(model.Columns.Count > 0)
                            {
                                var q = "insert into " + model.Table + " (";
                                foreach (string c in model.Columns)
                                {
                                    q += c + ", ";
                                }
                                q = q.TrimEnd().TrimEnd(',') + ") values (";
                                foreach (string c in model.Columns)
                                {
                                    q += " '', ";
                                }
                                q = q.TrimEnd().TrimEnd(',') + ");";
                                model.Query = q;
                            } else model.WarningMessage = _sr["You must select at least one column from the 'Columns' list!"];
                        }
                        break;

                    case "create_delete":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            model.Query = "delete from " + model.Table + " where ";
                        }
                        break;

                    case "save":
                        if (queryIsValid && !string.IsNullOrWhiteSpace(model.SaveName))
                        {
                            var q = model.Query.Trim();
                            var result = await _queryToolService.SaveQueryAsync(q, model.SaveName, model.SaveNameAsApi, User.GetUserIdAsGuid());
                            if (result) model.InformationMessage = _sr["Query saved"];
                            else model.WarningMessage = _sr["Error saving query"];
                        }
                        break;

                    case "load":
                        if (!string.IsNullOrWhiteSpace(model.SavedQueryName))
                        {
                            var savedQuery = await _queryToolService.LoadQueryAsync(model.SavedQueryName);
                            if (savedQuery != null)
                            {
                                model.Query = savedQuery.Statement;
                                model.InformationMessage = _sr["Query loaded"];
                                model.SaveName = model.SavedQueryName;
                                model.SaveNameAsApi = savedQuery.EnableAsApi;
                            } else model.WarningMessage = _sr["Error loading query"];
                        }
                        break;

                    case "delete":
                        if (!string.IsNullOrWhiteSpace(model.SavedQueryName))
                        {
                            var result = await _queryToolService.DeleteQueryAsync(model.SavedQueryName);
                            if (result) model.InformationMessage = _sr["Query deleted"];
                            else model.WarningMessage = _sr["Error deleting query"];
                        }
                        break;

                    default:
                        model.Data = null;
                        model.RowsAffected = null;
                        break;
                }

            }
            catch(Exception ex)
            {
                // model.ErrorMessage = $"{ex.Message} - {ex.StackTrace}";
                model.ErrorMessage = ex.Message;
                queryIsValid = false;
                _log.LogError(ex, "QueryTool: UserId:" + User.GetUserId() + " Query: " + query + " Command: " + model.Command);
            }

            if(model.Command == "export" && queryIsValid && model.Data != null)
            {
                var csv = model.Data.ToCsv(); // custom extension method from cloudscribe.Web.Common.Extensions
                var date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                UTF8Encoding encoding = new UTF8Encoding(true);
                byte[] contentAsBytes = encoding.GetBytes(csv);
                var withBOM = Encoding.UTF8.GetPreamble().Concat(contentAsBytes).ToArray();
                var result = new FileContentResult(withBOM, "text/csv");
                result.FileDownloadName = $"export_{date}.csv";
                return result;
            }

            // model.Query = query;
            model.QueryIsValid = queryIsValid;

            try
            {
                var queries = await _queryToolService.GetSavedQueriesAsync();
                var textFields = new List<string> { "Name", "Statement", "EnableAsApi" };
                model.SavedQueryNames = SavedQueriesToSelectList(queries, "Name", textFields);
            }
            catch(Exception ex)
            {
                _log.LogError(ex, "QueryTool: UserId:" + User.GetUserId() + " GetSavedQueriesAsync() failed");
            }


            if(model.WarningMessage != null) this.AlertWarning(model.WarningMessage, true);
            if(model.InformationMessage != null) this.AlertInformation(model.InformationMessage, true);

            return View(model);
        }
    }
}