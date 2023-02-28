using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using cloudscribe.QueryTool.Services;
using Microsoft.AspNetCore.Authorization;
using cloudscribe.Core.Identity;
using cloudscribe.Web.Common.Extensions;
using System.Text;
using Microsoft.Extensions.Localization;

namespace cloudscribe.QueryTool.Web
{
    [Authorize(Policy = "ServerAdminPolicy")]
    public partial class QueryToolController : Controller
    {
        public QueryToolController(
            ILogger<QueryToolController> logger,
            IQueryTool queryTool,
            IStringLocalizer<QueryToolResources> sr
            )
        {
            _log = logger;
            _queryToolService = queryTool;
            _sr = sr;
        }

        private ILogger _log;
        private IQueryTool _queryToolService;
        private IStringLocalizer _sr;

        [HttpGet]
        public async Task<IActionResult> Index()
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
            var textFields = new List<string> { "Name", "Statement" };
            model.SavedQueryNames = SavedQueriesToSelectList(queries, "Name", textFields);
            return View(model);
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

            if(model.Command == "query" || model.Command == "export" || model.Command == "save" || model.Command == "delete")
            {
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
            }

            if(!queryIsValid)
            {
                model.ErrorMessage = _sr["Invalid query! Only SELECT, INSERT, UPDATE, and DELETE are allowed."];
            }


            try
            {
                var tables = await _queryToolService.GetTableList();
                model.TableNames = DataTableToSelectList(tables, "TableName", "TableName");

                if(string.IsNullOrWhiteSpace(model.Table)) model.Table = model.TableNames.FirstOrDefault()?.Value;

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
                            var result = await _queryToolService.SaveQueryAsync(query, model.SaveName, User.GetUserIdAsGuid());
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
            }

            if(model.Command == "export" && queryIsValid && model.Data != null)
            {
                var csv = DataTableToCsv(model.Data);
                var bytes = Encoding.UTF8.GetBytes(csv);
                var date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                return File(bytes, "text/csv", $"export_{date}.csv");
            }

            model.Query = query;
            model.QueryIsValid = queryIsValid;

            var queries = await _queryToolService.GetSavedQueriesAsync();
            var textFields = new List<string> { "Name", "Statement" };
            model.SavedQueryNames = SavedQueriesToSelectList(queries, "Name", textFields);


            if(model.WarningMessage != null) this.AlertWarning(model.WarningMessage, true);
            if(model.InformationMessage != null) this.AlertInformation(model.InformationMessage, true);

            return View(model);
        }
    }
}