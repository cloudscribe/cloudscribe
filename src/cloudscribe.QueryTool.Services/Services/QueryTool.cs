using cloudscribe.QueryTool.EFCore.Common;
using Microsoft.Extensions.Logging;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public QueryTool(
            IQueryToolDbContextFactory dbContextFactory,
            ILogger<QueryTool> logger
            )
        {
            _dbContextFactory = dbContextFactory;
            _log = logger;
        }
        private readonly IQueryToolDbContextFactory _dbContextFactory;
        private readonly ILogger _log;

    }
}