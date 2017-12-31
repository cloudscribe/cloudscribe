using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Web.Common.Analytics
{
    public class Transaction
    {
        public Transaction(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException("id is required and cannnot be empty"); }

            Id = id;
            Items = new List<TransactonItem>();
        }

        public string Id { get; set; } // Transaction ID. Required.
        public string Affilitation { get; set; } // Affiliation or store name.

        /// <summary>
        /// Specifies the total revenue or grand total associated with the transaction (e.g. 11.99). 
        /// This value may include shipping, tax costs, or other adjustments to total revenue that 
        /// you want to include as part of your revenue calculations.
        /// </summary>
        public decimal Revenue { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public List<TransactonItem> Items { get; set; }
        /// <summary>
        /// https://support.google.com/analytics/answer/6205902#supported-currencies
        /// </summary>
        public string CurrencyCode { get; set; } = "USD";

        public const string TempDataKey = "GoogleAnalyticsTransactions";
    }
}
