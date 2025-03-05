using System.Text.RegularExpressions;

namespace cloudscribe.Web.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToSafeFileName(this string input)
        {
            var invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            var invalidReStr = string.Format(@"[{0}]+", invalidChars);
            return Regex.Replace(input, invalidReStr, "_");
        }
    }
}