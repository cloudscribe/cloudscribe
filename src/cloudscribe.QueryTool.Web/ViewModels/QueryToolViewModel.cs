using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace cloudscribe.QueryTool.Web
{
    public class QueryToolViewModel
    {
        public QueryToolViewModel()
        {
            //TableNames = new SelectList();
        }

        public SelectList? TableNames { get; set; }
        public string Table { get; set; } = string.Empty;

        public SelectList? ColumnNames { get; set; }
        public List<string>? Columns { get; set; } = new List<string>();

        public string Query { get; set; } = string.Empty;
        public DataTable? Data { get; set; }

        public string Command { get; set; } = string.Empty;


        public int? RowsAffected { get; set; }
        public string? ErrorMessage { get; set; }

    }
}
