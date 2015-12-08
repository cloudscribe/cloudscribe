// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2015-12-08
// 

using System;

namespace cloudscribe.Core.Models.Geography
{
    public class Currency : ICurrency
    {

        public Currency()
        { }

        
        public Guid Guid { get; set; } = Guid.Empty;

        private string title = string.Empty;
        public string Title
        {
            get { return title ?? string.Empty; }
            set { title = value; }
        }

        private string code = string.Empty;
        public string Code
        {
            get { return code ?? string.Empty; }
            set { code = value; }
        }

        // these props are not really used anywhere maybe we should drop them

        private string symbolLeft = string.Empty;
        public string SymbolLeft
        {
            get { return symbolLeft ?? string.Empty; }
            set { symbolLeft = value; }
        }

        private string symbolRight = string.Empty;
        public string SymbolRight
        {
            get { return symbolRight ?? string.Empty; }
            set { symbolRight = value; }
        }

        private string decimalPointChar = string.Empty;
        public string DecimalPointChar
        {
            get { return decimalPointChar ?? string.Empty; }
            set { decimalPointChar = value; }
        }

        private string thousandsPointChar = string.Empty;
        public string ThousandsPointChar
        {
            get { return thousandsPointChar ?? string.Empty; }
            set { thousandsPointChar = value; }
        }

        private string decimalPlaces = string.Empty;
        public string DecimalPlaces
        {
            get { return decimalPlaces ?? string.Empty; }
            set { decimalPlaces = value; }
        }
        
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
