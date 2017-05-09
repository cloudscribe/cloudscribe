using System.Net;
using System.Text;

namespace cloudscribe.Web.Common.Helpers
{
    public static class JavaScriptHelper
    {

        /// <summary>
        /// html encodes a string then replaces quote entities with escaped single quotes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HtmlEncodeThenReplaceQuotesForJs(string input)
        {
            var result = WebUtility.HtmlEncode(input);

            return result.Replace("&#39;", "\\\'").Replace("&quot;", "\\\'");
        }

        
    }
}
