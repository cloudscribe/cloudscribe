
Some thingds such as Controllers, ViewComponents, and TagHelpers are not resolved by the container by default but instead by factories.
However this can be changed:
http://stackoverflow.com/documentation/asp.net-core/1949/dependency-injection/23212/resolve-controllers-viewcomponents-and-taghelpers-via-dependency-injection#t=201701121334489459167

http://josephwoodward.co.uk/2017/07/injecting-javascript-into-views-using-itaghelpercomponent

http://www.maherjendoubi.io/new-asp-net-core-feature-coming-to-1-1-better-integration-of-3rd-party-ioc-containers-in-startup-class/

http://stackoverflow.com/questions/37512862/asp-net-core-identitydbcontext-using-dependency-injection

http://odetocode.com/blogs/scott/archive/2016/02/18/avoiding-the-service-locator-pattern-in-asp-net-core.aspx

http://dotnetliberty.com/index.php/2016/04/11/asp-net-core-custom-service-based-on-request/

http://odetocode.com/blogs/scott/archive/2016/02/18/avoiding-the-service-locator-pattern-in-asp-net-core.aspx

http://autofac.readthedocs.org/en/latest/advanced/adapters-decorators.html



this question is about something else but it shows a cool technique if you nned to inejct something for use in a static extension method
thoug some would frown at the use of servicelocator pattern
http://stackoverflow.com/questions/30755827/getting-absolute-urls-using-asp-net-mvc-6

http://www.emadashi.com/2015/06/dependency-injection-in-asp-net-5-one-step-deeper/

http://www.strathweb.com/2014/07/dependency-injection-directly-actions-asp-net-web-api/


Since there will be built in dependency injection in aspnet vnext I think we will use that instead of ninject

http://blog.dudak.me/2014/custom-middleware-with-dependency-injection-in-asp-net-5/

http://blog.tonysneed.com/2011/10/09/using-nlog-with-dependency-injection/

http://stackoverflow.com/questions/6846342/how-to-inject-log4net-ilog-implementations-using-unity-2-0

http://davidkeaveny.blogspot.com/2011/03/unity-and-log4net.html

http://unity.codeplex.com/discussions/203744

http://rationalgeek.com/blog/introducing-ninjectautologging/#more-318

in vnext/aspnet 5 owin there is startup code like this:
 // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Configure the HTTP request pipeline.
            // Add the console logger.
            loggerfactory.AddConsole();

maybe we will be able to plugin log4net or other logger there?

http://stackoverflow.com/questions/7905110/logging-aspect-oriented-programming-and-dependency-injection-trying-to-make/7906547#7906547

http://blog.ploeh.dk/2010/09/20/InstrumentationwithDecoratorsandInterceptors/

very interesting discussion here
http://blog.ploeh.dk/2010/02/03/ServiceLocatorisanAnti-Pattern/
