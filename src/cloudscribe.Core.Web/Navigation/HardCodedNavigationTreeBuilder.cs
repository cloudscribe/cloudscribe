//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:					2015-07-07
//// Last Modified:			2015-07-17
//// 

//using cloudscribe.Web.Navigation;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.Web.Navigation
//{
//    /// <summary>
//    /// this class is useful for tests as a mock
//    /// it was also used when implementing json and xml builders
//    /// I was serializing the tree created here to test serialization and deserialization of the json and xml builders
//    /// </summary>
//    public class HardCodedNavigationTreeBuilder : INavigationTreeBuilder
//    {
//        public HardCodedNavigationTreeBuilder()
//        {

//        }

//        private TreeNode<NavigationNode> siteRoot = null;

//        //disable warning about not really being async
//        // we know it is not since it is hard coded building of the tree there is nothign to wait for
//#pragma warning disable 1998

//        public async Task<TreeNode<NavigationNode>> GetTree()
//        {
//            // ultimately we will need to cache sitemap per site

//            if (siteRoot == null)
//            {
//                siteRoot = BuildTree();
//            }

//            return siteRoot;
//        }

//#pragma warning restore 1998

//        private TreeNode<NavigationNode> BuildTree()
//        {
            
//            NavigationNode home = new NavigationNode();
//            home.Key = "Home";
//            home.ParentKey = "RootNode";
//            home.Controller = "Home";
//            home.Action = "Index";
//            home.Text = "Home";
//            home.Url = home.ResolveUrl();
//            home.IsRootNode = true;
//            TreeNode<NavigationNode> treeRoot = new TreeNode<NavigationNode>(home);

//            NavigationNode about = new NavigationNode();
//            about.Key = "About";
//            about.ParentKey = "RootNode";
//            about.Controller = "Home";
//            about.Action = "About";
//            about.Text = "About";
//            about.Url = about.ResolveUrl();
//            treeRoot.AddChild(about);

//            NavigationNode contact = new NavigationNode();
//            contact.Key = "Contact";
//            contact.ParentKey = "RootNode";
//            contact.Controller = "Home";
//            contact.Action = "Contact";
//            contact.Text = "Contact";
//            contact.Url = contact.ResolveUrl();
//            treeRoot.AddChild(contact);


//            NavigationNode siteAdmin = new NavigationNode();
//            siteAdmin.Key = "SiteAdmin";
//            siteAdmin.ParentKey = "RootNode";
//            siteAdmin.Controller = "SiteAdmin";
//            siteAdmin.Action = "Index";
//            siteAdmin.Text = "Administration";
//            siteAdmin.ViewRoles = "Admins,Content Administrators";
//            siteAdmin.Url = siteAdmin.ResolveUrl();
//            TreeNode<NavigationNode> adminRoot = treeRoot.AddChild(siteAdmin);

//            NavigationNode siteSettings = new NavigationNode();
//            siteSettings.Key = "BasicSettings";
//            siteSettings.ParentKey = "SiteAdmin";
//            siteSettings.Controller = "SiteAdmin";
//            siteSettings.Action = "SiteInfo";
//            siteSettings.Text = "Site Settings";
//            siteSettings.ViewRoles = "Admins,Content Administrators";
//            siteSettings.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree; 
//            siteSettings.PreservedRouteParameters = "siteGuid";
//            siteSettings.Url = siteSettings.ResolveUrl();
//            TreeNode<NavigationNode> siteT = adminRoot.AddChild(siteSettings);

//            NavigationNode hosts = new NavigationNode();
//            hosts.Key = "SiteHostMappings";
//            hosts.ParentKey = "BasicSettings";
//            hosts.Controller = "SiteAdmin";
//            hosts.Action = "SiteHostMappings";
//            hosts.Text = "Domain Mappings";
//            hosts.ViewRoles = "Admins,Content Administrators";
//            hosts.ComponentVisibility = NamedNavigationFilters.Breadcrumbs;
//            hosts.PreservedRouteParameters = "siteGuid";
//            hosts.Url = hosts.ResolveUrl();
//            TreeNode<NavigationNode> hostsT = siteT.AddChild(hosts);

//            NavigationNode siteList = new NavigationNode();
//            siteList.Key = "SiteList";
//            siteList.ParentKey = "SiteAdmin";
//            siteList.Controller = "SiteAdmin";
//            siteList.Action = "SiteList";
//            siteList.Text = "SiteList";
//            siteList.ViewRoles = "ServerAdmins";
//            siteList.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree;
//            siteList.Url = siteList.ResolveUrl();
//            TreeNode<NavigationNode> siteListT = adminRoot.AddChild(siteList);

