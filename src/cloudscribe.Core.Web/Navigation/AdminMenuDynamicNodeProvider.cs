// Author:					Joe Audette
// Created:					2014-11-21
// Last Modified:			2014-11-21
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcSiteMapProvider;
using log4net;
using cloudscribe.Resources;

namespace cloudscribe.Core.Web.Navigation
{
    public class AdminMenuDynamicNodeProvider : DynamicNodeProviderBase 
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(AdminMenuDynamicNodeProvider));

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            //log.Info(node.Title);
            if (node.Title == "AdminItems")
            {
                return BuildAdminChildNodes();
            }

            return BuildAdminNodes();

            //using (var storeDB = new MusicStoreEntities())
            //{
            //    // Create a node for each album 
            //    foreach (var album in storeDB.Albums.Include("Genre"))
            //    {
            //        DynamicNode dynamicNode = new DynamicNode();
            //        dynamicNode.Title = album.Title;
            //        dynamicNode.ParentKey = "Genre_" + album.Genre.Name;
            //        dynamicNode.RouteValues.Add("id", album.AlbumId);

            //        yield return dynamicNode;
            //    }
            //}
        } 

        private List<DynamicNode> BuildAdminNodes()
        {
            List<DynamicNode> nodeList = new List<DynamicNode>();

            DynamicNode node = new DynamicNode();
            node.Title = CommonResources.Administration;
            node.Key = "SiteAdmin";
            node.Controller = "SiteAdmin";
            node.Action = "Index";
            node.Roles.Add("Admins");
            node.Roles.Add("Content Administrators");
           
            nodeList.Add(node);

            return nodeList;

        }

        private List<DynamicNode> BuildAdminChildNodes()
        {
            List<DynamicNode> nodeList = new List<DynamicNode>();

            DynamicNode node = new DynamicNode();
            node.Title = CommonResources.BasicSettings;
            node.Key = "BasicSettings";
            node.ParentKey = "SiteAdmin";
            node.Controller = "SiteAdmin";
            node.Action = "SiteInfo";
            node.Roles.Add("Admins");
            nodeList.Add(node);

            node = new DynamicNode();
            node.Title = CommonResources.SiteList;
            node.Key = "SiteList";
            node.ParentKey = "SiteAdmin";
            node.Controller = "SiteAdmin";
            node.Action = "SiteList";
            node.Roles.Add("Admins");
            nodeList.Add(node);

            node = new DynamicNode();
            node.Title = CommonResources.Roles;
            node.Key = "Roles";
            node.ParentKey = "SiteAdmin";
            node.Controller = "SiteAdmin";
            node.Action = "Roles";
            node.Roles.Add("Admins");
            nodeList.Add(node);

            node = new DynamicNode();
            node.Title = CommonResources.CoreData;
            node.Key = "CoreData";
            node.ParentKey = "SiteAdmin";
            node.Controller = "CoreData";
            node.Action = "Index";
            node.Roles.Add("Admins");
            nodeList.Add(node);

            LoadCoreDataMenu(nodeList);
            

            return nodeList;

        }

        private void LoadCoreDataMenu(List<DynamicNode> nodeList)
        {
            DynamicNode node = new DynamicNode();
            node.Title = CommonResources.CurrencyAdministration;
            node.Key = "CurrencyList";
            node.ParentKey = "CoreData";
            node.Controller = "CoreData";
            node.Action = "CurrencyList";
            node.Roles.Add("Admins");
            nodeList.Add(node);

            node = new DynamicNode();
            node.Title = CommonResources.CountryListAdministration;
            node.Key = "CountryListPage";
            node.ParentKey = "CoreData";
            node.Controller = "CoreData";
            node.Action = "CountryListPage";
            node.Roles.Add("Admins");
            //node.PreservedRouteParameters.Add("pageNumber");
            //node.RouteValues.Add("pageNumber", 1);
            nodeList.Add(node);

            node = new DynamicNode();
            node.Title = CommonResources.States;
            node.Key = "StateListPage";
            node.ParentKey = "CountryListPage";
            node.Controller = "CoreData";
            node.Action = "StateListPage";
            node.Roles.Add("Admins");
            //node.PreservedRouteParameters.Add("countryGuid");
          
            nodeList.Add(node);

        }

        private List<DynamicNode> BuildCoreDataChildNodes()
        {
            List<DynamicNode> nodes = new List<DynamicNode>();

            DynamicNode dynamicNode = new DynamicNode();
            dynamicNode.Title = CommonResources.BasicSettings;
            dynamicNode.Key = "BasicSettings";
            dynamicNode.ParentKey = "SiteAdmin";
            dynamicNode.Controller = "SiteAdmin";
            dynamicNode.Action = "SiteInfo";
            dynamicNode.Roles.Add("Admins");
            nodes.Add(dynamicNode);

            dynamicNode = new DynamicNode();
            dynamicNode.Title = CommonResources.SiteList;
            dynamicNode.Key = "SiteList";
            dynamicNode.ParentKey = "SiteAdmin";
            dynamicNode.Controller = "SiteAdmin";
            dynamicNode.Action = "SiteList";
            dynamicNode.Roles.Add("Admins");
            nodes.Add(dynamicNode);

            dynamicNode = new DynamicNode();
            dynamicNode.Title = CommonResources.Roles;
            dynamicNode.Key = "Roles";
            dynamicNode.ParentKey = "SiteAdmin";
            dynamicNode.Controller = "SiteAdmin";
            dynamicNode.Action = "Roles";
            dynamicNode.Roles.Add("Admins");
            nodes.Add(dynamicNode);

            dynamicNode = new DynamicNode();
            dynamicNode.Title = CommonResources.CoreData;
            dynamicNode.Key = "CoreData";
            dynamicNode.ParentKey = "SiteAdmin";
            dynamicNode.Controller = "CoreData";
            dynamicNode.Action = "Index";
            dynamicNode.Roles.Add("Admins");
            nodes.Add(dynamicNode);

            return nodes;

        }

    }
}
