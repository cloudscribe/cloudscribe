using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloudscribe.QueryTool.Models;

public class QueryResult
{
    public QueryResult()
    {
        Rows = new List<Dictionary<string, object>>();
    }

    public List<Dictionary<string, object>> Rows { get; set; }
}