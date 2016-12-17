// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace IdentityServer4.Events
{
    /// <summary>
    /// Details class for signing certificate validation
    /// </summary>
    public class SigningCertificateDetail
    {
        /// <summary>
        /// Gets or sets the name of the signing certificate.
        /// </summary>
        /// <value>
        /// The name of the signing certificate.
        /// </value>
        public string SigningCertificateName  { get; set; }
        
        /// <summary>
        /// Gets or sets the signing certificate expiration.
        /// </summary>
        /// <value>
        /// The signing certificate expiration.
        /// </value>
        public DateTime SigningCertificateExpiration { get; set; }
    }
}