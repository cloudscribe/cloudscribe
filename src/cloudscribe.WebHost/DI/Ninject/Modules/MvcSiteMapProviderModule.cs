using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Hosting;
using Ninject;
using Ninject.Modules;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Xml;
using cloudscribe.Configuration;

namespace cloudscribe.WebHost.DI.Ninject.Modules
{
    /// <summary>
    /// JA: I could not get this working, I think there is a problem becuase of the owin startup
    /// and the timing of things
    /// with wiring it up manually in startup.cs it was throwing an error like it was not initialized
    /// 2015-01-04 this code is not a working solution and could be removed in the future
    /// </summary>
    public class MvcSiteMapProviderModule
        : NinjectModule
    {
        public override void Load()
        {
            bool enableLocalization = true;
            //string absoluteFileName = HostingEnvironment.MapPath("~/Mvc.sitemap");
            string absoluteFileName = HostingEnvironment.MapPath(AppSettings.MvcSiteMapProvider_SiteMapFileName);
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);
            bool visibilityAffectsDescendants = true;
            bool useTitleIfDescriptionNotProvided = true;





            bool securityTrimmingEnabled = true;
            string[] includeAssembliesForScan = new string[] { "cloudscribe.WebHost" };


            var currentAssembly = this.GetType().Assembly;
            var siteMapProviderAssembly = typeof(SiteMaps).Assembly;
            var allAssemblies = new Assembly[] { currentAssembly, siteMapProviderAssembly };
            var excludeTypes = new Type[] {
// Use this array to add types you wish to explicitly exclude from convention-based  
// auto-registration. By default all types that either match I[TypeName] = [TypeName] or 
// I[TypeName] = [TypeName]Adapter will be automatically wired up as long as they don't 
// have the [ExcludeFromAutoRegistrationAttribute].
//
// If you want to override a type that follows the convention, you should add the name 
// of either the implementation name or the interface that it inherits to this list and 
// add your manual registration code below. This will prevent duplicate registrations 
// of the types from occurring. 

// Example:
// typeof(SiteMap),
// typeof(SiteMapNodeVisibilityProviderStrategy)
            };
            var multipleImplementationTypes = new Type[] {
                typeof(ISiteMapNodeUrlResolver),
                typeof(ISiteMapNodeVisibilityProvider),
                typeof(IDynamicNodeProvider)
            };

// Matching type name (I[TypeName] = [TypeName]) or matching type name + suffix Adapter (I[TypeName] = [TypeName]Adapter)
// and not decorated with the [ExcludeFromAutoRegistrationAttribute].
            CommonConventions.RegisterDefaultConventions(
                (interfaceType, implementationType) => this.Kernel.Bind(interfaceType).To(implementationType).InSingletonScope(),
                new Assembly[] { siteMapProviderAssembly },
                allAssemblies,
                excludeTypes,
                string.Empty);

// Multiple implementations of strategy based extension points (and not decorated with [ExcludeFromAutoRegistrationAttribute]).
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => this.Kernel.Bind(interfaceType).To(implementationType).InSingletonScope(),
                multipleImplementationTypes,
                allAssemblies,
                excludeTypes,
                string.Empty);

            this.Kernel.Bind<ISiteMapNodeVisibilityProviderStrategy>().To<SiteMapNodeVisibilityProviderStrategy>()
                .WithConstructorArgument("defaultProviderName", string.Empty);

            this.Kernel.Bind<ControllerBuilder>().ToConstant(ControllerBuilder.Current);

            this.Kernel.Bind<IControllerTypeResolverFactory>().To<ControllerTypeResolverFactory>()
                .WithConstructorArgument("areaNamespacesToIgnore", new string[0]);

// Configure Security
            this.Kernel.Bind<AuthorizeAttributeAclModule>().ToSelf();
            this.Kernel.Bind<XmlRolesAclModule>().ToSelf();
            this.Kernel.Bind<IAclModule>().To<CompositeAclModule>()
                .WithConstructorArgument("aclModules",
                    new IAclModule[] {
                        this.Kernel.Get<AuthorizeAttributeAclModule>(),
                        this.Kernel.Get<XmlRolesAclModule>()
                    });


// Setup cache





