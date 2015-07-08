// Author:					Joe Audette
// Created:					2015-07-07
// Last Modified:			2015-07-07
// 


using cloudscribe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace cloudscribe.Core.Web.ViewModels
{
    public class SiteMapTreeBuilder
    {
        public SiteMapTreeBuilder()
        {

        }

        public TreeNode<SiteMapNode> GetTree()
        {
            return BuildTree();
        }

        private TreeNode<SiteMapNode> BuildTree()
        {
            SiteMapNode rootNode = new SiteMapNode();
            rootNode.IsRootNode = true;
            rootNode.Key = "hardcodedroot";

            TreeNode<SiteMapNode> treeRoot = new TreeNode<SiteMapNode>(rootNode);

            SiteMapNode home = new SiteMapNode();
            home.Key = "Home";
            home.Controller = "Home";
            home.Action = "Home";
            home.Title = "Home";
            treeRoot.AddChild(home);

            SiteMapNode about = new SiteMapNode();
            about.Key = "About";
            about.Controller = "Home";
            about.Action = "About";
            about.Title = "About";
            treeRoot.AddChild(about);

            SiteMapNode contact = new SiteMapNode();
            contact.Key = "Contact";
            contact.Controller = "Home";
            contact.Action = "Contact";
            contact.Title = "Contact";
            treeRoot.AddChild(contact);


            SiteMapNode siteAdmin = new SiteMapNode();
            siteAdmin.Key = "SiteAdmin";
            siteAdmin.Controller = "SiteAdmin";
            siteAdmin.Action = "Index";
            siteAdmin.Title = "Administration";
            siteAdmin.ViewRoles = "Admins,Content Administrators";
            TreeNode<SiteMapNode> adminRoot = treeRoot.AddChild(siteAdmin);

            SiteMapNode siteSettings = new SiteMapNode();
            siteSettings.Key = "BasicSettings";
            siteSettings.Controller = "SiteAdmin";
            siteSettings.Action = "SiteInfo";
            siteSettings.Title = "Site Settings";
            siteSettings.ViewRoles = "Admins,Content Administrators";
            siteSettings.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*"; //this pattern was used in mvcsitemapprovider may change
            siteSettings.PreservedRouteParameters = "siteGuid";
            TreeNode<SiteMapNode> siteT  = adminRoot.AddChild(siteSettings);

            SiteMapNode hosts = new SiteMapNode();
            hosts.Key = "SiteHostMappings";
            hosts.Controller = "SiteAdmin";
            hosts.Action = "SiteHostMappings";
            hosts.Title = "Domain Mappings";
            hosts.ViewRoles = "Admins,Content Administrators";
            hosts.ComponentVisibility = "SiteMapPathHelper,!*";
            hosts.PreservedRouteParameters = "siteGuid";
            TreeNode<SiteMapNode> hostsT = siteT.AddChild(hosts);

            SiteMapNode siteList = new SiteMapNode();
            siteList.Key = "SiteList";
            siteList.Controller = "SiteAdmin";
            siteList.Action = "SiteList";
            siteList.Title = "SiteList";
            siteList.ViewRoles = "ServerAdmins";
            siteList.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            TreeNode<SiteMapNode> siteListT = adminRoot.AddChild(siteList);

            SiteMapNode newSite = new SiteMapNode();
            newSite.Key = "NewSite";
            newSite.Controller = "SiteAdmin";
            newSite.Action = "NewSite";
            newSite.Title = "NewSite";
            newSite.ViewRoles = "ServerAdmins";
            newSite.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            TreeNode<SiteMapNode> newSiteT = siteListT.AddChild(newSite);


            SiteMapNode userAdmin = new SiteMapNode();
            userAdmin.Key = "UserAdmin";
            userAdmin.Controller = "UserAdmin";
            userAdmin.Action = "Index";
            userAdmin.Title = "UserManagement";
            userAdmin.ViewRoles = "ServerAdmins";
            userAdmin.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            TreeNode<SiteMapNode> userAdminT = adminRoot.AddChild(userAdmin);

            SiteMapNode newUser = new SiteMapNode();
            newUser.Key = "UserEdit";
            newUser.Controller = "UserAdmin";
            newUser.Action = "UserEdit";
            newUser.Title = "NewUser";
            newUser.ViewRoles = "Admins";
            newUser.ComponentVisibility = "SiteMapPathHelper,ChildMenu,!*";
            TreeNode<SiteMapNode> newUserT = userAdminT.AddChild(newUser);

            SiteMapNode userSearch = new SiteMapNode();
            userSearch.Key = "UserSearch";
            userSearch.Controller = "UserAdmin";
            userSearch.Action = "Search";
            userSearch.Title = "User Search";
            userSearch.ViewRoles = "Admins";
            userSearch.ComponentVisibility = "SiteMapPathHelper,!*";
            TreeNode<SiteMapNode> userSearchT = userAdminT.AddChild(userSearch);

            SiteMapNode ipSearch = new SiteMapNode();
            ipSearch.Key = "IpSearch";
            ipSearch.Controller = "UserAdmin";
            ipSearch.Action = "IpSearch";
            ipSearch.Title = "IpSearch";
            ipSearch.ViewRoles = "Admins";
            ipSearch.ComponentVisibility = "SiteMapPathHelper,!*";
            TreeNode<SiteMapNode> ipSearchT = userAdminT.AddChild(ipSearch);

            //string serialized = JsonConvert.SerializeObject(treeRoot);


            return treeRoot;
        }
    }
}
