
ASP.NET Core - Feature Slices for ASP.NET Core MVC
https://msdn.microsoft.com/magazine/mt763233


Walkthrough: Organizing an ASP.NET MVC Application using Areas
.NET Framework 4

The MVC pattern separates the model (data) logic of an application from its presentation logic and business logic. In ASP.NET MVC, this logical separation is also implemented physically in the project structure, where controllers and views are kept in folders that use naming conventions to define relationships. This structure supports the needs of most Web applications.

However, some applications can have a large number of controllers, and each controller can be associated with several views. For these types of applications, the default ASP.NET MVC project structure can become unwieldy.

To accommodate large projects, ASP.NET MVC lets you partition Web applications into smaller units that are referred to as areas. Areas provide a way to separate a large MVC Web application into smaller functional groupings. An area is effectively an MVC structure inside an application. An application could contain several MVC structures (areas).

For example, a single large e-commerce application might be divided into areas that represent the storefront, product reviews, user account administration, and the purchasing system. Each area represents a separate function of the overall application.

This walkthrough demonstrates how to implement areas in an ASP.NET MVC application. The walkthrough creates the functional framework for a blog site that has the following areas:

    Main. This is entry point to the Web application. This area includes the landing page and a log-in feature.

    Blog. This area is used to display blog posts and to search the archive.

    Dashboard. This area is used to create and edit blog posts.

To keep this tutorial simple, the areas do not contain logic to perform the actual tasks for the blog.

A Visual Studio project with source code is available to accompany this topic: Download.
https://msdn.microsoft.com/en-us/library/ee671793%28v=vs.100%29.aspx

http://stackoverflow.com/questions/7271592/mvc3-when-to-use-areas

http://arunendapally.com/post/how-to-use-a-mvc-area-in-multiple-applications

public static void RegisterRoutes(RouteCollection routes)
{
  routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
 
  routes.MapRoute(
      name: "Default",
      url: "{controller}/{action}/{id}",
      defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
      namespaces: new string[] { "MainApplication1.Controllers" }
  );
}

19. Similarly go to BlogAppAreaRegistration.cs in MainApplication1 > Areas > BlogApp and add namespace as shown below.

public override void RegisterArea(AreaRegistrationContext context)
{
  context.MapRoute(
      "Blog_default",
      "Blog/{controller}/{action}/{id}",
      new { action = "Index", id = UrlParameter.Optional },
      new string[] { "BlogApp.Controllers" }
  );
}


http://www.strathweb.com/2015/04/asp-net-mvc-6-discovers-controllers/

10 good practices for mvc
http://www.codemag.com/Article/1405071

http://www.veryofficial.com/logic/programming/nested-mvc-controllers-and-custom-routes/

http://stackoverflow.com/questions/8167853/performing-search-in-asp-net-mvc

http://www.mikesdotnetting.com/article/256/entity-framework-6-recipe-alphabetical-paging-in-asp-net-mvc

http://forums.asp.net/t/1649100.aspx?alphabetical+pager+helper


some arguments against using a base controller
http://stackoverflow.com/questions/6119206/what-are-good-candidates-for-base-controller-class-in-asp-net-mvc

seems like a lot of what could be done in a base controller could also be done in extension methods

when you do use an extension method from inside a controller you have to use the this keyword to call the method, no problem but noteworthy becuase it is a little unexpected when the extension method is not found without the this keyword and compilation fails
http://stackoverflow.com/questions/12105869/controller-extension-method-without-this

I started doing alerts inside a base controller as in this article:
http://jameschambers.com/2014/06/day-14-bootstrap-alerts-and-mvc-framework-tempdata/

but then realized I could refactor that functionality as extension methods so they could be used from any controller without inheriting a base class

ASP.NET MVC 5 Authentication Filters
http://www.dotnetcurry.com/showarticle.aspx?ID=957

ASP.NET Core - Real-World ASP.NET Core MVC Filters
https://msdn.microsoft.com/en-us/magazine/mt767699.aspx