            this.Kernel.Bind<System.Runtime.Caching.ObjectCache>()
                .ToConstant<System.Runtime.Caching.ObjectCache>(System.Runtime.Caching.MemoryCache.Default);
            this.Kernel.Bind(typeof(ICacheProvider<>)).To(typeof(RuntimeCacheProvider<>));
            this.Kernel.Bind<ICacheDependency>().To<RuntimeFileCacheDependency>().Named("cacheDependency1")
                .WithConstructorArgument("fileName", absoluteFileName);

            this.Kernel.Bind<ICacheDetails>().To<CacheDetails>().Named("cacheDetails1")
                .WithConstructorArgument("absoluteCacheExpiration", absoluteCacheExpiration)
                .WithConstructorArgument("slidingCacheExpiration", TimeSpan.MinValue)
                .WithConstructorArgument("cacheDependency", this.Kernel.Get<ICacheDependency>("cacheDependency1"));


// Configure the visitors
            this.Kernel.Bind<ISiteMapNodeVisitor>().To<UrlResolvingSiteMapNodeVisitor>();

// Prepare for our node providers
            this.Kernel.Bind<IXmlSource>().To<FileXmlSource>().Named("XmlSource1")
                .WithConstructorArgument("fileName", absoluteFileName);
            this.Kernel.Bind<IReservedAttributeNameProvider>().To<ReservedAttributeNameProvider>()
                .WithConstructorArgument("attributesToIgnore", new string[0]);

// Register the sitemap node providers
            this.Kernel.Bind<ISiteMapNodeProvider>().To<XmlSiteMapNodeProvider>().Named("xmlSiteMapNodeProvider1")
                .WithConstructorArgument("includeRootNode", true)
                .WithConstructorArgument("useNestedDynamicNodeRecursion", false)
                .WithConstructorArgument("xmlSource", this.Kernel.Get<IXmlSource>("XmlSource1"));

            this.Kernel.Bind<ISiteMapNodeProvider>().To<ReflectionSiteMapNodeProvider>().Named("reflectionSiteMapNodeProvider1")
                .WithConstructorArgument("includeAssemblies", includeAssembliesForScan)
                .WithConstructorArgument("excludeAssemblies", new string[0]);

            this.Kernel.Bind<ISiteMapNodeProvider>().To<CompositeSiteMapNodeProvider>().Named("siteMapNodeProvider1")
                .WithConstructorArgument("siteMapNodeProviders",
                    new ISiteMapNodeProvider[] {
                        this.Kernel.Get<ISiteMapNodeProvider>("xmlSiteMapNodeProvider1"),
                        this.Kernel.Get<ISiteMapNodeProvider>("reflectionSiteMapNodeProvider1")
                    });

// Register the sitemap builders
            this.Kernel.Bind<ISiteMapBuilder>().To<SiteMapBuilder>().Named("siteMapBuilder1")
                .WithConstructorArgument("siteMapNodeProvider", this.Kernel.Get<ISiteMapNodeProvider>("siteMapNodeProvider1"));

// Configure the builder sets
            this.Kernel.Bind<ISiteMapBuilderSet>().To<SiteMapBuilderSet>().Named("siteMapBuilderSet1")
                .WithConstructorArgument("instanceName", "default")
                .WithConstructorArgument("securityTrimmingEnabled", securityTrimmingEnabled)
                .WithConstructorArgument("enableLocalization", enableLocalization)
                .WithConstructorArgument("visibilityAffectsDescendants", visibilityAffectsDescendants)
                .WithConstructorArgument("useTitleIfDescriptionNotProvided", useTitleIfDescriptionNotProvided)
                .WithConstructorArgument("siteMapBuilder", this.Kernel.Get<ISiteMapBuilder>("siteMapBuilder1"))
                .WithConstructorArgument("cacheDetails", this.Kernel.Get<ICacheDetails>("cacheDetails1"));

            this.Kernel.Bind<ISiteMapBuilderSetStrategy>().To<SiteMapBuilderSetStrategy>()
                .WithConstructorArgument("siteMapBuilderSets",
                    new ISiteMapBuilderSet[] {
                        this.Kernel.Get<ISiteMapBuilderSet>("siteMapBuilderSet1")
                    });
        }
    }
}
