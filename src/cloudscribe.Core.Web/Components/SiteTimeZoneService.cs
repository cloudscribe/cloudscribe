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
            _timeZoneHelper = timeZoneHelper;
            _timeZoneIdResolver = timeZoneIdResolver;
        }

        private ITimeZoneHelper _timeZoneHelper;
        private ITimeZoneIdResolver _timeZoneIdResolver;
        private string _currentTimeZoneId = null;


        public IReadOnlyCollection<string> GetTimeZoneList()
        {
            return _timeZoneHelper.GetTimeZoneList();
        }

        public async Task<DateTime> ConvertToLocalTime(DateTime utcInput)
        {
            if(string.IsNullOrEmpty(_currentTimeZoneId)) { _currentTimeZoneId = await _timeZoneIdResolver.GetUserTimeZoneId(); }

            return _timeZoneHelper.ConvertToLocalTime(utcInput, _currentTimeZoneId);
        }
    }
}
