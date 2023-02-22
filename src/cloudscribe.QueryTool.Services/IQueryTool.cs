using cloudscribe.QueryTool.Models;

namespace cloudscribe.QueryTool.Services;

public interface IQueryTool
{
    Task<QueryResult> Query(string queryString);
}