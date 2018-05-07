using cloudscribe.HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cloudscribe.Web.Common.Html
{
    public class HtmlHelper
    {
        public static string ConvertUrlsToAbsolute(
            string absoluteBaseMediaUrl,
            string htmlInput)
        {
            if (string.IsNullOrWhiteSpace(htmlInput)) return htmlInput;
            var writer = new StringWriter();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);

            foreach (var img in doc.DocumentNode.Descendants("img"))
            {
                if (img.Attributes["src"] != null)
                {
                    var src = img.Attributes["src"].Value;
                    if (src.StartsWith("data")) continue; //base64
                    if (src.StartsWith("http")) continue;
                    img.Attributes["src"].Value = new Uri(new Uri(absoluteBaseMediaUrl), src).AbsoluteUri;

                }

            }

            foreach (var a in doc.DocumentNode.Descendants("a"))
            {
                if (a.Attributes["href"] != null)
                {
                    var href = a.Attributes["href"].Value;
                    if (href.StartsWith("http")) continue;
                    a.Attributes["href"].Value = new Uri(new Uri(absoluteBaseMediaUrl), href).AbsoluteUri;
                }

            }

            doc.Save(writer);

            var newHtml = writer.ToString();

            return newHtml;

        }

    }
}
