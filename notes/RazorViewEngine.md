
# New things in ASP.NET Core

http://developer.telerik.com/topics/net/building-reusable-ui-components-in-asp-net-core/

https://daveaglick.com/posts/the-bleeding-edge-of-razor

http://www.jeffreyfritz.com/2015/05/where-did-my-asp-net-bundles-go-in-asp-net-5/

http://weblogs.asp.net/imranbaloch/custom-viewengine-aspnet5-mvc6
http://www.jeffreyfritz.com/2015/05/customize-asp-net-mvc-6-view-location-easily/
http://stackoverflow.com/questions/27843749/mvc6-specify-custom-location-for-views-of-viewcomponents

http://docs.asp.net/en/toc/mvc/migration/migratingfrommvc5.html

http://www.davepaquette.com/archive/2015/05/11/cleaner-forms-using-tag-helpers-in-mvc6.aspx
http://www.davepaquette.com/archive/2015/12/28/complex-custom-tag-helpers-in-mvc-6.aspx

https://tuespetre.github.io/aspnetcore/performance/mvc/2016/08/01/aspnet-core-mvc-performance-squeeze.html

# General Tips

http://haacked.com/archive/2011/01/06/razor-syntax-quick-reference.aspx/

http://www.codeproject.com/Tips/256090/Add-namespaces-for-Razor-pages


# sitemap menus bread crumbs

See SiteMaps.txt

# modal dialogs

See modal-dialogs.txt


# cascading dropdowns

surprisingly hard to find a reuseable implementation of state/country dropdown.
something like this would be used in many places so an HtmlHelper method would seem like a good idea.
however, by MVC prinicples we would not want data access code/logic inside an HtmlHelper. If the helper is wiring up some js we could pass in json service url(s) to the helper.
Perhaps we could implement a cascading dropdown helper where you pass in an array of config objects for each of the related dropdowns each succcessive one refering to the id of the parent whose value would presumably be passed to the url for the child. ie the state dropdown would take the country code from the parent as a param for its service url.

At first my thoughts for state/country list were focused on cascading dropdown lists, but the more I think of it, it might be better implemented as auto-complete textboxes. ie the country textbox auto suggests from the known countries, the state textbox autocomplete would need to look for the value of the country textbox and pass that to its autocomplete url.
One benefit of this approach is it takes into account that our country/state database information may not be complete or up to date (actually I'm sure it is not complete or up to date except for US states). If the dropdown list does not have what the user is looking for it would be very frustrating. Allowing the user to provide the data directly but helping them reduce typing for known data would be ideal. The only downside is that we are no longer constricting the data and therefore it is possible to get wrong data input, but that is true of most data that we capture, certainly true for other parts of address data so why should we be so strict with state/country?

perhaps we could have helpers for both approaches in cloudscribe so one can choose based on the specific needs of the moment

**2015-05-15** I recently implemented re-useable generic solutions for cascading dropdown:
Scripts/cloudescribe-cascade-unobtrusive.js

you simply include the script and then decorate the markup with some data- attributes to wire it up and configure it.
See working example in Views/Sys/SiteAdmin/_BasicSettingsPartial.cshtml

and also implemented a genric re-useable autocomplete:
Scripts/cloudescribe-autosuggest-unobtrusive.js

however, Chrome browser puts its own auto complete on top making it unusable in some cases
http://stackoverflow.com/questions/29286244/jquery-ui-autocomplete-and-google-chrome-autocomplete
because google in their infinite wisdom refuse to respect autocomplete="off" attribute
http://stackoverflow.com/questions/12374442/chrome-browser-ignoring-autocomplete-off

usage example:

<div class="form-group">
	@Html.LabelFor(model => model.CompanyCountry, htmlAttributes: new { @class = "control-label col-md-2" })
	<div class="col-md-10">
		
		@Html.EditorFor(model => model.CompanyCountry, new { 
		htmlAttributes = new { 
		@class = "form-control", 
		@autocomplete = "off",
		@maxlength = "2",
		@data_autosuggest_serviceurl = Url.Content("~/CoreData/CountryAutoSuggestJson"),
		@data_autosuggest_label_prop = "Name",
		@data_autosuggest_value_prop = "ISOCode2"
		} })
		
		@Html.ValidationMessageFor(model => model.CompanyCountry, "", new { @class = "text-danger" })
	</div>
</div>
<div class="form-group">
	@Html.LabelFor(model => model.CompanyRegion, htmlAttributes: new { @class = "control-label col-md-2" })
	<div class="col-md-10">
		
		@Html.EditorFor(model => model.CompanyRegion, 
		new { htmlAttributes = new { 
		@class = "form-control" ,
		@autocomplete = "off",
		@maxlength = "7",
		@data_autosuggest_serviceurl = Url.Content("~/CoreData/StateAutoSuggestJson"),
		@data_autosuggest_label_prop = "Name",
		@data_autosuggest_value_prop = "Code",
		@data_autosuggest_parentid = "CompanyCountry",
		@data_autosuggest_parent_data_name = "countryCode"
		} })

		@Html.ValidationMessageFor(model => model.CompanyRegion, "", new { @class = "text-danger" })  
	</div>
</div>

@section Scripts {
    @Scripts.Render("~/bundles/jqueryui") 
    @Scripts.Render("~/Scripts/cloudscribe-autosuggest-unobtrusive.js")
    
}

## View Pre-compilation

https://github.com/aspnet/Mvc/issues/6021
https://github.com/aspnet/MvcPrecompilation/tree/rel/1.1.0/testapps/ClassLibraryWithPrecompiledViews
https://github.com/aspnet/MvcPrecompilation/blob/rel/1.1.0/testapps/ClassLibraryWithPrecompiledViews/ClassLibraryWithPrecompiledViews.csproj


other references:

good insights about taghelpers in the answer here
http://stackoverflow.com/questions/32922425/multiple-tag-helpers-targetting-the-same-element

http://blog.simontimms.com/2015/06/09/getting-lookup-data-into-you-view/

http://blogs.msdn.com/b/rickandy/archive/2012/01/09/cascasding-dropdownlist-in-asp-net-mvc.aspx

http://stackoverflow.com/questions/19145641/how-can-i-reuse-a-dropdownlist-in-several-views-with-net-mvc

http://stackoverflow.com/questions/4231144/asp-net-mvc-populate-commonly-used-dropdownlists

http://www.codeproject.com/Articles/730953/Cascading-Dropdown-List-With-MVC-LINQ-to-SQL-and-A

http://www.devcurry.com/2013/01/aspnet-mvc-cascading-dropdown-list.html

http://www.c-sharpcorner.com/UploadFile/4d9083/creating-simple-cascading-dropdownlist-in-mvc-4-using-razor/

http://www.c-sharpcorner.com/UploadFile/4b0136/working-with-dropdownlist-in-mvc-5/

http://www.c-sharpcorner.com/UploadFile/fba912/cascading-dropdownlist-box-in-Asp-Net-mvc4-using-json-jquer/

http://stackoverflow.com/questions/18173717/angularjs-trigger-country-state-dependency

http://softwareexperiences.blogspot.com/2014/05/angularjs-country-state-select-example.html

http://aspnetscript.com/asp-net/cascading-dropdownlist-asp-net-mvc-angularjs/

http://dzapart.blogspot.com/2013/04/auto-complete-controll-with-aspnet-mvc.html

http://www.c-sharpcorner.com/UploadFile/4d9083/creating-simple-cascading-dropdownlist-in-mvc-4-using-razor/







