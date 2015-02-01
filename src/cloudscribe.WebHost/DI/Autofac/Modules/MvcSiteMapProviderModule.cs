using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Reflection;
using Autofac;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Xml;

namespace cloudscribe.WebHost.DI.Autofac.Modules
{
    public class MvcSiteMapProviderModule
        : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            bool enableLocalization = true;
            string absoluteFileName = HostingEnvironment.MapPath("~/site.sitemap");
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
                (interfaceType, implementationType) => builder.RegisterType(implementationType).As(interfaceType).SingleInstance(),
                new Assembly[] { siteMapProviderAssembly },
                allAssemblies,
                excludeTypes,
                string.Empty);

// Multiple implementations of strategy based extension points (and not decorated with [ExcludeFromAutoRegistrationAttribute]).
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => builder.RegisterType(implementationType).As(interfaceType).SingleInstance(),
                multipleImplementationTypes,
                allAssemblies,
                excludeTypes,
                string.Empty);

// Registration of internal controllers
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => builder.RegisterType(implementationType).As(interfaceType).AsSelf().InstancePerDependency(),
                new Type[] { typeof(IController) },
                new Assembly[] { siteMapProviderAssembly },
                new Type[0],
                string.Empty);

// Visibility Providers
            builder.RegisterType<SiteMapNodeVisibilityProviderStrategy>()
                .As<ISiteMapNodeVisibilityProviderStrategy>()
                .WithParameter("defaultProviderName", "MvcSiteMapProvider.FilteredSiteMapNodeVisibilityProvider, MvcSiteMapProvider");

            

// Pass in the global controllerBuilder reference
            builder.RegisterInstance(ControllerBuilder.Current)
                   .As<ControllerBuilder>();

            builder.RegisterType<ControllerTypeResolverFactory>()
                .As<IControllerTypeResolverFactory>()
                .WithParameter("areaNamespacesToIgnore", new string[0]);

// Configure Security
            builder.RegisterType<AuthorizeAttributeAclModule>()
                .Named<IAclModule>("authorizeAttributeAclModule");
            builder.RegisterType<XmlRolesAclModule>()
                .Named<IAclModule>("xmlRolesAclModule");
            builder.RegisterType<CompositeAclModule>()
                .As<IAclModule>()
                .WithParameter(
                    (p, c) => p.Name == "aclModules",
                    (p, c) => new[]
                        {
                            c.ResolveNamed<IAclModule>("authorizeAttributeAclModule"),
                            c.ResolveNamed<IAclModule>("xmlRolesAclModule")
                        });









            builder.RegisterInstance(System.Runtime.Caching.MemoryCache.Default)
                   .As<System.Runtime.Caching.ObjectCache>();

            builder.RegisterGeneric(typeof(RuntimeCacheProvider<>))
                .As(typeof(ICacheProvider<>));

            builder.RegisterType<RuntimeFileCacheDependency>()
                .Named<ICacheDependency>("cacheDependency1")
                .WithParameter("fileName", absoluteFileName);

            builder.RegisterType<CacheDetails>()
                .Named<ICacheDetails>("cacheDetails1")
                .WithParameter("absoluteCacheExpiration", absoluteCacheExpiration)
                .WithParameter("slidingCacheExpiration", TimeSpan.MinValue)
                .WithParameter(
                    (p, c) => p.Name == "cacheDependency",
                    (p, c) => c.ResolveNamed<ICacheDependency>("cacheDependency1"));

// Configure the visitors
            builder.RegisterType<UrlResolvingSiteMapNodeVisitor>()
                   .As<ISiteMapNodeVisitor>();

// Prepare for our node providers
            builder.RegisterType<FileXmlSource>()
                .Named<IXmlSource>("xmlSource1")
                .WithParameter("fileName", absoluteFileName);

            builder.RegisterType<ReservedAttributeNameProvider>()
                .As<IReservedAttributeNameProvider>()
                .WithParameter("attributesToIgnore", new string[0]);


// Register the sitemap node providers
            builder.RegisterType<XmlSiteMapNodeProvider>()
                .Named<ISiteMapNodeProvider>("xmlSiteMapNodeProvider1")
                .WithParameter("includeRootNode", true)
                .WithParameter("useNestedDynamicNodeRecursion", false)
                .WithParameter(
                    (p, c) => p.Name == "xmlSource",
                    (p, c) => c.ResolveNamed<IXmlSource>("xmlSource1"));

            builder.RegisterType<ReflectionSiteMapNodeProvider>()
                .Named<ISiteMapNodeProvider>("reflectionSiteMapNodeProvider1")
                .WithParameter("includeAssemblies", includeAssembliesForScan)
                .WithParameter("excludeAssemblies", new string[0]);

            builder.RegisterType<CompositeSiteMapNodeProvider>()
                .Named<ISiteMapNodeProvider>("siteMapNodeProvider1")
                .WithParameter(
                    (p, c) => p.Name == "siteMapNodeProviders",
                    (p, c) => new[]
                        {
                            c.ResolveNamed<ISiteMapNodeProvider>("xmlSiteMapNodeProvider1"),
                            c.ResolveNamed<ISiteMapNodeProvider>("reflectionSiteMapNodeProvider1")
                        });

// Register the sitemap builders
            builder.RegisterType<SiteMapBuilder>()
                .Named<ISiteMapBuilder>("siteMapBuilder1")
                .WithParameter(
                    (p, c) => p.Name == "siteMapNodeProvider",
                    (p, c) => c.ResolveNamed<ISiteMapNodeProvider>("siteMapNodeProvider1"));

// Configure the builder sets
            builder.RegisterType<SiteMapBuilderSet>()
                   .Named<ISiteMapBuilderSet>("builderSet1")
                   .WithParameter("instanceName", "default")
                   .WithParameter("securityTrimmingEnabled", securityTrimmingEnabled)
                   .WithParameter("enableLocalization", enableLocalization)
                   .WithParameter("visibilityAffectsDescendants", visibilityAffectsDescendants)
                   .WithParameter("useTitleIfDescriptionNotProvided", useTitleIfDescriptionNotProvided)
                   .WithParameter(
                        (p, c) => p.Name == "siteMapBuilder",
                        (p, c) => c.ResolveNamed<ISiteMapBuilder>("siteMapBuilder1"))
                   .WithParameter(
                        (p, c) => p.Name == "cacheDetails",
                        (p, c) => c.ResolveNamed<ICacheDetails>("cacheDetails1"));

            builder.RegisterType<SiteMapBuilderSetStrategy>()
                .As<ISiteMapBuilderSetStrategy>()
                .WithParameter(
                    (p, c) => p.Name == "siteMapBuilderSets",
                    (p, c) => c.ResolveNamed<IEnumerable<ISiteMapBuilderSet>>("builderSet1"));
        }
    }
}

