// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
namespace cloudscribe.Core.Models.Geography
{
    public interface ICurrency
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Code { get; set; }
        string CultureCode { get; set; }
        //DateTime CreatedUtc { get; set; }
        //string DecimalPlaces { get; set; }
        //string DecimalPointChar { get; set; }
        //DateTime LastModifiedUtc { get; set; }
        //string SymbolLeft { get; set; }
        //string SymbolRight { get; set; }
        //string ThousandsPointChar { get; set; }
        
        //decimal Value { get; set; }
    }
}
