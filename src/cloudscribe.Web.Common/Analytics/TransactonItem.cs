using System;
using System.Collections.Generic;
using System.Text;

//https://developers.google.com/analytics/devguides/collection/analyticsjs/ecommerce

namespace cloudscribe.Web.Common.Analytics
{
    public class TransactonItem
    {
        public TransactonItem(string transactionId, string name)
        {
            if(string.IsNullOrWhiteSpace(transactionId)) { throw new ArgumentException("transactionid is required and cannnot be empty"); }
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("name is required and cannnot be empty"); }

            TransactionId = transactionId;
            Name = name;

        }

        public string TransactionId { get; set; } // Transaction ID. Required.
        public string Name { get; set; } // Product name. Required.
        public string Sku { get; set; } // SKU/code.
        public string Category { get; set; } // Category or variation.
        public decimal Price { get; set; }  // Unit price.
        public int Quantity { get; set; } = 1;
        /// <summary>
        /// https://support.google.com/analytics/answer/6205902#supported-currencies
        /// </summary>
        public string CurrencyCode { get; set; } = "USD";
    }
}
