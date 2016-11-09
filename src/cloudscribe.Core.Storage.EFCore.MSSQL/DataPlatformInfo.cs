// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2015-12-09
//	Last Modified:		    2015-12-09
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;

namespace cloudscribe.Core.Storage.EFCore.MSSQL
{
    public class DataPlatformInfo : IDataPlatformInfo
    {
        public string DBPlatform
        {
            get { return "Entity Framework with Microsoft SqlServer"; }
        }
    }
}
