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

namespace cloudscribe.QueryTool.Web
{
    [Authorize(Policy = "QueryToolAdminPolicy")]
    public partial class QueryToolController : Controller
    {
        public QueryToolController(
            ILogger<QueryToolController> logger,
            IQueryTool queryTool,
            IStringLocalizer<QueryToolResources> sr
            )
        {
            _log                = logger;
            _queryToolService   = queryTool;
            _sr                 = sr;
        }

        private readonly ILogger            _log;
        private readonly IQueryTool         _queryToolService;
        private readonly IStringLocalizer   _sr;

        [HttpGet]
        [Route("querytool")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new QueryToolViewModel();
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
                _log.LogError(ex, "QueryTool: Error in Index()");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(QueryToolViewModel model)
        {
            if(model == null) model = new QueryToolViewModel();
            if(model.Query == null) model.Query = string.Empty;
            if(model.Columns == null) model.Columns = new List<string>();

            bool queryWillReturnResults = false;
            bool queryIsValid = true;

            model.hasQuery = false;
            var query = model.Query.Trim();
            if (query.Length > 0) model.hasQuery = true;

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
                        query= string.Empty;
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
                            query = "select ";
                            if(model.Columns.Count == 0) query += "* ";
                            else
                            {
                                foreach (string c in model.Columns)
                                {
                                    query += c + ", ";
                                }
                            }
                            query = query.TrimEnd().TrimEnd(',') + " from " + model.Table + " ";
                        }
                        break;

                    case "create_update":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            if(model.Columns.Count > 0)
                            {
                                query = "update " + model.Table + " set ";
                                foreach (string c in model.Columns)
                                {
                                    query += c + " = '', ";
                                }
                                query = query.TrimEnd().TrimEnd(',') + " where ";
                            } else model.WarningMessage = _sr["You must select at least one column from the 'Columns' list!"];
                        }
                        break;

                    case "create_insert":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            if(model.Columns.Count > 0)
                            {
                                query = "insert into " + model.Table + " (";
                                foreach (string c in model.Columns)
                                {
                                    query += c + ", ";
                                }
                                query = query.TrimEnd().TrimEnd(',') + ") values (";
                                foreach (string c in model.Columns)
                                {
                                    query += " '', ";
                                }
                                query = query.TrimEnd().TrimEnd(',') + ");";
                            } else model.WarningMessage = _sr["You must select at least one column from the 'Columns' list!"];
                        }
                        break;

                    case "create_delete":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            query = "delete from " + model.Table + " where ";
                        }
                        break;

                    case "save":
                        if (queryIsValid && !string.IsNullOrWhiteSpace(model.SaveName))
                        {
                            var result = await _queryToolService.SaveQueryAsync(query, model.SaveName, model.SaveNameAsApi, User.GetUserIdAsGuid());
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
                                query = savedQuery.Statement;
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
                model.ErrorMessage = $"{ex.Message} - {ex.StackTrace}";
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

            model.Query = query;
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