//            NavigationNode newSite = new NavigationNode();
//            newSite.Key = "NewSite";
//            newSite.ParentKey = "SiteList";
//            newSite.Controller = "SiteAdmin";
//            newSite.Action = "NewSite";
//            newSite.Text = "NewSite";
//            newSite.ViewRoles = "ServerAdmins";
//            newSite.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree;
//            newSite.Url = newSite.ResolveUrl();
//            TreeNode<NavigationNode> newSiteT = siteListT.AddChild(newSite);


//            NavigationNode userAdmin = new NavigationNode();
//            userAdmin.Key = "UserAdmin";
//            userAdmin.ParentKey = "SiteAdmin";
//            userAdmin.Controller = "UserAdmin";
//            userAdmin.Action = "Index";
//            userAdmin.Text = "UserManagement";
//            userAdmin.ViewRoles = "ServerAdmins";
//            userAdmin.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree;
//            userAdmin.Url = userAdmin.ResolveUrl();
//            TreeNode<NavigationNode> userAdminT = adminRoot.AddChild(userAdmin);

//            NavigationNode newUser = new NavigationNode();
//            newUser.Key = "UserEdit";
//            newUser.ParentKey = "UserAdmin";
//            newUser.Controller = "UserAdmin";
//            newUser.Action = "UserEdit";
//            newUser.Text = "NewUser";
//            newUser.ViewRoles = "Admins";
//            newUser.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree;
//            newUser.Url = newUser.ResolveUrl();
//            TreeNode<NavigationNode> newUserT = userAdminT.AddChild(newUser);

//            NavigationNode userSearch = new NavigationNode();
//            userSearch.Key = "UserSearch";
//            userSearch.ParentKey = "UserAdmin";
//            userSearch.Controller = "UserAdmin";
//            userSearch.Action = "Search";
//            userSearch.Text = "User Search";
//            userSearch.ViewRoles = "Admins";
//            userSearch.ComponentVisibility = NamedNavigationFilters.Breadcrumbs;
//            userSearch.Url = userSearch.ResolveUrl();
//            TreeNode<NavigationNode> userSearchT = userAdminT.AddChild(userSearch);

//            NavigationNode ipSearch = new NavigationNode();
//            ipSearch.Key = "IpSearch";
//            ipSearch.ParentKey = "UserAdmin";
//            ipSearch.Controller = "UserAdmin";
//            ipSearch.Action = "IpSearch";
//            ipSearch.Text = "IpSearch";
//            ipSearch.ViewRoles = "Admins";
//            ipSearch.ComponentVisibility = NamedNavigationFilters.Breadcrumbs;
//            ipSearch.Url = ipSearch.ResolveUrl();
//            TreeNode<NavigationNode> ipSearchT = userAdminT.AddChild(ipSearch);


//            NavigationNode roleAdmin = new NavigationNode();
//            roleAdmin.Key = "RoleAdmin";
//            roleAdmin.ParentKey = "SiteAdmin";
//            roleAdmin.Controller = "RoleAdmin";
//            roleAdmin.Action = "Index";
//            roleAdmin.Text = "RoleManagement";
//            roleAdmin.ViewRoles = "Admins";
//            roleAdmin.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree;
//            roleAdmin.Url = roleAdmin.ResolveUrl();
//            TreeNode<NavigationNode> roleAdminT = adminRoot.AddChild(roleAdmin);

//            // TODO: this one should not be in main or child menus
//            // we can't have just one url since it depends on roleId
//            // but we do want it to appear ar the active breadcrumb
//            NavigationNode roleMembers = new NavigationNode();
//            roleMembers.Key = "RoleMembers";
//            roleMembers.ParentKey = "RoleAdmin";
//            roleMembers.Controller = "RoleAdmin";
//            roleMembers.Action = "RoleMembers";
//            roleMembers.Text = "RoleMembers";
//            roleMembers.ViewRoles = "Admins";
//            roleMembers.ComponentVisibility = NamedNavigationFilters.Breadcrumbs;
//            roleMembers.Url = roleMembers.ResolveUrl();
//            roleMembers.PreservedRouteParameters = "roleId,pageNumber,pageSize";
//            TreeNode<NavigationNode> roleMembersT = roleAdminT.AddChild(roleMembers);

//            NavigationNode roleEdit = new NavigationNode();
//            roleEdit.Key = "RoleEdit";
//            roleEdit.ParentKey = "RoleAdmin";
//            roleEdit.Controller = "RoleAdmin";
//            roleEdit.Action = "RoleEdit";
//            roleEdit.Text = "NewRole";
//            roleEdit.ViewRoles = "Admins";
//            roleEdit.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree;
//            roleEdit.Url = roleEdit.ResolveUrl();
//            roleEdit.PreservedRouteParameters = "roleIde";
//            TreeNode<NavigationNode> roleEditT = roleAdminT.AddChild(roleEdit);


