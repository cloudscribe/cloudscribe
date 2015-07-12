// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-03
// Last Modified:			2014-11-03
// 

using System;

namespace cloudscribe.Core.Models.Geography
{
    public class Currency : ICurrency
    {

        public Currency()
        { }

        private Guid guid = Guid.Empty;

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        private string title = string.Empty;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string code = string.Empty;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string symbolLeft = string.Empty;

        public string SymbolLeft
        {
            get { return symbolLeft; }
            set { symbolLeft = value; }
        }
        private string symbolRight = string.Empty;

        public string SymbolRight
        {
            get { return symbolRight; }
            set { symbolRight = value; }
        }
        private string decimalPointChar = string.Empty;

        public string DecimalPointChar
        {
            get { return decimalPointChar; }
            set { decimalPointChar = value; }
        }
        private string thousandsPointChar = string.Empty;

        public string ThousandsPointChar
        {
            get { return thousandsPointChar; }
            set { thousandsPointChar = value; }
        }
        private string decimalPlaces = string.Empty;

        public string DecimalPlaces
        {
            get { return decimalPlaces; }
            set { decimalPlaces = value; }
        }
        private decimal valuation;

        public decimal Value
        {
            get { return valuation; }
            set { valuation = value; }
        }
        private DateTime lastModified = DateTime.UtcNow;

        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        private DateTime created = DateTime.UtcNow;

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

    }
}
