using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using cloudscribe.QueryTool.Services;

namespace cloudscribe.QueryTool.Web
{
    public class QueryToolController : Controller
    {
        public QueryToolController(
            ILogger<QueryToolController> logger,
            IQueryTool queryTool
            )
        {
            _log = logger;
            _queryTool = queryTool;
        }
        
        private ILogger _log;
        private IQueryTool _queryTool;

        public async Task<IActionResult> Index()
        {
            var result = await _queryTool.Query("select * from cs_Site");
            return Json(result);
        }


    }
}