using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;

namespace cloudscribe.QueryTool.Web.TagHelpers
{
    public class DataTableTagHelper : TagHelper
    {
        private const string DataAttributeName = "cs-data";

        [HtmlAttributeName(DataAttributeName)]
        public DataTable? Data { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Data == null)
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = "table";

            var trHead = new TagBuilder("tr");
            foreach (DataColumn col in Data.Columns)
            {
                var th = new TagBuilder("th");
                th.InnerHtml.Append(col.ColumnName);
                trHead.InnerHtml.AppendHtml(th);
            }
            output.Content.AppendHtml(trHead);

            var c=Data.Columns.Count;
            foreach(DataRow row in Data.Rows)
            {
                var tr = new TagBuilder("tr");
                for(int i=0; i<c; i++)
                {
                    var col = row[i];
                    if(col == null) col = string.Empty;
                    var td = new TagBuilder("td");
                    td.InnerHtml.Append(col.ToString());
                    tr.InnerHtml.AppendHtml(td);
                }
                output.Content.AppendHtml(tr);
            }

            output.Attributes.Clear();
            output.Attributes.Add("class", "table table-sm table-striped table-bordered table-hover table-responsive");
        }
    }
}
