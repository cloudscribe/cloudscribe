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
using System.Web;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

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
                    model.ErrorMessage = _sr["'QueryToolConnectionString' not found in config!"];
                    return View(model);
                }
                var tables = await _queryToolService.GetTableList();
                model.TableNames = DataTableToSelectList(tables, "TableName", "TableName");
                if(string.IsNullOrWhiteSpace(model.Table))
                {
                    var firstTable = model.TableNames.FirstOrDefault();
                    if(firstTable != null) model.Table = firstTable.Value;
                }

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
                model.ErrorMessage = _sr["'QueryToolConnectionString' not found in config!"];
                return View(model);
            }

            bool queryWillReturnResults = false;
            bool queryIsValid = false;
            bool queryHasParameters = false;
            NameValueCollection queryNVC = new NameValueCollection();

            var query = model.Query.Trim();
            if (query.Length > 0) queryIsValid = true;

            if (queryIsValid)
            {
                if(!string.IsNullOrWhiteSpace(model.HighlightText)) query = model.HighlightText.Trim();

                //try and make an educated guess as to whether the query will return results
                if(query.StartsWith("select ", StringComparison.OrdinalIgnoreCase)) queryWillReturnResults = true;
                if(query.Contains(" select ", StringComparison.OrdinalIgnoreCase)) queryWillReturnResults = true;

            }

            try
            {
                DataTable tableList = await _queryToolService.GetTableList();
                model.TableNames = DataTableToSelectList(tableList, "TableName", "TableName");

                if(string.IsNullOrWhiteSpace(model.Table))
                {
                    var firstTable = model.TableNames.FirstOrDefault();
                    if(firstTable != null) model.Table = firstTable.Value;
                }

                if (!string.IsNullOrWhiteSpace(model.Table))
                {
                    var fields = await _queryToolService.GetColumnList(model.Table);
                    model.ColumnNames = DataTableToSelectList(fields, "ColumnName", "ColumnDataType", true);
                }

                switch (model.Command)
                {
                    case "clear":
                        model.Query = string.Empty;
                        model.Data = null;
                        model.RowsAffected = null;
                        model.HighlightStart = 0;
                        model.HighlightEnd = 0;
                        model.HighlightText = string.Empty;
                        break;

                    case "query":
                    case "export":
                        if (queryIsValid)
                        {
                            //Get a list of needed parameters from the SQL Query, all defaulted to null
                            Dictionary<string,string?> parameters = _queryToolService.ExtractParametersFromQueryString(query);
                            if(parameters.Count > 0) queryHasParameters = true;

                            if(queryHasParameters)
                            {
                                if(!string.IsNullOrWhiteSpace(model.QueryParameters)) {
                                    queryNVC = HttpUtility.ParseQueryString("?" + model.QueryParameters);
                                }
                                foreach(var p in queryNVC.AllKeys)
                                {
                                    if(p != null) {
                                        var key = p.TrimStart('@');
                                        if(parameters.ContainsKey(key) && parameters[key]!=null) continue; //ignore duplicate supplied parameters, just use the first value
                                        parameters[key]=queryNVC[p]; //overwrite the default null value with the supplied value
                                    }
                                }

                                if(queryWillReturnResults)
                                {
                                    model.Data = await _queryToolService.ExecuteQueryAsync(query, parameters);
                                    if (model.Data != null) model.RowsAffected = model.Data.Rows.Count;
                                }
                                else
                                {
                                    model.RowsAffected = await _queryToolService.ExecuteNonQueryAsync(query, parameters);
                                }
                                _log.LogInformation("QueryTool:\nUserId: " + User.GetUserId() + "\nRows Affected: " + model.RowsAffected + "\nParameters: " + model.QueryParameters + "\nQuery: " + query);
                            }
                            else
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
                                _log.LogInformation("QueryTool:\nUserId: " + User.GetUserId() + "\nRows Affected: " + model.RowsAffected + "\nQuery: " + query);

                            }
                        }
                        break;

                    case "create_select":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            var databaseType = _queryToolService.GetDatabaseType();

                            var q = "select ";
                            if(model.Columns.Count == 0) q += "* ";
                            else
                            {
                                foreach (string c in model.Columns)
                                {
                                    if(databaseType.isMySql) q += "`" + c + "`, ";
                                    else q += "\"" + c + "\", ";
                                }
                            }
                            if(databaseType.isMySql) q = q.TrimEnd().TrimEnd(',') + " from `" + model.Table + "` ";
                            else q = q.TrimEnd().TrimEnd(',') + " from \"" + model.Table + "\" ";
                            model.Query = q;
                            model.HighlightStart = 0;
                            model.HighlightEnd = 0;
                            model.HighlightText = string.Empty;
                        }
                        break;

                    case "create_update":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            if(model.Columns.Count > 0)
                            {
                                var databaseType = _queryToolService.GetDatabaseType();

                                var q = "update ";
                                if(databaseType.isMySql) q += "`" + model.Table + "` set ";
                                else q += "\"" + model.Table + "\" set ";
                                foreach (string c in model.Columns)
                                {
                                    if(databaseType.isMySql) q += "`" + c + "` = '', ";
                                    else q += "\"" + c + "\" = '', ";
                                }
                                q = q.TrimEnd().TrimEnd(',') + " where ";
                                model.Query = q;
                                model.HighlightStart = 0;
                                model.HighlightEnd = 0;
                                model.HighlightText = string.Empty;
                            } else model.WarningMessage = _sr["You must select at least one column from the 'Columns' list!"];
                        }
                        break;

                    case "create_insert":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            if(model.Columns.Count > 0)
                            {
                                var databaseType = _queryToolService.GetDatabaseType();

                                var q = "insert into ";
                                if(databaseType.isMySql) q += "`" + model.Table + "` (";
                                else q += "\"" + model.Table + "\" (";
                                foreach (string c in model.Columns)
                                {
                                    if(databaseType.isMySql) q += "`" + c + "`, ";
                                    else q += "\"" + c + "\", ";
                                }
                                q = q.TrimEnd().TrimEnd(',') + ") values (";
                                foreach (string c in model.Columns)
                                {
                                    q += " '', ";
                                }
                                q = q.TrimEnd().TrimEnd(',') + ");";
                                model.Query = q;
                                model.HighlightStart = 0;
                                model.HighlightEnd = 0;
                                model.HighlightText = string.Empty;
                            } else model.WarningMessage = _sr["You must select at least one column from the 'Columns' list!"];
                        }
                        break;

                    case "create_delete":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            var databaseType = _queryToolService.GetDatabaseType();

                            if(databaseType.isMySql) model.Query = "delete from `" + model.Table + "` where ";
                            else model.Query = "delete from \"" + model.Table + "\" where ";
                            model.HighlightStart = 0;
                            model.HighlightEnd = 0;
                            model.HighlightText = string.Empty;
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
                                model.HighlightStart = 0;
                                model.HighlightEnd = 0;
                                model.HighlightText = string.Empty;
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
                model.ErrorMessage = ex.Message;
                queryIsValid = false;
                _log.LogError(ex, "QueryTool:\nUserId: " + User.GetUserId() + "\nCommand: " + model.Command + "\nQuery: " + query);
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

            model.QueryIsValid = queryIsValid;
            model.HasQuery = queryIsValid;

            try
            {
                var queries = await _queryToolService.GetSavedQueriesAsync();
                var textFields = new List<string> { "Name", "Statement", "EnableAsApi" };
                model.SavedQueryNames = SavedQueriesToSelectList(queries, "Name", textFields);
            }
            catch(Exception ex)
            {
                _log.LogError(ex, "QueryTool:\nUserId: " + User.GetUserId() + "\nGetSavedQueriesAsync() failed");
            }

            if (!string.IsNullOrWhiteSpace(model.SavedQueryName))
            {
                var savedQuery = await _queryToolService.LoadQueryAsync(model.SavedQueryName);
                if (savedQuery != null)
                {
                    model.QueryIsAPI = savedQuery.EnableAsApi;
                }
            }

            if(model.WarningMessage != null) this.AlertWarning(model.WarningMessage, true);
            if(model.InformationMessage != null) this.AlertInformation(model.InformationMessage, true);

            return View(model);
        }
    }
}