using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using cloudscribe.QueryTool.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

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
            model.TableNames = ToSelectList(tables, "TableName", "TableName");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(QueryToolViewModel model)
        {
            if(model == null) model = new QueryToolViewModel();
            if(model.Query == null) model.Query = string.Empty;
            if(model.Columns == null) model.Columns = new List<string>();

            try
            {
                var tables = await _queryToolService.GetTableList();
                model.TableNames = ToSelectList(tables, "TableName", "TableName");

                if(model.Table == null) model.Table = model.TableNames.FirstOrDefault()?.Value;

                if (!string.IsNullOrWhiteSpace(model.Table))
                {
                    var fields = await _queryToolService.GetFieldList(model.Table);
                    model.ColumnNames = ToSelectList(fields, "ColumnName", "ColumnName");
                }

                switch (model.Command)
                {
                    case "clear":
                        model.Query = string.Empty;
                        model.Data = null;
                        model.RowsAffected = null;
                        break;
                    case "query":
                        if (!string.IsNullOrWhiteSpace(model.Query))
                            model.Data = await _queryToolService.ExecuteQueryAsync(model.Query);
                        break;

                    case "nonquery":
                        if (!string.IsNullOrWhiteSpace(model.Query))
                            model.RowsAffected = await _queryToolService.ExecuteNonQueryAsync(model.Query);
                        break;

                    case "create_select":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            model.Query = "select ";
                            if(model.Columns.Count == 0) model.Query += "* ";
                            else
                            {
                                foreach (string c in model.Columns)
                                {
                                    model.Query += c + ", ";
                                }
                            }
                            model.Query = model.Query.TrimEnd().TrimEnd(',') + " from " + model.Table + " ";
                        }
                        break;

                    case "create_update":
                        if (!string.IsNullOrWhiteSpace(model.Table) && model.Columns.Count > 0)
                        {
                            model.Query = "update " + model.Table + " set ";
                            foreach (string c in model.Columns)
                            {
                                model.Query += c + " = '', ";
                            }
                            model.Query = model.Query.TrimEnd().TrimEnd(',') + " where ";
                        }
                        break;

                    case "create_insert":
                        if (!string.IsNullOrWhiteSpace(model.Table) && model.Columns.Count > 0)
                        {
                            model.Query = "insert into " + model.Table + " (";
                            foreach (string c in model.Columns)
                            {
                                model.Query += c + ", ";
                            }
                            model.Query = model.Query.TrimEnd().TrimEnd(',') + ") values (";
                            foreach (string c in model.Columns)
                            {
                                model.Query += " '', ";
                            }
                            model.Query = model.Query.TrimEnd().TrimEnd(',') + ") ";
                        }
                        break;

                    case "create_delete":
                        if (!string.IsNullOrWhiteSpace(model.Table))
                        {
                            model.Query = "delete from " + model.Table + " where ";
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


            return View(model);
        }



        [NonAction]
        private SelectList ToSelectList(DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }


    }
}