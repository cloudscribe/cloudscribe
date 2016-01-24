// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2016-01-24
// Last Modified:		    2016-01-24
// 

using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace cloudscribe.Messaging.Sms
{
    public class TwilioSmsSender
    {
        public TwilioSmsSender(
            TwilioSmsCredentials credentials,
            ILogger logger)
        {
            this.credentials = credentials;
            log = logger;
        }

        private TwilioSmsCredentials credentials;
        private ILogger log;

        /// <summary>
        /// Send an sms message using Twilio REST API
        /// </summary>
        /// <param name="toPhoneNumber">E.164 formatted phone number, e.g. +16175551212</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> SendMessage(string toPhoneNumber, string message)
        {
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = CreateBasicAuthenticationHeader(
                credentials.AccountSid, 
                credentials.AuthToken);

            var content = new StringContent(string.Format(
                CultureInfo.InvariantCulture,
                "From={0}&amp;To={1}&amp;Body={2}", 
                credentials.FromNumber, 
                toPhoneNumber, 
                message));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync(
                string.Format(
                    CultureInfo.InvariantCulture,
                    credentials.SmsEndpointUrlFormat, 
                    credentials.AccountSid), content);

            if (response.IsSuccessStatusCode)
            {
                //the POST succeeded
                log.LogDebug("success sending sms message to " + toPhoneNumber);
                return true;
            }
            else
            {
                //the POST failed
                log.LogWarning("failed to send sms message " + response.ReasonPhrase);
                return false;
            }
            
        }

        private AuthenticationHeaderValue CreateBasicAuthenticationHeader(string username, string password)
        {
            return new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(
                     string.Format("{0}:{1}", username, password)
                     )
                 )
            );
        }


    }
}
