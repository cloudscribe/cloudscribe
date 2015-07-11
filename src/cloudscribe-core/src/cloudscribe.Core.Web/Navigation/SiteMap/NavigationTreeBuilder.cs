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

            //string serialized = JsonConvert.SerializeObject(treeRoot,Formatting.Indented);


            return treeRoot;
        }

        //public TreeNode<NavigationNode> BuildTreeFromJson(string jsonString)
        //{

        //}

        

    }

    
}
