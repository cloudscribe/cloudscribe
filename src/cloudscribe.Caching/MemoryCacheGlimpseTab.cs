using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using System.Runtime.Caching;
using cloudscribe.Configuration;

namespace cloudscribe.Caching
{
    //http://msdn.microsoft.com/en-us/library/system.runtime.caching.memorycache%28v=vs.110%29.aspx


    //http://msdn.microsoft.com/en-us/library/system.runtime.caching.memorycache.getenumerator%28v=vs.110%29.aspx
    // "Retrieving an enumerator for a MemoryCache instance is a resource-intensive and blocking operation. 
    // Therefore, the enumerator should not be used in production applications."


    public class MemoryCacheGlimpseTab : AspNetTab
    {
        public override object GetData(ITabContext context)
        {
            var result = new List<object>();

            if (AppSettings.Glimpse_AllowMemoryCacheEnumeration)
            {


                //http://stackoverflow.com/questions/23674798/is-it-possible-to-show-or-hide-certain-glimpse-tabs-on-a-per-request-basis


                //long itemCount = MemoryCache.Default.GetCount();

                IEnumerable items = (IEnumerable)MemoryCache.Default;


                //var httpContext = context.GetHttpContext();
                foreach (DictionaryEntry o in items)
                {

                    result.Add(new
                    {
                        Index = result.Count + 1,
                        Name = o.Key,
                        Type = o.Value.GetType().FullName
                    });
                }
            }
            else
            {
                string messageFormat = "There are {0} items in memory cache. Enumerating those items is disabled by the setting Glimpse_AllowMemoryCacheEnumeration";
                result.Add(new
                {
                    Index = result.Count + 1,
                    Name = string.Format(CultureInfo.InvariantCulture, messageFormat, MemoryCache.Default.GetCount()),
                    Type = string.Empty
                });

            }
            return result;
        }

        public override string Name
        {
            get { return "Memory Cache"; }
        }
    }

    //[Glimpse.Core.Extensibility.GlimpsePluginAttribute]
    //public class ApplicationCacheGlimpsePlugin : IGlimpsePlugin
    //{
    //    public object GetData(HttpContextBase context)
    //    {
    //        var data = new List<object[]> { new[] { "Key", "Value" } };
    //        foreach (DictionaryEntry item in context.Cache)
    //        {
    //            data.Add(new object[] { item.Key, item.Value });
    //        }
    //        return data;
    //    }

    //    public void SetupInit()
    //    {
    //    }

    //    public string Name
    //    {
    //        get { return "ApplicationCache"; }
    //    }
    //}
}
