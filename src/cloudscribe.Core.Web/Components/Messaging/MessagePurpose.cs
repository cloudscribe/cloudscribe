// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-01-19
// Last Modified:			2016-01-19
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class MessagePurpose
    {
        public const string ConfirmAccount = "ConfirmAccount";
        public const string SendSecurityCode = "SendSecurityCode";
        public const string PasswordReset = "PasswordReset";
        public const string AccountApproved = "AccountApproved";
    }
}
