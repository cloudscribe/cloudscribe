

viewcomponents tutorial
http://www.asp.net/vnext/overview/aspnet-vnext/vc



http://stackoverflow.com/questions/14762680/how-should-i-structure-my-view-model-asp-net-mvc


dropdown and cascading dropdown examples:

http://www.c-sharpcorner.com/UploadFile/3d39b4/casecading-dropdown-list-with-mvc-linq-to-sql-and-ajax/

http://nerddinnerbook.s3.amazonaws.com/Part6.htm

http://www.c-sharpcorner.com/UploadFile/4d9083/creating-simple-cascading-dropdownlist-in-mvc-4-using-razor/

http://www.c-sharpcorner.com/UploadFile/4b0136/working-with-dropdownlist-in-mvc-5/

How MVC 5 editor templates are selected
@Html.Editor()
@Html.EditorFor() -- same as above but strongly typed

There are built in mvc templates in the runtime for common data types
it is possible to override the template for built in html helpers and it is possible to implement custom templated helpers, and it is even possible to override the template used for custom ones
System templates are compiled into the runtime, but custom templates are just .cshtml files aka views and are searched for the same way views are.


See CoreViewEngine.cs to understand how views are searched
// {0} represents the name of the view
// {1} represents the name of the controller
// {2} represents the name of the area

ViewLocationFormats = new string[] {
    "~/Views/{1}/{0}.cshtml",
    "~/Views/Shared/{0}.cshtml",
    "~/Views/Sys/{1}/{0}.cshtml",
    "~/Views/Sys/Shared/{0}.cshtml"
};

views and editor templates are searched in the above locations and in the order shown from top to bottom, so the /Sys folders are the last ditch choices and higher priority is given to views in the upper folders.

others should not modify our views that are under /Sys folders, to customize they would copy it up to a higher priority folder
like /Views/Shared for example, then modify it

Sys views may be updated during upgrades therefore one may need to review any custom views for corresponding changes that might be needed or possible. We can provide release notes for any important changes to Sys views

Editor templates are searched in the following way according to Pro MVC 5 book
1. The template named passed to the helper Html.EditorFor(m => m.SomeProperty, "MyTemplate") would lead to MyTemplate.cshtml being used
2. Any template that is specified by meta data of the model using UIHint attribute
3. The template associated with the DataType attribute
4. Any template that corresponds to the .NET class name of the data type being processed
5. The built in String template if the data type is a simple type
6. Any template that corresponds ot the base classes of the data type
7. If the data type implements IEnumerable then the built in Collection template will be used
8. If all else fails the Object template will be used, subject to the rule that scaffolding is not recursive


example of Razor function
@helper is no longer supported but @function is
I used this function at first inside BootstrapTopNav.cshtml
but I didn't like it since it is not easy to change the template without editing C# code
so I managed to replace it with a partial view ChildDropdownPartial.cshtml which is called from BootstrapTopNav.cshtml, then called recursively 
from inside ChildDropdownPartial.cshtml


@functions {

    public string BuildDropDown(TreeNode<NavigationNode> node)
    {
        var sb = new StringBuilder();

        var ul = new TagBuilder("ul");
        ul.AddCssClass("dropdown-menu");
        ul.Attributes.Add("role", "menu");

        foreach (var childNode in node.Children)
        {
            if (!Model.ShouldAllowView(childNode)) { continue; }

            var listItem = new TagBuilder("li");

            if (childNode.Value.Text == "Separator")
            {
                listItem.AddCssClass("divider");
                sb.AppendLine(listItem.ToString(TagRenderMode.Normal));
                continue;
            }

            var link = new TagBuilder("a");
            link.Attributes.Add("href", Url.Content(childNode.Value.Url));

            if (!Model.HasVisibleChildren(childNode))
            {
                link.InnerHtml = childNode.Value.Text;
                listItem.InnerHtml = link.ToString(TagRenderMode.Normal);
            }
            else
            {
                link.InnerHtml = childNode.Value.Text;

                listItem.AddCssClass("dropdown-submenu");
                // recursion
                listItem.InnerHtml =
                    link.ToString(TagRenderMode.Normal)
                    + BuildDropDown(childNode)
                    ;
            }


            sb.AppendLine(listItem.ToString(TagRenderMode.Normal));
        }
        ul.InnerHtml = sb.ToString();
        return ul.ToString(TagRenderMode.Normal);
    }


        }

		
#Validation

http://www.codeproject.com/Tips/780992/Asp-Net-MVC-Custom-Compare-Data-annotation-with-Cl

https://github.com/ASP-NET-MVC/aspnetwebstack/blob/master/src/System.Web.Mvc/CompareAttribute.cs



            <input class="form-control valid" 
			data-val="true" 
			data-val-equalto="passwords must match" data-val-equalto-other="*.Password" 
			id="ConfirmPassword" name="ConfirmPassword" type="password">
            <span class="text-danger field-validation-valid" data-valmsg-for="ConfirmPassword" data-valmsg-replace="true"></span>
        