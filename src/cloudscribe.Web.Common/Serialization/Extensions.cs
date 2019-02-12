using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
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

        public static string ToCsv(this DataTable table)
        {
            var sb = new StringBuilder();

            var iColCount = table.Columns.Count;
            int i;
            for (i = 0; i <= iColCount - 1; i++)
            {
                sb.Append(table.Columns[i]);
                if (i < iColCount - 1)
                {
                    sb.Append(",");
                }
            }
            sb.AppendLine("");

            i = 0;

            foreach (DataRow row in table.Rows)
            {

                for (i = 0; i <= iColCount - 1; i++)
                {
                    if (!Convert.IsDBNull(row[i]))
                    {
                        sb.Append("\"");
                        // see #7 in this article
                        //http://tools.ietf.org/html/rfc4180
                        sb.Append(row[i].ToString().CsvEscapeQuotes());
                        sb.Append("\"");
                    }
                    else
                    {
                        sb.Append("");
                    }
                    if (i < iColCount - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.AppendLine("");
            }



            return sb.ToString();

        }

        public static string CsvEscapeQuotes(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            return s.Replace("\"", "\"\"");

        }

        public static string ToWordDocString(this DataTable table)
        {
            var wordString = new StringBuilder();

            wordString.Append(@"<html " +
                    "xmlns:o='urn:schemas-microsoft-com:office:office' " +
                    "xmlns:w='urn:schemas-microsoft-com:office:word'" +
                    "xmlns='http://www.w3.org/TR/REC-html40'>" +
                    "<head><title>Time</title>");

            wordString.Append(@"<!--[if gte mso 9]>" +
                                     "<xml>" +
                                     "<w:WordDocument>" +
                                     "<w:View>Print</w:View>" +
                                     "<w:Zoom>90</w:Zoom>" +
                                     "</w:WordDocument>" +
                                     "</xml>" +
                                     "<![endif]-->");

            wordString.Append(@"<style>" +
                                    "<!-- /* Style Definitions */" +
                                    "@page Section1" +
                                    "   {size:8.5in 11.0in; " +
                                    "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                                    "   mso-header-margin:.5in; " +
                                    "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                                    " div.Section1" +
                                    "   {page:Section1;}" +
                                    "-->" +
                                   "</style></head>");

            wordString.Append(@"<body lang=EN-US style='tab-interval:.5in'>" +
                                    "<div class=Section1>" +
                                    "<h1>Time and tide wait for none</h1>" +
                                    "<p style='color:red'><I>" +
                                    DateTime.Now + "</I></p></div><table border='1'>");


            wordString.Append("<tr>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                wordString.Append("<td>" + table.Columns[i].ColumnName + "</td>");
            }
            wordString.Append("</tr>");

            //Items
            for (int x = 0; x < table.Rows.Count; x++)
            {
                wordString.Append("<tr>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    wordString.Append("<td>" + table.Rows[x][i] + "</td>");
                }
                wordString.Append("</tr>");
            }

            wordString.Append(@"</table></body></html>");

            return wordString.ToString();
        }

        public static string ToDateTimeStringForFileName(this DateTime d, bool includeMiliseconds = false)
        {
           
            string dateString = d.Year.ToString();

            string monthString = d.Month.ToString();
            if (monthString.Length == 1)
            {
                monthString = "0" + monthString;
            }
            string dayString = d.Day.ToString();
            if (dayString.Length == 1)
            {
                dayString = "0" + dayString;
            }
            string hourString = d.Hour.ToString();
            if (hourString.Length == 1)
            {
                hourString = "0" + hourString;
            }

            string minuteString = d.Minute.ToString();
            if (minuteString.Length == 1)
            {
                minuteString = "0" + minuteString;
            }

            string secondString = d.Second.ToString();
            if (secondString.Length == 1)
            {
                secondString = "0" + secondString;
            }

            dateString
                = dateString
                + monthString
                + dayString
                + hourString
                + minuteString + secondString;

            if (includeMiliseconds)
            {
                return dateString + d.Millisecond.ToString();
            }

            return dateString;
        }

        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public static T FromByteArray<T>(this byte[] byteArray) where T : class
        {
            if (byteArray == null)
            {
                return default(T);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                return binaryFormatter.Deserialize(memoryStream) as T;
            }
        }

    }
}
