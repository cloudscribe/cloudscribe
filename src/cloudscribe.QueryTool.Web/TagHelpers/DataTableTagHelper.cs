using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;
using System.Web;

namespace cloudscribe.QueryTool.Web.TagHelpers
{
    public class DataTableTagHelper : TagHelper
    {
        private const string IdAttributeName = "cs-id";
        private const string DataAttributeName = "cs-data";
        private const string HeightAttributeName = "cs-height";
        private const string FixHeadingAttributeName = "cs-fix-heading";
        private const string CaptionAttributeName = "cs-caption";

        [HtmlAttributeName(IdAttributeName)]
        public string? Id { get; set; } = null;

        [HtmlAttributeName(DataAttributeName)]
        public DataTable? Data { get; set; } = null;

        [HtmlAttributeName(HeightAttributeName)]
        public int Height { get; set; } = 600;

        [HtmlAttributeName(FixHeadingAttributeName)]
        public bool FixHeading { get; set; }= true;

        [HtmlAttributeName(CaptionAttributeName)]
        public string? Caption { get; set; } = null;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Data == null)
            {
                output.SuppressOutput();
                return;
            }

            output.Content.Clear();
            output.TagName = "div";

            var html = "<table class=\"table table-sm table-striped table-bordered table-hover\">";
            html += @"
                <thead class=""table-dark""><tr>";
            foreach (DataColumn col in Data.Columns)
            {
                html += @$"
                    <th>{col.ColumnName}</th>";
            }
            html += @"
                </tr></thead>
                <tbody>";
            var c=Data.Columns.Count;
            foreach(DataRow row in Data.Rows)
            {
                html += @"
                    <tr>";
                for(int i=0; i<c; i++)
                {
                    var col = row[i];
                    html += @$"
                        <td>{HttpUtility.HtmlEncode(col.ToString()??string.Empty)}</td>";
                }
                html += @"
                    </tr>";
            }
            html += @"
                </tbody>
            </table>";

            output.Content.AppendHtml(html);
            output.Attributes.Clear();
            output.Attributes.Add("class", "table-responsive border border-secondary rounded");
            if(!string.IsNullOrWhiteSpace(Caption)) output.Attributes.Add("title", Caption);
            if(!string.IsNullOrWhiteSpace(Id))
            {
                output.Attributes.Add("id", Id);
                if(FixHeading)
                {
                    var heightStyle = "";
                    if(Height > 0) heightStyle = $"height: {Height}px;";
                    output.Content.AppendHtml(@$"
                    <style>
                    #" + Id + "{ overflow: scroll; " + heightStyle + "width: 100%; }" + @"
                    #" + Id + " table { border-collapse: collapse; }" + @"
                    #" + Id + " table th, #" + Id + " table td { max-width: 100%; padding: 8px 16px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }" + @"
                    #" + Id + " table thead { position: sticky; inset-block-start: 0; }" + @"
                    </style>");

                }
            }
        }
    }
}
