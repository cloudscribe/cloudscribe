using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using cloudscribe.QueryTool.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.QueryTool.Models;
using cloudscribe.Web.Common.Extensions;
using System.Text;

namespace cloudscribe.QueryTool.Web
{
    [Authorize(Policy = "ServerAdminPolicy")]
    public class QueryToolController : Controller
    {
        public QueryToolController(
            ILogger<QueryToolController> logger,
            IQueryTool queryTool
            )
        {
            _log = logger;
            _queryToolService = queryTool;
        }

        private ILogger _log;
        private IQueryTool _queryToolService;

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
            var query = model.Query.Trim();

            if(model.Command == "query" || model.Command == "export" || model.Command == "save" )
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
                model.ErrorMessage = "Invalid query! Only SELECT, INSERT, UPDATE, and DELETE are allowed.";
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
                            } else model.WarningMessage = "You must select at least one column from the 'Columns' list!";
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
                            } else model.WarningMessage = "You must select at least one column from the 'Columns' list!";
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
                            if (result) model.InformationMessage = "Query saved";
                            else model.WarningMessage = "Error saving query";
                        }
                        break;

                    case "load":
                        if (!string.IsNullOrWhiteSpace(model.SavedQueryName))
                        {
                            var savedQuery = await _queryToolService.LoadQueryAsync(model.SavedQueryName);
                            if (savedQuery != null)
                            {
                                query = savedQuery.Statement;
                                model.InformationMessage = "Query loaded";
                            } else model.WarningMessage = "Error loading query";
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

        [NonAction]
        private string DataTableToCsv(DataTable table)
        {
            var sb = new StringBuilder();
            var headers = table.Columns.Cast<DataColumn>();
            sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.ColumnName + "\"").ToArray()));
            foreach (DataRow row in table.Rows)
            {
                var fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"");
                sb.AppendLine(string.Join(",", fields));
            }
            return sb.ToString();
        }

        [NonAction]
        private SelectList DataTableToSelectList(DataTable table, string valueField, string textField, bool prefixTextWithValue = false)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            int vLength = 0;
            if(prefixTextWithValue)
            {
                foreach (DataRow row in table.Rows)
                {
                    if(row[valueField].ToString().Length > vLength) vLength = row[valueField].ToString().Length;
                }
            }
            Console.WriteLine(vLength.ToString());

            foreach (DataRow row in table.Rows)
            {
                string text = row[textField].ToString();
                if (prefixTextWithValue)
                {
                    text = row[valueField].ToString().PadRight(vLength + 1, (char)160) + "| " + text;
                }
                list.Add(new SelectListItem()
                {
                    Text = text,
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        [NonAction]
        private SelectList SavedQueriesToSelectList(List<SavedQuery> queries, string valueField, List<string> textFields)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var q in queries)
            {
                string text = "";
                foreach(var f in textFields)
                {
                    text += q.GetType().GetProperty(f).GetValue(q).ToString() + " - ";
                }
                text = text.TrimEnd().TrimEnd('-').TrimEnd();

                list.Add(new SelectListItem()
                {
                    Text = text,
                    Value = q.GetType().GetProperty(valueField).GetValue(q).ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }


    }
}