//            NavigationNode coreData = new NavigationNode();
//            coreData.Key = "CoreData";
//            coreData.ParentKey = "SiteAdmin";
//            coreData.Controller = "CoreData";
//            coreData.Action = "Index";
//            coreData.Text = "CoreData";
//            coreData.ViewRoles = "ServerAdmins";
//            coreData.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree;
//            coreData.Url = coreData.ResolveUrl();
//            TreeNode<NavigationNode> coreDataT = adminRoot.AddChild(coreData);

//            NavigationNode currencyList = new NavigationNode();
//            currencyList.Key = "CurrencyList";
//            currencyList.ParentKey = "SiteAdmin";
//            currencyList.Controller = "CoreData";
//            currencyList.Action = "CurrencyList";
//            currencyList.Text = "CurrencyAdministration";
//            currencyList.ViewRoles = "ServerAdmins";
//            currencyList.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree;
//            currencyList.Url = currencyList.ResolveUrl();
//            TreeNode<NavigationNode> currencyListT = coreDataT.AddChild(currencyList);

//            //TODO: again I think we just want to be a breadcrumb here
//            NavigationNode currencyEdit = new NavigationNode();
//            currencyEdit.Key = "CurrencyEdit";
//            currencyEdit.ParentKey = "CurrencyList";
//            currencyEdit.Controller = "CoreData";
//            currencyEdit.Action = "CurrencyEdit";
//            currencyEdit.Text = "NewCurrency";
//            currencyEdit.ViewRoles = "ServerAdmins";
//            currencyEdit.ComponentVisibility = NamedNavigationFilters.Breadcrumbs;
//            currencyEdit.Url = currencyEdit.ResolveUrl();
//            currencyEdit.PreservedRouteParameters = "currencyGuid";
//            TreeNode<NavigationNode> currencyEditT = currencyListT.AddChild(currencyEdit);


//            NavigationNode countryList = new NavigationNode();
//            countryList.Key = "CountryListPage";
//            countryList.ParentKey = "SiteAdmin";
//            countryList.Controller = "CoreData";
//            countryList.Action = "CountryListPage";
//            countryList.Text = "CountryStateAdministration";
//            countryList.ViewRoles = "ServerAdmins";
//            countryList.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree;
//            countryList.Url = countryList.ResolveUrl();
//            TreeNode<NavigationNode> countryListT = coreDataT.AddChild(countryList);

//            NavigationNode countryEdit = new NavigationNode();
//            countryEdit.Key = "CountryEdit";
//            countryEdit.ParentKey = "CountryListPage";
//            countryEdit.Controller = "CoreData";
//            countryEdit.Action = "CountryEdit";
//            countryEdit.Text = "NewCountry";
//            countryEdit.ViewRoles = "ServerAdmins";
//            countryEdit.ComponentVisibility = NamedNavigationFilters.Breadcrumbs + "," + NamedNavigationFilters.ChildTree;
//            countryEdit.Url = countryEdit.ResolveUrl();
//            countryEdit.PreservedRouteParameters = "guid";
//            TreeNode<NavigationNode> countryEditT = countryListT.AddChild(countryEdit);

//            NavigationNode stateList = new NavigationNode();
//            stateList.Key = "StateListPage";
//            stateList.ParentKey = "CountryListPage";
//            stateList.Controller = "CoreData";
//            stateList.Action = "StateListPage";
//            stateList.Text = "States";
//            stateList.ViewRoles = "ServerAdmins";
//            stateList.ComponentVisibility = NamedNavigationFilters.Breadcrumbs;
//            stateList.Url = stateList.ResolveUrl();
//            stateList.PreservedRouteParameters = "countryGuid";
//            TreeNode<NavigationNode> stateListT = countryListT.AddChild(stateList);

//            NavigationNode stateEdit = new NavigationNode();
//            stateEdit.Key = "StateEdit";
//            stateEdit.ParentKey = "StateListPage";
//            stateEdit.Controller = "CoreData";
//            stateEdit.Action = "StateEdit";
//            stateEdit.Text = "New State";
//            stateEdit.ViewRoles = "ServerAdmins";
//            stateEdit.ComponentVisibility = NamedNavigationFilters.Breadcrumbs;
//            stateEdit.Url = stateEdit.ResolveUrl();
//            stateEdit.PreservedRouteParameters = "countryGuid";
//            TreeNode<NavigationNode> stateEditT = stateListT.AddChild(stateEdit);



//            //string serialized = JsonConvert.SerializeObject(treeRoot,Formatting.Indented);


//            return treeRoot;
//        }

        



//    }

    
//}
