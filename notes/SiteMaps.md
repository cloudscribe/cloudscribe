# sitemap menus bread crumbs
===========================

We are using this:
https://github.com/maartenba/MvcSiteMapProvider

3 ways to define nodes
--------------------------
1. sitemap files xml, see site.sitemap

2. Dynamic Node Providers
the xml nodes in the above file can also refer to DynamicNodeProviders that can generate nodes
https://github.com/maartenba/MvcSiteMapProvider/wiki/Defining-sitemap-nodes-using-IDynamicNodeProvider
we did this in cloudscribe.Core.Web.Navigation.AdminMenuDynamicNodeProvider.cs (which we may or may not keep)

definitely the dynamic provider approach will be needed for cms pages in the cloudscribe cms project

3. Nodes can also be defined on ActionMethods in a controller class using DataAnnotations:

[Authorize(Roles = "Admins,Role Admins")]
[MvcSiteMapNode(Title = "New Role", ParentKey = "Roles", Key = "RoleEdit")]
public async Task<ActionResult> RoleEdit(int? roleId)
{


This article really helped me understand mvcsitemapprovider
http://www.shiningtreasures.com/post/2013/09/02/how-to-make-mvcsitemapprovider-remember-a-user-position

http://stackoverflow.com/questions/20328189/how-to-localize-the-mvcsitemapprovider

<mvcSiteMapNode title="$resources:SiteMapLocalizations,BrowseGenresTitle" controller="Store" action="Index"/>

<mvcSiteMapNode title="$resources:CommonResources,UserManagement" controller="foo" action="bar"/>

based on ms implementation in WebForms SiteMapProvider
http://msdn.microsoft.com/en-us/library/ms178427%28v=vs.100%29.aspx

Seems it is limited to working with resx files that live in the /App_GlobalResources folder

this post talks about implementing a custom resource provider
http://stackoverflow.com/questions/3395009/localization-of-web-sitemap-using-a-resx-file-in-other-project

got a little foobarred because the mvcsitemapprovider ninject stuff wiored up a second instance on ninject
found solution here:
https://github.com/maartenba/MvcSiteMapProvider/issues/288
https://github.com/maartenba/MvcSiteMapProvider/blob/master/src/MvcSiteMapProvider/CodeAsConfiguration/Ninject/DI/Ninject/ReadMe.txt

ok research showed a way to do better localization in mvcsitemapprovider by plugging in a custom IStringLocalizer
but to do that requires changing to use external di for mvcsitemap itself
and with ninject combined with owin startup I could not get that working

so I'm going to ask upstream if they can make a way to plugin a custom type for IStringLocalizer without having to use external di.
from review of the code the place it is hard coded is at line 81 in SiteMapNodeFactoryContainer.cs:

private ILocalizationServiceFactory ResolveLocalizationServiceFactory()
{
	return new LocalizationServiceFactory(
		new ExplicitResourceKeyParser(),
		new StringLocalizer(this.mvcContextFactory));
}




http://stackoverflow.com/questions/21494075/breadcrumbs-in-c-sharp-mvc-website-using-bootstrap

http://stackoverflow.com/questions/18402684/create-breadcrumbs-path-in-asp-net-mvc


http://joeylicc.wordpress.com/2014/10/03/asp-net-mvc-5-menu-using-site-map-provider-bootstrap-3-navbar/

https://joeylicc.wordpress.com/2013/07/08/asp-net-mvc-sitemappath-using-site-map-provider-bootstrap-breadcrumbs/

https://github.com/maartenba/MvcSiteMapProvider/wiki/Advanced-Node-Visibility-with-ISiteMapNodeVisibilityProvider






