// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2015-07-06
//



namespace cloudscribe.Core.Web.ViewModels
{
    
    public class PaginationSettings
    {
        public int TotalItems { get; set; } = 0;
       
        public int ItemsPerPage { get; set; } = 10;
        
        public int CurrentPage { get; set; } = 1;
        
        public int MaxPagerItems { get; set; } = 10;

        public bool ShowFirstLast { get; set; } = false;

    }


    
}
