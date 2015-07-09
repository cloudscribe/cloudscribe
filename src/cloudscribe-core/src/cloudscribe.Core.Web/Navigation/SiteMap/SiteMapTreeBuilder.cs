// Author:					Joe Audette
// Created:					2015-07-07
// Last Modified:			2015-07-09
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
    public class SiteMapTreeBuilder
    {
        public SiteMapTreeBuilder()
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

            SiteMapNode rootNode = new SiteMapNode();
            rootNode.IsRootNode = true;
            rootNode.Key = "RootNode";

            TreeNode<NavigationNode> treeRoot = new TreeNode<NavigationNode>(rootNode);

            SiteMapNode home = new SiteMapNode();
            home.Key = "Home";
            home.ParentKey = "RootNode";
            home.Controller = "Home";
            home.Action = "Home";
            home.Title = "Home";
            treeRoot.AddChild(home);

            SiteMapNode about = new SiteMapNode();
            about.Key = "About";
            about.ParentKey = "RootNode";
            about.Controller = "Home";
            about.Action = "About";
            about.Title = "About";
            treeRoot.AddChild(about);

            SiteMapNode contact = new SiteMapNode();
            contact.Key = "Contact";
            contact.ParentKey = "RootNode";
            contact.Controller = "Home";
            contact.Action = "Contact";
            contact.Title = "Contact";
            treeRoot.AddChild(contact);


            SiteMapNode siteAdmin = new SiteMapNode();
            siteAdmin.Key = "SiteAdmin";
            siteAdmin.ParentKey = "RootNode";
            siteAdmin.Controller = "SiteAdmin";
            siteAdmin.Action = "Index";
            siteAdmin.Title = "Administration";
            siteAdmin.ViewRoles = "Admins,Content Administrators";
            TreeNode<NavigationNode> adminRoot = treeRoot.AddChild(siteAdmin);

            SiteMapNode siteSettings = new SiteMapNode();
            siteSettings.Key = "BasicSettings";
            siteSettings.ParentKey = "SiteAdmin";
            siteSettings.Controller = "SiteAdmin";
            siteSettings.Action = "SiteInfo";
            siteSettings.Title = "Site Settings";
            siteSettings.ViewRoles = "Admins,Content Administrators";
            siteSettings.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*"; //this pattern was used in mvcsitemapprovider may change
            siteSettings.PreservedRouteParameters = "siteGuid";
            TreeNode<NavigationNode> siteT = adminRoot.AddChild(siteSettings);

            SiteMapNode hosts = new SiteMapNode();
            hosts.Key = "SiteHostMappings";
            hosts.ParentKey = "BasicSettings";
            hosts.Controller = "SiteAdmin";
            hosts.Action = "SiteHostMappings";
            hosts.Title = "Domain Mappings";
            hosts.ViewRoles = "Admins,Content Administrators";
            hosts.ComponentVisibility = "SiteMapPathHelper,!*";
            hosts.PreservedRouteParameters = "siteGuid";
            TreeNode<NavigationNode> hostsT = siteT.AddChild(hosts);

            SiteMapNode siteList = new SiteMapNode();
            siteList.Key = "SiteList";
            siteList.ParentKey = "SiteAdmin";
            siteList.Controller = "SiteAdmin";
            siteList.Action = "SiteList";
            siteList.Title = "SiteList";
            siteList.ViewRoles = "ServerAdmins";
            siteList.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            TreeNode<NavigationNode> siteListT = adminRoot.AddChild(siteList);

            SiteMapNode newSite = new SiteMapNode();
            newSite.Key = "NewSite";
            newSite.ParentKey = "SiteList";
            newSite.Controller = "SiteAdmin";
            newSite.Action = "NewSite";
            newSite.Title = "NewSite";
            newSite.ViewRoles = "ServerAdmins";
            newSite.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            TreeNode<NavigationNode> newSiteT = siteListT.AddChild(newSite);


            SiteMapNode userAdmin = new SiteMapNode();
            userAdmin.Key = "UserAdmin";
            userAdmin.ParentKey = "SiteAdmin";
            userAdmin.Controller = "UserAdmin";
            userAdmin.Action = "Index";
            userAdmin.Title = "UserManagement";
            userAdmin.ViewRoles = "ServerAdmins";
            userAdmin.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            TreeNode<NavigationNode> userAdminT = adminRoot.AddChild(userAdmin);

            SiteMapNode newUser = new SiteMapNode();
            newUser.Key = "UserEdit";
            newUser.ParentKey = "UserAdmin";
            newUser.Controller = "UserAdmin";
            newUser.Action = "UserEdit";
            newUser.Title = "NewUser";
            newUser.ViewRoles = "Admins";
            newUser.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            TreeNode<NavigationNode> newUserT = userAdminT.AddChild(newUser);

            SiteMapNode userSearch = new SiteMapNode();
            userSearch.Key = "UserSearch";
            userSearch.ParentKey = "UserAdmin";
            userSearch.Controller = "UserAdmin";
            userSearch.Action = "Search";
            userSearch.Title = "User Search";
            userSearch.ViewRoles = "Admins";
            userSearch.ComponentVisibility = "SiteMapPathHelper,!*";
            TreeNode<NavigationNode> userSearchT = userAdminT.AddChild(userSearch);

            SiteMapNode ipSearch = new SiteMapNode();
            ipSearch.Key = "IpSearch";
            ipSearch.ParentKey = "UserAdmin";
            ipSearch.Controller = "UserAdmin";
            ipSearch.Action = "IpSearch";
            ipSearch.Title = "IpSearch";
            ipSearch.ViewRoles = "Admins";
            ipSearch.ComponentVisibility = "SiteMapPathHelper,!*";
            TreeNode<NavigationNode> ipSearchT = userAdminT.AddChild(ipSearch);

            //string serialized = JsonConvert.SerializeObject(treeRoot,Formatting.Indented);


            return treeRoot;
        }

        //public TreeNode<NavigationNode> BuildTreeFromJson(string jsonString)
        //{

        //}

        

    }

    
}
