// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2015-01-07
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.ViewModels
{
    /// <summary>
    /// the mvcpager expects totalItems and it calculates totalPages
    /// since we already had data methods that provided totalPages as an output parameter
    /// they were willing to implement a flag to treat totalItems as totalPages so we could use the pager
    /// that worked and works, but as we try to change to async methods we find that output parameters are not allowed in async
    /// so we need to refactor our data access to eliminate the output params and just get the count in a separate call
    /// then we can also use the pager as it was intended with totalItems
    /// once all the data accces code has been updated we can remove the TotalPages property here
    /// </summary>
    public class PagingInfo
    {
        private int totalpages = 0;
        public int TotalPages 
        {
            get { return totalpages; }
            set { totalpages = value; } 
        }

        private int totalItems = 0;
        public int TotalItems
        {
            get { return totalItems; }
            set { totalItems = value; }
        }

        private int itemsPerPage = 10;
        public int ItemsPerPage 
        {
            get { return itemsPerPage; }
            set { itemsPerPage = value; } 
        }

        private int currentPage = 1;
        public int CurrentPage 
        {
            get { return currentPage; }
            set { currentPage = value; } 
        }

        private int maxPagerItems = 10;
        public int MaxPagerItems
        {
            get { return maxPagerItems; }
            set { maxPagerItems = value; }
        }
    }
}
