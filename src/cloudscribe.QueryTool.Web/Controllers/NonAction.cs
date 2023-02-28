using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using cloudscribe.QueryTool.Models;
using System.Text;

namespace cloudscribe.QueryTool.Web
{

    public partial class QueryToolController : Controller
    {
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