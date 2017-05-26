using cloudscribe.Web.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteTimeZoneService
    {
        public SiteTimeZoneService(
            ITimeZoneHelper timeZoneHelper,
            ITimeZoneIdResolver timeZoneIdResolver
            )
        {
            this.timeZoneHelper = timeZoneHelper;
            this.timeZoneIdResolver = timeZoneIdResolver;
        }

        private ITimeZoneHelper timeZoneHelper;
        private ITimeZoneIdResolver timeZoneIdResolver;
        private string currentTimeZoneId = null;


        public IReadOnlyCollection<string> GetTimeZoneList()
        {
            return timeZoneHelper.GetTimeZoneList();
        }

        public async Task<DateTime> ConvertToLocalTime(DateTime utcInput)
        {
            if(string.IsNullOrEmpty(currentTimeZoneId)) { currentTimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId(); }

            return timeZoneHelper.ConvertToLocalTime(utcInput, currentTimeZoneId);
        }
    }
}
