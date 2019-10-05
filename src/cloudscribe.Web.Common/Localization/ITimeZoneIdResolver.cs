//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace cloudscribe.Web.Common
//{
//    [Obsolete("Please use the cloudscribe.DateTimeUtils version of this interface")]
//    public interface ITimeZoneIdResolver
//    {
//        Task<string> GetUserTimeZoneId(CancellationToken cancellationToken = default(CancellationToken));
//        Task<string> GetSiteTimeZoneId(CancellationToken cancellationToken = default(CancellationToken));
//    }

//    public class GmtTimeZoneIdResolver : ITimeZoneIdResolver
//    {
//        public Task<string> GetUserTimeZoneId(CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return Task.FromResult("GMT");
//        }

//        public Task<string> GetSiteTimeZoneId(CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return Task.FromResult("GMT");
//        }

//    }
//}
