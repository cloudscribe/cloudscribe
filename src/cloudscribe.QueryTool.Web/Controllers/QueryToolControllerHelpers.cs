using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using cloudscribe.QueryTool.Models;
using System.Text.RegularExpressions;

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
                    var value = row[valueField].ToString();
                    if(value != null && value.Length > vLength) vLength = value.Length;
                }
            }

            foreach (DataRow row in table.Rows)
            {
                var text = row[textField].ToString();
                if (prefixTextWithValue)
                {
                    var value = row[valueField].ToString();
                    if(value != null) text = value.PadRight(vLength + 1, (char)160) + "| " + text;
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
                string value = "";
                foreach(var f in textFields)
                {
                    string v = "";
                    var field = q.GetType().GetProperty(f);
                    if(field != null)
                    {
                        var v1 = field.GetValue(q) ?? "";
                        v = v1.ToString() ?? "";
                    }
                    if(f == valueField) value = v;
                    switch(f)
                    {
                        case "EnableAsApi":
                            if(v == "True") text += "(API ✔)" + (char)160 + (char)160;
                            break;
                        case "Name":
                            text += v + ":" + (char)160 + (char)160;
                            break;
                        case "Statement":
                            if(v.Length > 40) text +="\"" + v.Substring(0, 39) + "...\"" + (char)160 + (char)160;
                            else text += "\"" + v + "\""  + (char)160 + (char)160;
                            break;
                        default:
                            text += v  + (char)160 + (char)160;
                            break;
                   }
                }
                text = text.Remove(text.Length - 2, 2);

                list.Add(new SelectListItem()
                {
                    Text = text,
                    Value = value //q.GetType().GetProperty(valueField).GetValue(q).ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }
    }
}