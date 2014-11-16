// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2014-11-15
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.ViewModels
{
    public class PagingInfo
    {
        private int totalpages = 0;
        public int TotalPages 
        {
            get { return totalpages; }
            set { totalpages = value; } 
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
