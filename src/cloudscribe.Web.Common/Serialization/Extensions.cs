using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace cloudscribe.Web.Common.Serialization
{
    public static class Extensions
    {
        //https://tools.ietf.org/html/rfc4180 csv spec
        public static string ToCsv<T>(
            this IEnumerable<T> objectlist, 
            List<string> excludedPropertyNames = null,
            bool quoteEveryField = false,
            bool includeFieldNamesAsFirstRow = true)
        {
            if (excludedPropertyNames == null) { excludedPropertyNames = new List<string>(); }
            var separator = ",";
            Type t = typeof(T);
            PropertyInfo[] props = t.GetProperties();

            var arrPropNames = props.Where(p => !excludedPropertyNames.Contains(p.Name)).Select(f => f.Name).ToArray();
            var csvBuilder = new StringBuilder();

            if (includeFieldNamesAsFirstRow)
            {
                if (quoteEveryField)
                {
                    for(int i = 0; i <= arrPropNames.Length -1; i++)
                    {
                        if (i > 0) { csvBuilder.Append(separator); }
                        csvBuilder.Append("\"");
                        csvBuilder.Append(arrPropNames[i]);
                        csvBuilder.Append("\"");
                    }
                    csvBuilder.Append(Environment.NewLine);

                }
                else
                {
                    string header = string.Join(separator, arrPropNames);
                    csvBuilder.AppendLine(header);
                }
            }
            
            foreach (var o in objectlist)
            {
                AppendCsvRow(csvBuilder, excludedPropertyNames, separator, quoteEveryField, props, o);
                csvBuilder.Append(Environment.NewLine);
            }


            return csvBuilder.ToString();
        }

        private static void AppendCsvRow(
            StringBuilder sb, 
            List<string> excludedPropertyNames, 
            string separator, 
            bool alwaysQuote,
            PropertyInfo[] props, 
            object o)
        {
            int index = 0;
            foreach (var f in props)
            {
                if (!excludedPropertyNames.Contains(f.Name))
                {
                    if (index > 0)
                        sb.Append(separator);

                    var x = f.GetValue(o);

                    if (x != null)
                    {
                        x.ToString().AppendCsvCell(sb, alwaysQuote);
                    }
                    index++;
                }

            }

        }

        private static void AppendCsvCell(
            this string str, 
            StringBuilder sb,
            bool alwaysQuote)
        {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
            }
            else if(alwaysQuote)
            {
                sb.Append("\"");
                sb.Append(str);
                sb.Append("\"");
            }
            else
            {
                sb.Append(str);
            }

        }

    }
}
