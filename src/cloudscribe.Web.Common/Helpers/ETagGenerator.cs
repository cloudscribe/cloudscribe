using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace cloudscribe.Web.Common.Helpers
{
    public static class ETagGenerator
    {
        

        public static StatusCodeResult AddEtagForStream(HttpContext context, Stream stream)
        {
            if(!stream.CanSeek) { return null; }

            var checksum = CalculateChecksum(stream);
            context.Response.Headers[HeaderNames.ETag] = checksum;

            if (context.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etag) && checksum == etag)
            {
                return new StatusCodeResult(304);
            }

            stream.Seek(0, SeekOrigin.Begin);

            return null;
        }

        public static string CalculateChecksum(Stream ms)
        {
            string checksum = "";

            using (var algo = SHA1.Create())
            {
                ms.Position = 0;
                byte[] bytes = algo.ComputeHash(ms);
                checksum = $"\"{WebEncoders.Base64UrlEncode(bytes)}\"";
            }

            return checksum;
        }
    }
}
