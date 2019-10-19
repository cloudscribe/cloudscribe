using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Localization
{
    /// <summary>
    /// determines culture by the first or second url segment, second for use with folder tenants
    /// </summary>
    public class UrlSegmentRequestCultureProvider : RequestCultureProvider
    {
        public UrlSegmentRequestCultureProvider(
            IList<CultureInfo> supportedUICultures,
            IList<CultureInfo> supportedCultures = null
            )
        {
            _supportedUICultures = supportedUICultures;
            _supportedCultures = supportedCultures;
        }

        private readonly IList<CultureInfo> _supportedUICultures;
        private readonly IList<CultureInfo> _supportedCultures;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var segments = GetSegments(httpContext.Request.Path);

            if (!string.IsNullOrWhiteSpace(segments.FirstSegment))
            {
                var matchingUICulture = _supportedUICultures.Where(x => 
                x.Name.Equals(segments.FirstSegment, StringComparison.InvariantCultureIgnoreCase) 
                || x.TwoLetterISOLanguageName.Equals(segments.FirstSegment, StringComparison.InvariantCultureIgnoreCase)
                || x.Name.Equals(segments.SecondSegment, StringComparison.InvariantCultureIgnoreCase)
                || x.TwoLetterISOLanguageName.Equals(segments.SecondSegment, StringComparison.InvariantCultureIgnoreCase)
                ).FirstOrDefault();

                CultureInfo mainCulture = null;
                if (_supportedCultures != null)
                {
                    mainCulture = _supportedCultures.Where(x => 
                    x.Name.Equals(segments.FirstSegment,StringComparison.InvariantCultureIgnoreCase)
                    || x.TwoLetterISOLanguageName.Equals(segments.FirstSegment, StringComparison.InvariantCultureIgnoreCase)
                    || x.Name.Equals(segments.SecondSegment, StringComparison.InvariantCultureIgnoreCase)
                    || x.TwoLetterISOLanguageName.Equals(segments.SecondSegment, StringComparison.InvariantCultureIgnoreCase)
                    ).FirstOrDefault();
                }
                if (matchingUICulture != null)
                {
                    if (mainCulture != null)
                    {
                        return Task.FromResult(new ProviderCultureResult(mainCulture.Name, matchingUICulture.Name));
                    }
                    return Task.FromResult(new ProviderCultureResult(matchingUICulture.Name, matchingUICulture.Name));
                }
            }

            //nothing matched
            return NullProviderCultureResult;

        }

        private UrlSegments GetSegments(string requestPath)
        {
            var result = new UrlSegments();

            if (string.IsNullOrEmpty(requestPath)) return result;
            if (!requestPath.Contains("/")) return result;

            var segments = SplitOnCharAndTrim(requestPath, '/');
            if(segments.Count > 0)
            {
                result.FirstSegment = segments[0];
            }

            if (segments.Count > 1)
            {
                result.SecondSegment = segments[1];
            }


            return result;

        }

        private string GetStartingSegment(string requestPath)
        {
            if (string.IsNullOrEmpty(requestPath)) return requestPath;
            if (!requestPath.Contains("/")) return requestPath;

            var segments = SplitOnCharAndTrim(requestPath, '/');
            return segments.FirstOrDefault();
        }

        private List<string> SplitOnCharAndTrim(string s, char c)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrWhiteSpace(s)) { return list; }

            string[] a = s.Split(c);
            foreach (string item in a)
            {
                if (!string.IsNullOrWhiteSpace(item)) { list.Add(item.Trim()); }
            }


            return list;
        }

        private class UrlSegments
        {
            public string FirstSegment { get; set; }
            public string SecondSegment { get; set; }
        }

    }
}
