Integrating MvcSiteMapProvider with Autofac
=========================================================================

To add MvcSiteMapProvider to your DI configuration,
add the following code to your composition root.

	// Create a container builder (typically part of your DI setup already)
	var builder = new ContainerBuilder();

	// Register modules
	builder.RegisterModule(new MvcSiteMapProviderModule()); // Required
	builder.RegisterModule(new MvcModule()); // Required by MVC. Typically already part of your setup (double check the contents of the module).

	// Create the DI container (typically part of your config already)
	var container = builder.Build();

	// Setup global sitemap loader (required)
    MvcSiteMapProvider.SiteMaps.Loader = container.Resolve<ISiteMapLoader>();

    // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider (optional)
    var validator = container.Resolve<ISiteMapXmlValidator>();
    validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));

    // Register the Sitemaps routes for search engines (optional)
    XmlSiteMapController.RegisterRoutes(RouteTable.Routes);

For more help consult the Autofac documentation at
http://code.google.com/p/autofac/wiki/StructuringWithModules


IMPORTANT: KEEPING YOUR DI CONFIGURATION UP TO DATE
=========================================================================

Making MvcSiteMapProvider depend on DI is a bit of a double-edged sword. 
While this makes MvcSiteMapProvider extremely easy to extend, it is 
possible that new features added to MvcSiteMapProvider will cause 
your existing DI configuration to break when doing an upgrade.

Unfortunately, NuGet doesn't have a way to automatically merge changes 
into your DI modules - if you have changed your configuration in any
way, the module will be skipped when you upgrade. But then, the purpose
of giving you this code is so you can change it. For this reason, 
when you upgrade your MvcSiteMapProvider version, you should also compare 
your DI module to the corresponding module in the master branch to see if 
there are any changes that need to be made to your configuration. The best 
way to do this is to use some kind of diff tool (such as Beyond Compare)
to highlight the differences and assist with bringing the changes into 
your configuration without overwriting your customizations.

Note that you don't need to merge in #if, #else, and #endif blocks inside 
of the module, but only the code between them that applies to your specific 
.NET and/or MVC version.

The latest module for Autofac is located at the following location:

https://github.com/maartenba/MvcSiteMapProvider/blob/master/src/MvcSiteMapProvider/CodeAsConfiguration/Autofac/DI/Autofac/Modules/MvcSiteMapProviderModule.cs
