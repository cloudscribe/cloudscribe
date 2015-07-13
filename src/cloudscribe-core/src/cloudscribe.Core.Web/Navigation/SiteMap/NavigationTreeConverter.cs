// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-07
// Last Modified:			2015-07-13
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationTreeConverter : JsonCreationConverter<TreeNode<NavigationNode>>
    {
        protected override TreeNode<NavigationNode> Create(Type objectType, JObject jObject)
        {
            ExtendedNavigationNode home = new ExtendedNavigationNode();
            home.Key = (string)jObject["Value"]["Key"];
            home.ParentKey = (string)jObject["Value"]["ParentKey"]; 
            home.Controller = (string)jObject["Value"]["Controller"];
            home.Action = (string)jObject["Value"]["Action"];
            home.Text = (string)jObject["Value"]["Text"];
            home.Url = home.ResolveUrl();
            home.IsRootNode = true;
            TreeNode<NavigationNode> treeRoot = new TreeNode<NavigationNode>(home);

            JArray firstLevelChildren = (JArray)jObject["Children"];
            AddChildren(treeRoot, firstLevelChildren);

            return treeRoot;


        }

        private void AddChildren(TreeNode<NavigationNode> node, JArray children)
        {
            foreach (var child in children)
            {
                //build the child node
                ExtendedNavigationNode childNode = new ExtendedNavigationNode();
                childNode.Key = (string)child["Value"]["Key"];
                childNode.ParentKey = (string)child["Value"]["ParentKey"];
                childNode.Controller = (string)child["Value"]["Controller"];
                childNode.Action = (string)child["Value"]["Action"];
                childNode.Text = (string)child["Value"]["Text"];

                if (child["Value"]["Url"] != null)
                {
                    childNode.Url = (string)child["Value"]["Url"];
                }
                else
                {
                    childNode.Url = childNode.ResolveUrl();
                }
                

                if(child["Value"]["PreservedRouteParameters"] != null)
                {
                    childNode.PreservedRouteParameters = (string)child["Value"]["PreservedRouteParameters"];
                }

                if (child["Value"]["ComponentVisibility"] != null)
                {
                    childNode.ComponentVisibility = (string)child["Value"]["ComponentVisibility"];
                }

                

                // add the node to the parent and create the childnodeT reference for the next level of children
                TreeNode<NavigationNode> childNodeT = node.AddChild(childNode);



                JArray subChildren = (JArray)child["Children"];
                if (subChildren.Count > 0)
                {
                    // recursion
                    AddChildren(childNodeT, subChildren);
                }


            }

        }
        

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }
}
