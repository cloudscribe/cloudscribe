using System.Data;
using System.Text;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public async Task<List<Dictionary<string,string>>> DataTableToDictionaryList(DataTable table)
        {
            List<Dictionary<string,string>> list = new List<Dictionary<string, string>>();
            await Task.Run(() =>
            {
                foreach (DataRow row in table.Rows)
                {
                    Dictionary<string,string> jsonRow = new Dictionary<string, string>();
                    foreach (DataColumn col in table.Columns)
                    {
                        var value = row[col].ToString();
                        jsonRow.Add(col.ColumnName, value ?? string.Empty);
                    }
                    list.Add(jsonRow);
                }
            });
            return list;
        }
    }
}