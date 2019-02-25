// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-19
// Last Modified:           2019-02-24
// 

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace cloudscribe.Web.Common.TagHelpers
{
    /// <summary>
    /// <img gravatar-email="@User.GetEmail()" gravatar-size="30" />
    /// </summary>
    [HtmlTargetElement("img", Attributes = EmailAttributeName)]
    public class GravatarTagHelper : TagHelper
    {
        public GravatarTagHelper(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        protected internal IHttpContextAccessor contextAccessor { get; set; }

        private const string EnableGravatarAttributeName = "gravatar-enabled";
        private const string EmailAttributeName = "gravatar-email";
        private const string OverrideUrlAttribute = "override-url";
        private const string DisabledDefaultUrlAttribute = "disabled-default-url";
        private const string SizeAttributeName = "gravatar-size";
        private const string DefaultAttributeName = "gravatar-default-image";
        private const string RatingAttributeName = "gravatar-rating";

        private const string httpEndpointFormat = "http://gravatar.com/avatar/{0}?s={1}&d={2}&r={3}";
        private const string httpsEndpointFormat = "https://secure.gravatar.com/avatar/{0}?s={1}&d={2}&r={3}";

        [HtmlAttributeName(EnableGravatarAttributeName)]
        public bool GravatarEnabled { get; set; } = true;

        [HtmlAttributeName(EmailAttributeName)]
        public string EmailAddress { get; set; }

        [HtmlAttributeName(OverrideUrlAttribute)]
        public string OverrideUrl { get; set; }

        [HtmlAttributeName(DisabledDefaultUrlAttribute)]
        public string DisabledDefaultUrl { get; set; } = "/cr/images/generic-user.png";

        [HtmlAttributeName(SizeAttributeName)]
        public int Size { get; set; } = 80;

        [HtmlAttributeName(DefaultAttributeName)]
        public string DefaultImage { get; set; } = "mm";

        [HtmlAttributeName(RatingAttributeName)]
        public string Rating { get; set; } = "g";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string url = null;
            string emailHash;
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

            if (GravatarEnabled && string.IsNullOrWhiteSpace(OverrideUrl))
            {
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
                
                url = string.Format(
                    CultureInfo.InvariantCulture,
                    format,
                    emailHash,
                    Size,
                    DefaultImage,
                    Rating
                    );
            }
            else
            {
                if(!string.IsNullOrWhiteSpace(OverrideUrl))
                {
                    url = OverrideUrl;
                }
                else
                {
                    url = DisabledDefaultUrl;
                }

                output.Attributes.Add("style", "height:" + Size.ToString() + "px;");

            }
            
            if(string.IsNullOrWhiteSpace(url))
            {
                output.SuppressOutput();
                return;
            }

            var att = new TagHelperAttribute("gravatar-email");
            output.Attributes.Remove(att);

            att = new TagHelperAttribute("override-url");
            output.Attributes.Remove(att);
            
            att = new TagHelperAttribute("gravatar-size");
            output.Attributes.Remove(att);
           
            att = new TagHelperAttribute("gravatar-default-image");
            output.Attributes.Remove(att);
            
            att = new TagHelperAttribute("gravatar-rating");
            output.Attributes.Remove(att);
          
            att = new TagHelperAttribute("disabled-default-url");
            output.Attributes.Remove(att);
            
            output.Attributes.Add("src", url);
        }
    }
}
