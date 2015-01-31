using System.Web.Mvc;
using Autofac;
using MvcSiteMapProvider.Web.Mvc;

namespace cloudscribe.WebHost.DI.Autofac.Modules
{
    public class MvcModule
        : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = typeof(MvcModule).Assembly;

            builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t => typeof(IController).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerDependency();
        }
    }
}
