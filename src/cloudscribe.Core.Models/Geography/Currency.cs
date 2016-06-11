// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2016-06-11
// 

using System;

namespace cloudscribe.Core.Models.Geography
{
    public class Currency : ICurrency
    {

        public Currency()
        {
            Id = Guid.NewGuid();
        }

        
        public Guid Id { get; set; } = Guid.Empty;

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

        private string cultureCode = string.Empty;
        public string CultureCode
        {
            get { return cultureCode ?? string.Empty; }
            set { cultureCode = value; }
        }

        // these props are not really used anywhere maybe we should drop them

        //private string symbolLeft = string.Empty;
        //public string SymbolLeft
        //{
        //    get { return symbolLeft ?? string.Empty; }
        //    set { symbolLeft = value; }
        //}

        //private string symbolRight = string.Empty;
        //public string SymbolRight
        //{
        //    get { return symbolRight ?? string.Empty; }
        //    set { symbolRight = value; }
        //}

        //private string decimalPointChar = string.Empty;
        //public string DecimalPointChar
        //{
        //    get { return decimalPointChar ?? string.Empty; }
        //    set { decimalPointChar = value; }
        //}

        //private string thousandsPointChar = string.Empty;
        //public string ThousandsPointChar
        //{
        //    get { return thousandsPointChar ?? string.Empty; }
        //    set { thousandsPointChar = value; }
        //}

        //private string decimalPlaces = string.Empty;
        //public string DecimalPlaces
        //{
        //    get { return decimalPlaces ?? string.Empty; }
        //    set { decimalPlaces = value; }
        //}

        //public decimal Value { get; set; } = 1;

        //public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
        //public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public static Currency FromICurrency(ICurrency icurrency)
        {
            Currency c = new Currency();
            c.Id = icurrency.Id;
            c.Title = icurrency.Title;
            c.Code = icurrency.Code;
            c.CultureCode = icurrency.CultureCode;
            //c.SymbolLeft = icurrency.SymbolLeft;
            //c.SymbolRight = icurrency.SymbolRight;
            //c.DecimalPointChar = icurrency.DecimalPointChar;
            //c.ThousandsPointChar = icurrency.ThousandsPointChar;
            //c.DecimalPlaces = icurrency.DecimalPlaces;
            //c.Value = icurrency.Value;
            //c.LastModifiedUtc = icurrency.LastModifiedUtc;
            //c.CreatedUtc = icurrency.CreatedUtc;

            return c;
        }


    }
}
