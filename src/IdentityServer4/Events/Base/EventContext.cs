// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace IdentityServer4.Events
{
    /// <summary>
    /// Contextual information included with every event.
    /// </summary>
    public class EventContext
    {
        /// <summary>
        /// Gets or sets the per-request activity identifier.
        /// </summary>
        /// <value>
        /// The activity identifier.
        /// </value>
        public string ActivityId { get; set; }
        
        /// <summary>
        /// Gets or sets the time stamp when the event was raised.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public DateTime TimeStamp { get; set; }
        
        /// <summary>
        /// Gets or sets the server process identifier.
        /// </summary>
        /// <value>
        /// The process identifier.
        /// </value>
        public int ProcessId { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the server machine.
        /// </summary>
        /// <value>
        /// The name of the machine.
        /// </value>
        public string MachineName { get; set; }
        
        /// <summary>
        /// Gets or sets the remote ip address of the current request.
        /// </summary>
        /// <value>
        /// The remote ip address.
        /// </value>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the subject identifier of the current user (if available).
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public string SubjectId { get; set; }
    }
}