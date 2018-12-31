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

        //public static string GetETag(string key, byte[] contentBytes)
        //{
        //    var keyBytes = Encoding.UTF8.GetBytes(key);
        //    var combinedBytes = Combine(keyBytes, contentBytes);

        //    return GenerateETag(combinedBytes);
        //}

        //private static string GenerateETag(byte[] data)
        //{
        //    using (var md5 = MD5.Create())
        //    {
        //        var hash = md5.ComputeHash(data);
        //        string hex = BitConverter.ToString(hash);
        //        return hex.Replace("-", "");
        //    }
        //}

        //private static byte[] Combine(byte[] a, byte[] b)
        //{
        //    byte[] c = new byte[a.Length + b.Length];
        //    Buffer.BlockCopy(a, 0, c, 0, a.Length);
        //    Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
        //    return c;
        //}
    }
}
