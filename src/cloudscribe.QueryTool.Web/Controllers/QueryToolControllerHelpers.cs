using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using cloudscribe.QueryTool.Models;

namespace cloudscribe.QueryTool.Web
{
    public partial class QueryToolController : Controller
    {
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
        private SelectList DictionaryToSelectList(Dictionary<string,string> dictionary)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var row in dictionary)
            {
                selectList.Add(new SelectListItem()
                {
                    Text = row.Value.ToString(),
                    Value = row.Key.ToString()
                });
            }

            return new SelectList(selectList, "Value", "Text");
        }

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
                            if(value.Length > 40) text +="\"" + value.Substring(0, 39) + "...\"" + (char)160 + (char)160;
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