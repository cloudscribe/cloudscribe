// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-19
// Last Modified:           2016-02-19
// 

using Microsoft.AspNet.Http;
using Microsoft.AspNet.Razor.TagHelpers;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace cloudscribe.Web.Common.TagHelpers
{
    /// <summary>
    /// <cs-gravatar email="@User.GetEmail()" size="30"></cs-gravatar>
    /// </summary>
    [HtmlTargetElement("cs-gravatar", Attributes = EmailAttributeName)]
    public class GravatarTagHelper : TagHelper
    {
        public GravatarTagHelper(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        protected internal IHttpContextAccessor contextAccessor { get; set; }

        private const string EmailAttributeName = "email";
        private const string SizeAttributeName = "size";
        private const string DefaultAttributeName = "default-image";
        private const string RatingAttributeName = "rating";

        private const string httpEndpointFormat = "http://gravatar.com/avatar/{0}?s={1}&d={2}&r={3}";
        private const string httpsEndpointFormat = "https://secure.gravatar.com/avatar/{0}?s={1}&d={2}&r={3}";

        [HtmlAttributeName(EmailAttributeName)]
        public string EmailAddress { get; set; }

        [HtmlAttributeName(SizeAttributeName)]
        public int Size { get; set; } = 80;

        [HtmlAttributeName(DefaultAttributeName)]
        public string DefaultImage { get; set; } = "mm";

        [HtmlAttributeName(RatingAttributeName)]
        public string Rating { get; set; } = "g";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //change the cs-pager element into a 
            output.TagName = "img";
            string emailHash;
            using (var md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(EmailAddress));

                
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }

                emailHash = sb.ToString();
            }

            var scheme = contextAccessor.HttpContext.Request.Scheme;
            string format;
            if (scheme == "https")
            {
                format = httpsEndpointFormat;
            }
            else
            {
                format = httpEndpointFormat;
            }

            var url = string.Format(
                CultureInfo.InvariantCulture,
                format,
                emailHash,
                Size,
                DefaultImage,
                Rating
                );

            output.Attributes.Clear();

            output.Attributes.Add("src", url);
        }
    }
}
