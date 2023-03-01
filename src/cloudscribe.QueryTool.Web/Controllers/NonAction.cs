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
        private List<Dictionary<string,string>> DataTableToList(DataTable table)
        {
            List<Dictionary<string,string>> list = new List<Dictionary<string, string>>();
            foreach (DataRow row in table.Rows)
            {
                Dictionary<string,string> jsonRow = new Dictionary<string, string>();
                foreach (DataColumn col in table.Columns)
                {
                    jsonRow.Add(col.ColumnName, row[col].ToString());
                }
                list.Add(jsonRow);
            }
            return list;
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

        // [NonAction]
        // private SelectList ListToSelectList(List<string> list)
        // {
        //     List<SelectListItem> selectList = new List<SelectListItem>();
        //     foreach (var row in list)
        //     {
        //         selectList.Add(new SelectListItem()
        //         {
        //             Text = row["Text"],
        //             Value = row[valueField].ToString()
        //         });
        //     }

        //     return new SelectList(list, "Value", "Text");
        // }

        [NonAction]
        private SelectList SavedQueriesToSelectList(List<SavedQuery> queries, string valueField, List<string> textFields)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var q in queries)
            {
                if(q == null) continue;
                string text = "";
                foreach(var f in textFields)
                {
                    string value = q.GetType().GetProperty(f).GetValue(q).ToString() ?? "";
                    switch(f)
                    {
                        case "EnableAsApi":
                            if(value == "True") text += "(API ✔)" + (char)160 + (char)160;
                            break;
                        case "Name":
                            text += value + ":" + (char)160 + (char)160;
                            break;
                        case "Statement":
                            if(text.Length > 40) text +="\"" + value.Substring(0, 39) + "...\"" + (char)160 + (char)160;
                            else text += "\"" + value + "\""  + (char)160 + (char)160;
                            break;
                        default:
                            text += value  + (char)160 + (char)160;
                            break;
                   }
                }
                text = text.Remove(text.Length - 2, 2);

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