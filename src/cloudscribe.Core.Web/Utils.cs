// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2014-10-23
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Web;
using cloudscribe.Configuration;

namespace cloudscribe.Core.Web.Helpers
{
    public static class Utils
    {

        public static string GetIP4Address()
        {
            string ip4Address = string.Empty;
            if (HttpContext.Current == null) { return ip4Address; }
            if (HttpContext.Current.Request == null) { return ip4Address; }

            if (AppSettings.ClientIpServerVariable.Length > 0)
            {
                if (HttpContext.Current.Request.ServerVariables[AppSettings.ClientIpServerVariable] != null)
                {
                    return HttpContext.Current.Request.ServerVariables[AppSettings.ClientIpServerVariable];
                }
            }

            if (HttpContext.Current.Request.UserHostAddress == null) { return ip4Address; }

            try
            {
                IPAddress ip = IPAddress.Parse(HttpContext.Current.Request.UserHostAddress);
                if (ip.AddressFamily.ToString() == "InterNetwork") { return ip.ToString(); }
            }
            catch (FormatException)
            { }
            catch (ArgumentNullException) { }

            try
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        ip4Address = IPA.ToString();
                        break;
                    }
                }
            }
            catch (ArgumentException)
            { }
            catch (System.Net.Sockets.SocketException) { }

            if (ip4Address != string.Empty)
            {
                return ip4Address;
            }

            //this part makes no sense it would get the local server ip address
            try
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        ip4Address = IPA.ToString();
                        break;
                    }
                }
            }
            catch (ArgumentException)
            { }
            catch (System.Net.Sockets.SocketException) { }

            return ip4Address;
        }

        public static string CreateRandomPassword(int length, string allowedPasswordChars)
        {
            if (length == 0)
            {
                length = 7;
            }

            char[] allowedChars;
            if (string.IsNullOrEmpty(allowedPasswordChars))
            {
                allowedChars = "abcdefgijkmnopqrstwxyzABCDEFGHJKLMNPQRSTWXYZ23456789*$".ToCharArray();
            }
            else
            {
                allowedChars = allowedPasswordChars.ToCharArray();
            }
            char[] passwordChars = new char[length];
            byte[] seedBytes = new byte[4];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(seedBytes);

            int seed = (seedBytes[0] & 0x7f) << 24 |
                seedBytes[1] << 16 |
                seedBytes[2] << 8 |
                seedBytes[3];

            Random random = new Random(seed);

            for (int i = 0; i < length; i++)
            {
                passwordChars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }

            return new string(passwordChars);

        }

        public static List<string> SplitOnChar(this string s, char c)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(s)) { return list; }

            string[] a = s.Split(c);
            foreach (string item in a)
            {
                if (!string.IsNullOrEmpty(item)) { list.Add(item); }
            }


            return list;
        }

        public static List<string> SplitOnCharAndTrim(this string s, char c)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(s)) { return list; }

            string[] a = s.Split(c);
            foreach (string item in a)
            {
                if (!string.IsNullOrEmpty(item)) { list.Add(item.Trim()); }
            }


            return list;
        }

    }
}
