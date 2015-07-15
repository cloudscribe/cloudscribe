// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-07
// Last Modified:			2015-07-15
// 

using Newtonsoft.Json.Linq;
using System;

namespace cloudscribe.Web.Navigation
{
    public class NavigationTreeJsonConverter : JsonCreationConverter<TreeNode<NavigationNode>>
    {
        public string ConvertToJsonIndented(TreeNode<NavigationNode> tNode)
        {
            return tNode.ToJsonIndented();
        }

        public string ConvertToJsonCompact(TreeNode<NavigationNode> tNode)
        {
            return tNode.ToJsonCompact();
        }

        protected override TreeNode<NavigationNode> Create(Type objectType, JObject jObject)
        {
            if(jObject["Value"] == null) { return null; }

            TreeNode<NavigationNode> treeRoot = CreateTreeNode(null, jObject);
            if(jObject["Children"] != null)
            {
                JArray firstLevelChildren = (JArray)jObject["Children"];
                AddChildren(treeRoot, firstLevelChildren);
            }
            
        
            return treeRoot;
        }

        private TreeNode<NavigationNode> CreateTreeNode(TreeNode<NavigationNode> tNode, JToken jNode)
        {
            //build the child node
            NavigationNode navNode = new NavigationNode();

            if(jNode["Value"]["Key"] != null)
            {
                navNode.Key = (string)jNode["Value"]["Key"];
            }
            
            if(jNode["Value"]["ParentKey"] != null)
            {
                navNode.ParentKey = (string)jNode["Value"]["ParentKey"];
            }
            
            if(jNode["Value"]["Controller"] != null)
            {
                navNode.Controller = (string)jNode["Value"]["Controller"];
            }
            
            if(jNode["Value"]["Action"] != null)
            {
                navNode.Action = (string)jNode["Value"]["Action"];
            }
            
            if(jNode["Value"]["Text"] != null)
            {
                navNode.Text = (string)jNode["Value"]["Text"];
            }
            
            if (jNode["Value"]["Url"] != null)
            {
                navNode.Url = (string)jNode["Value"]["Url"];
            }
            else
            {
                navNode.Url = navNode.ResolveUrl();
            }

            if (jNode["Value"]["PreservedRouteParameters"] != null)
            {
                navNode.PreservedRouteParameters = (string)jNode["Value"]["PreservedRouteParameters"];
            }

            if (jNode["Value"]["ComponentVisibility"] != null)
            {
                navNode.ComponentVisibility = (string)jNode["Value"]["ComponentVisibility"];
            }

            if (jNode["Value"]["ViewRoles"] != null)
            {
                navNode.ViewRoles = (string)jNode["Value"]["ViewRoles"];
            }

            if (jNode["Value"]["IsRootNode"] != null)
            {
                navNode.IsRootNode = Convert.ToBoolean((string)jNode["Value"]["IsRootNode"]);
            }

            if (jNode["Value"]["ResourceName"] != null)
            {
                navNode.ResourceName = (string)jNode["Value"]["ResourceName"];
            }

            if (jNode["Value"]["ResourceTextKey"] != null)
            {
                navNode.ResourceTextKey = (string)jNode["Value"]["ResourceTextKey"];
            }

            if (jNode["Value"]["ResourceTitleKey"] != null)
            {
                navNode.ResourceTitleKey = (string)jNode["Value"]["ResourceTitleKey"];
            }

            if (jNode["Value"]["IncludeAmbientValuesInUrl"] != null)
            {
                navNode.IncludeAmbientValuesInUrl = Convert.ToBoolean((string)jNode["Value"]["IncludeAmbientValuesInUrl"]);
            }

            if (tNode == null)
            {
                TreeNode<NavigationNode> rootNode = new TreeNode<NavigationNode>(navNode);
                return rootNode;
            }
            else
            { 
                TreeNode<NavigationNode> childNode = tNode.AddChild(navNode);

                return childNode;
            }

            
        }

        private void AddChildren(TreeNode<NavigationNode> node, JArray children)
        {
            foreach (var child in children)
            {
                TreeNode<NavigationNode> childNodeT = CreateTreeNode(node, child);
                
                if(child["Children"] != null)
                {
                    JArray subChildren = (JArray)child["Children"];
                    if (subChildren.Count > 0)
                    {
                        // recursion
                        AddChildren(childNodeT, subChildren);
                    }

                }
                
            }

        }
        

    }
}
