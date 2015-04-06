using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;

namespace cloudscribe.WebHost.DI.Autofac.Modules
{
    public class CloudscribeCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // although this is a central place to wire up dependencies for cloudscribe core
            // I decided to keep the repository mappings in Startup.cs to make them easier to find
            // since they are the most likely mappings that someone would want to change


            //builder.RegisterType<cloudscribe.Core.Web.Components.DoNothingStartupTrigger>()
            //    .As<cloudscribe.Core.Models.Site.ITriggerStartup>();

            builder.RegisterType<cloudscribe.Core.Web.Components.WebConfigStartupTrigger>()
                .As<cloudscribe.Core.Models.Site.ITriggerStartup>();
        }

    }
}