// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-07
// Last Modified:			2015-07-11
// 


using cloudscribe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationTreeBuilder
    {
        public NavigationTreeBuilder()
        {

        }

        private TreeNode<NavigationNode> siteRoot = null;

        public TreeNode<NavigationNode> GetTree()
        {
            // ultimately we will need to cache sitemap per site

            if (siteRoot == null)
            {
                siteRoot = BuildTree();
            }

            return siteRoot;
        }

        private TreeNode<NavigationNode> BuildTree()
        {
            // note that from the Tree point of view we are using
            // the lighter NavigationNode class
            // but actually we are populating it with the richer class
            // SiteMapNode
            // basically get the larger object out of the way so it doesn't clutter
            // the more common useage scenarios
            // scenarios that need the richer object can cast it
            // or we could add extension methods to NavigationNode to access things from SiteMapNode where needed
            // we are making a conscious choice here that when we want to serialize-deserialze nodes
            // we are limiting to the properties on NavigationNode

            //SiteMapNode rootNode = new SiteMapNode();
            //rootNode.IsRootNode = true;
            //rootNode.Key = "RootNode";

           // TreeNode<NavigationNode> treeRoot = new TreeNode<NavigationNode>(rootNode);

            SiteMapNode home = new SiteMapNode();
            home.Key = "Home";
            home.ParentKey = "RootNode";
            home.Controller = "Home";
            home.Action = "Index";
            home.Text = "Home";
            home.Url = home.ResolveUrl();
            home.IsRootNode = true;
            // TreeNode<NavigationNode>  tHome = treeRoot.AddChild(home);
            TreeNode<NavigationNode> treeRoot = new TreeNode<NavigationNode>(home);

            SiteMapNode about = new SiteMapNode();
            about.Key = "About";
            about.ParentKey = "RootNode";
            about.Controller = "Home";
            about.Action = "About";
            about.Text = "About";
            about.Url = about.ResolveUrl();
            treeRoot.AddChild(about);

            SiteMapNode contact = new SiteMapNode();
            contact.Key = "Contact";
            contact.ParentKey = "RootNode";
            contact.Controller = "Home";
            contact.Action = "Contact";
            contact.Text = "Contact";
            contact.Url = contact.ResolveUrl();
            treeRoot.AddChild(contact);


            SiteMapNode siteAdmin = new SiteMapNode();
            siteAdmin.Key = "SiteAdmin";
            siteAdmin.ParentKey = "RootNode";
            siteAdmin.Controller = "SiteAdmin";
            siteAdmin.Action = "Index";
            siteAdmin.Text = "Administration";
            siteAdmin.ViewRoles = "Admins,Content Administrators";
            siteAdmin.Url = siteAdmin.ResolveUrl();
            TreeNode<NavigationNode> adminRoot = treeRoot.AddChild(siteAdmin);

            SiteMapNode siteSettings = new SiteMapNode();
            siteSettings.Key = "BasicSettings";
            siteSettings.ParentKey = "SiteAdmin";
            siteSettings.Controller = "SiteAdmin";
            siteSettings.Action = "SiteInfo";
            siteSettings.Text = "Site Settings";
            siteSettings.ViewRoles = "Admins,Content Administrators";
            siteSettings.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*"; //this pattern was used in mvcsitemapprovider may change
            siteSettings.PreservedRouteParameters = "siteGuid";
            siteSettings.Url = siteSettings.ResolveUrl();
            TreeNode<NavigationNode> siteT = adminRoot.AddChild(siteSettings);

            SiteMapNode hosts = new SiteMapNode();
            hosts.Key = "SiteHostMappings";
            hosts.ParentKey = "BasicSettings";
            hosts.Controller = "SiteAdmin";
            hosts.Action = "SiteHostMappings";
            hosts.Text = "Domain Mappings";
            hosts.ViewRoles = "Admins,Content Administrators";
            hosts.ComponentVisibility = "SiteMapPathHelper,!*";
            hosts.PreservedRouteParameters = "siteGuid";
            hosts.Url = hosts.ResolveUrl();
            TreeNode<NavigationNode> hostsT = siteT.AddChild(hosts);

            SiteMapNode siteList = new SiteMapNode();
            siteList.Key = "SiteList";
            siteList.ParentKey = "SiteAdmin";
            siteList.Controller = "SiteAdmin";
            siteList.Action = "SiteList";
            siteList.Text = "SiteList";
            siteList.ViewRoles = "ServerAdmins";
            siteList.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            siteList.Url = siteList.ResolveUrl();
            TreeNode<NavigationNode> siteListT = adminRoot.AddChild(siteList);

            SiteMapNode newSite = new SiteMapNode();
            newSite.Key = "NewSite";
            newSite.ParentKey = "SiteList";
            newSite.Controller = "SiteAdmin";
            newSite.Action = "NewSite";
            newSite.Text = "NewSite";
            newSite.ViewRoles = "ServerAdmins";
            newSite.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            newSite.Url = newSite.ResolveUrl();
            TreeNode<NavigationNode> newSiteT = siteListT.AddChild(newSite);


            SiteMapNode userAdmin = new SiteMapNode();
            userAdmin.Key = "UserAdmin";
            userAdmin.ParentKey = "SiteAdmin";
            userAdmin.Controller = "UserAdmin";
            userAdmin.Action = "Index";
            userAdmin.Text = "UserManagement";
            userAdmin.ViewRoles = "ServerAdmins";
            userAdmin.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            userAdmin.Url = userAdmin.ResolveUrl();
            TreeNode<NavigationNode> userAdminT = adminRoot.AddChild(userAdmin);

            SiteMapNode newUser = new SiteMapNode();
            newUser.Key = "UserEdit";
            newUser.ParentKey = "UserAdmin";
            newUser.Controller = "UserAdmin";
            newUser.Action = "UserEdit";
            newUser.Text = "NewUser";
            newUser.ViewRoles = "Admins";
            newUser.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            newUser.Url = newUser.ResolveUrl();
            TreeNode<NavigationNode> newUserT = userAdminT.AddChild(newUser);

            SiteMapNode userSearch = new SiteMapNode();
            userSearch.Key = "UserSearch";
            userSearch.ParentKey = "UserAdmin";
            userSearch.Controller = "UserAdmin";
            userSearch.Action = "Search";
            userSearch.Text = "User Search";
            userSearch.ViewRoles = "Admins";
            userSearch.ComponentVisibility = "SiteMapPathHelper,!*";
            userSearch.Url = userSearch.ResolveUrl();
            TreeNode<NavigationNode> userSearchT = userAdminT.AddChild(userSearch);

            SiteMapNode ipSearch = new SiteMapNode();
            ipSearch.Key = "IpSearch";
            ipSearch.ParentKey = "UserAdmin";
            ipSearch.Controller = "UserAdmin";
            ipSearch.Action = "IpSearch";
            ipSearch.Text = "IpSearch";
            ipSearch.ViewRoles = "Admins";
            ipSearch.ComponentVisibility = "SiteMapPathHelper,!*";
            ipSearch.Url = ipSearch.ResolveUrl();
            TreeNode<NavigationNode> ipSearchT = userAdminT.AddChild(ipSearch);


            SiteMapNode roleAdmin = new SiteMapNode();
            roleAdmin.Key = "RoleAdmin";
            roleAdmin.ParentKey = "SiteAdmin";
            roleAdmin.Controller = "RoleAdmin";
            roleAdmin.Action = "Index";
            roleAdmin.Text = "RoleManagement";
            roleAdmin.ViewRoles = "Admins";
            roleAdmin.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            roleAdmin.Url = roleAdmin.ResolveUrl();
            TreeNode<NavigationNode> roleAdminT = adminRoot.AddChild(roleAdmin);

            // TODO: this one should not be in main or child menus
            // we can't have just one url since it depends on roleId
            // but we do want it to appear ar the active breadcrumb
            SiteMapNode roleMembers = new SiteMapNode();
            roleMembers.Key = "RoleMembers";
            roleMembers.ParentKey = "RoleAdmin";
            roleMembers.Controller = "RoleAdmin";
            roleMembers.Action = "RoleMembers";
            roleMembers.Text = "RoleManagement";
            roleMembers.ViewRoles = "Admins";
            roleMembers.ComponentVisibility = "SiteMapPathHelper,!*";
            roleMembers.Url = roleMembers.ResolveUrl();
            roleMembers.PreservedRouteParameters = "roleId,pageNumber,pageSize";
            TreeNode<NavigationNode> roleMembersT = roleAdminT.AddChild(roleMembers);

            SiteMapNode roleEdit = new SiteMapNode();
            roleEdit.Key = "RoleEdit";
            roleEdit.ParentKey = "RoleAdmin";
            roleEdit.Controller = "RoleAdmin";
            roleEdit.Action = "RoleEdit";
            roleEdit.Text = "NewRole";
            roleEdit.ViewRoles = "Admins";
            roleEdit.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            roleEdit.Url = roleEdit.ResolveUrl();
            roleEdit.PreservedRouteParameters = "roleIde";
            TreeNode<NavigationNode> roleEditT = roleAdminT.AddChild(roleEdit);


            SiteMapNode coreData = new SiteMapNode();
            coreData.Key = "CoreData";
            coreData.ParentKey = "SiteAdmin";
            coreData.Controller = "CoreData";
            coreData.Action = "Index";
            coreData.Text = "CoreData";
            coreData.ViewRoles = "ServerAdmins";
            coreData.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            coreData.Url = coreData.ResolveUrl();
            TreeNode<NavigationNode> coreDataT = adminRoot.AddChild(coreData);

            SiteMapNode currencyList = new SiteMapNode();
            currencyList.Key = "CurrencyList";
            currencyList.ParentKey = "SiteAdmin";
            currencyList.Controller = "CoreData";
            currencyList.Action = "CurrencyList";
            currencyList.Text = "CurrencyAdministration";
            currencyList.ViewRoles = "ServerAdmins";
            currencyList.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            currencyList.Url = currencyList.ResolveUrl();
            TreeNode<NavigationNode> currencyListT = coreDataT.AddChild(currencyList);

            //TODO: again I think we just want to be a breadcrumb here
            SiteMapNode currencyEdit = new SiteMapNode();
            currencyEdit.Key = "CurrencyEdit";
            currencyEdit.ParentKey = "CurrencyList";
            currencyEdit.Controller = "CoreData";
            currencyEdit.Action = "CurrencyEdit";
            currencyEdit.Text = "NewCurrency";
            currencyEdit.ViewRoles = "ServerAdmins";
            currencyEdit.ComponentVisibility = "SiteMapPathHelper,!*";
            currencyEdit.Url = currencyEdit.ResolveUrl();
            currencyEdit.PreservedRouteParameters = "currencyGuid";
            TreeNode<NavigationNode> currencyEditT = currencyListT.AddChild(currencyEdit);


            SiteMapNode countryList = new SiteMapNode();
            countryList.Key = "CountryListPage";
            countryList.ParentKey = "SiteAdmin";
            countryList.Controller = "CoreData";
            countryList.Action = "CountryListPage";
            countryList.Text = "CountryStateAdministration";
            countryList.ViewRoles = "ServerAdmins";
            countryList.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            countryList.Url = countryList.ResolveUrl();
            TreeNode<NavigationNode> countryListT = coreDataT.AddChild(countryList);

            SiteMapNode countryEdit = new SiteMapNode();
            countryEdit.Key = "CountryEdit";
            countryEdit.ParentKey = "CountryListPage";
            countryEdit.Controller = "CoreData";
            countryEdit.Action = "CountryEdit";
            countryEdit.Text = "NewCountry";
            countryEdit.ViewRoles = "ServerAdmins";
            countryEdit.ComponentVisibility = "SiteMapPathHelper,!*";
            countryEdit.Url = countryEdit.ResolveUrl();
            countryEdit.PreservedRouteParameters = "guid";
            TreeNode<NavigationNode> countryEditT = countryListT.AddChild(countryEdit);

            SiteMapNode stateList = new SiteMapNode();
            stateList.Key = "StateListPage";
            stateList.ParentKey = "CountryListPage";
            stateList.Controller = "CoreData";
            stateList.Action = "StateListPage";
            stateList.Text = "States";
            stateList.ViewRoles = "ServerAdmins";
            stateList.ComponentVisibility = "SiteMapPathHelper,!*";
            stateList.Url = stateList.ResolveUrl();
            stateList.PreservedRouteParameters = "countryGuid";
            TreeNode<NavigationNode> stateListT = countryListT.AddChild(stateList);

            SiteMapNode stateEdit = new SiteMapNode();
            stateEdit.Key = "StateEdit";
            stateEdit.ParentKey = "StateListPage";
            stateEdit.Controller = "CoreData";
            stateEdit.Action = "StateEdit";
            stateEdit.Text = "New State";
            stateEdit.ViewRoles = "ServerAdmins";
            stateEdit.ComponentVisibility = "SiteMapPathHelper,!*";
            stateEdit.Url = stateEdit.ResolveUrl();
            stateEdit.PreservedRouteParameters = "countryGuid";
            TreeNode<NavigationNode> stateEditT = stateListT.AddChild(stateEdit);



            //string serialized = JsonConvert.SerializeObject(treeRoot,Formatting.Indented);


            return treeRoot;
        }

        //public TreeNode<NavigationNode> BuildTreeFromJson(string jsonString)
        //{

        //}

        

    }

    
}
