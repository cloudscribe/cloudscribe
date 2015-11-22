// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2015-11-22
// 

using System;

namespace cloudscribe.Core.Models.Geography
{
    public class Currency : ICurrency
    {

        public Currency()
        { }

        
        public Guid Guid { get; set; } = Guid.Empty;
        public string Title { get; set; } = string.Empty;  
        public string Code { get; set; } = string.Empty;
        
        // these props are not really used anywhere maybe we should drop them
        public string SymbolLeft { get; set; } = string.Empty;
        public string SymbolRight { get; set; } = string.Empty;
        public string DecimalPointChar { get; set; } = string.Empty;
        public string ThousandsPointChar { get; set; } = string.Empty;
        public string DecimalPlaces { get; set; } = string.Empty;
        public decimal Value { get; set; } = 1;


        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public static Currency FromICurrency(ICurrency icurrency)
        {
            Currency c = new Currency();
            c.Guid = icurrency.Guid;
            c.Title = icurrency.Title;
            c.Code = icurrency.Code;

            c.SymbolLeft = icurrency.SymbolLeft;
            c.SymbolRight = icurrency.SymbolRight;
            c.DecimalPointChar = icurrency.DecimalPointChar;
            c.ThousandsPointChar = icurrency.ThousandsPointChar;
            c.DecimalPlaces = icurrency.DecimalPlaces;
            c.Value = icurrency.Value;

            return c;
        }


    }
}
