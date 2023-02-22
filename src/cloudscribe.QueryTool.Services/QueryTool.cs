using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using cloudscribe.QueryTool.EFCore.Common;
using cloudscribe.QueryTool.Models;

namespace cloudscribe.QueryTool.Services
{
    public class QueryTool
    {
        public QueryTool(
            IServiceProvider scopedServiceProvider,
            IQueryToolDbContextFactory dbContextFactory
            )
        {
            _scopedServiceProvider = scopedServiceProvider;
            _dbContextFactory = dbContextFactory;
        }
        private readonly IServiceProvider _scopedServiceProvider;
        private readonly IQueryToolDbContextFactory _dbContextFactory;

        public async Task<QueryResult> Query(string queryString)
        {

            var db = _dbContextFactory.CreateContext();

            var rows = await db.Database.ExecuteSqlRawAsync(queryString);
            var result = await db.QueryResults.ToListAsync();



            return new QueryResult();
        }






    }
}