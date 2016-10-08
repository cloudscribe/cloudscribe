using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web
{
    public interface ISmsSender
    {
        Task SendSmsAsync(ISiteContext site, string number, string message);
    }
}
