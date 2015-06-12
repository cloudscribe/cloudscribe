// Author:					Joe Audette
// Created:				    2015-05-20
// Last Modified:		    2015-05-20
// 

using Newtonsoft.Json;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.Models
{
    public class RecaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
