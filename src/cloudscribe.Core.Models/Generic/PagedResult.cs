//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.Models.Generic
//{
//    public class PagedResult<T> where T : class
//    {
//        [Obsolete("this class is obsolete and will be removed in a future release. Please use cloudscribe.Pagination.Models.PagedResult instead")]
//        public PagedResult()
//        {
//            Data = new List<T>();
//        }
//        public List<T> Data { get; set; }
//        public int TotalItems { get; set; } = 0;
//    }
//}
