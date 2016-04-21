// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-01-24
// Last Modified:           2016-02-03
// 

using System;
using System.Collections.Generic;
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
        public TwilioSmsSender(ILogger logger = null)
        {
            if(logger != null) { log = logger; }
            
        }

        
        private ILogger log = null;
        private const string TwilioSmsEndpointFormat
            = "https://api.twilio.com/2010-04-01/Accounts/{0}/Messages.json";

        /// <summary>
        /// Send an sms message using Twilio REST API
        /// </summary>
        /// <param name="credentials">TwilioSmsCredentials</param>
        /// <param name="toPhoneNumber">E.164 formatted phone number, e.g. +16175551212</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> SendMessage(
            TwilioSmsCredentials credentials,
            string toPhoneNumber, 
            string message)
        {
            if (string.IsNullOrWhiteSpace(toPhoneNumber))
            {
                throw new ArgumentException("toPhoneNumber was not provided");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("message was not provided");
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = CreateBasicAuthenticationHeader(
                credentials.AccountSid, 
                credentials.AuthToken);

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("To", toPhoneNumber));
            keyValues.Add(new KeyValuePair<string, string>("From", credentials.FromNumber));
            keyValues.Add(new KeyValuePair<string, string>("Body", message));

            var content = new FormUrlEncodedContent(keyValues);
            
            var postUrl = string.Format(
                    CultureInfo.InvariantCulture,
                    TwilioSmsEndpointFormat,
                    credentials.AccountSid);

            var response = await client.PostAsync(
                postUrl, 
                content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                if(log != null)
                {
                    log.LogDebug("success sending sms message to " + toPhoneNumber);
                }
                
                return true;
            }
            else
            {
                if (log != null)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var logmessage = $"failed to send sms message to {toPhoneNumber} from {credentials.FromNumber} { response.ReasonPhrase } { responseBody }";
                    log.LogWarning(logmessage);
                }
                
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
