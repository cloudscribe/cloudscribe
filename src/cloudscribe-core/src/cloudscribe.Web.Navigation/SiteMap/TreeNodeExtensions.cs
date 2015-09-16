// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-09
// Last Modified:			2015-09-16
// 

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace cloudscribe.Web.Navigation
{
    public static class TreeNodeExtensions
    {
        /// <summary>
        /// finds the first child node whose Url property contains the urlToMatch
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="urlToMatch"></param>
        /// <returns></returns>
        public static TreeNode<NavigationNode> FindByUrl(this TreeNode<NavigationNode> currentNode, string urlToMatch, string urlPrefix = "")
        {
            Func<TreeNode<NavigationNode>, bool> match = delegate (TreeNode<NavigationNode> n)
            {
                if( n.Value.Url.Contains(urlToMatch)) { return true; }
                if(urlPrefix.Length > 0)
                {
                    string targetUrl = n.Value.Url.Replace("~/", "~/" + urlPrefix + "/");
                    if(targetUrl.Contains(urlToMatch)) { return true; }
                }

                return false;
            };

            return currentNode.Find(match);
        }

        /// <summary>
        /// finds the first child node whose url exactly matches the provided urlToMatch
        /// note that Url usually starts with ~/
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="urlToMatch"></param>
        /// <returns></returns>
        public static TreeNode<NavigationNode> FindByUrlExact(this TreeNode<NavigationNode> currentNode, string urlToMatch)
        {
            Func<TreeNode<NavigationNode>, bool> match = delegate (TreeNode<NavigationNode> n)
            {
                return (n.Value.Url == urlToMatch);
            };

            return currentNode.Find(match);
        }

        public static TreeNode<NavigationNode> FindByKey(this TreeNode<NavigationNode> currentNode, string key)
        {
            Func<TreeNode<NavigationNode>, bool> match = delegate (TreeNode<NavigationNode> n)
            {
                return (n.Value.Key == key);
            };

            return currentNode.Find(match);
        }

        /// <summary>
        /// finds the first child node that matches the provided controller and action name
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TreeNode<NavigationNode> FindByControllerAndAction(this TreeNode<NavigationNode> currentNode, string controller, string action)
        {
            Func<TreeNode<NavigationNode>, bool> match = delegate (TreeNode<NavigationNode> n)
            {
                return ((n.Value.Controller == controller)&&(n.Value.Action == action));
            };

            return currentNode.Find(match);
        }

        public static List<TreeNode<NavigationNode>> GetParentNodeChain(
            this TreeNode<NavigationNode> currentNode, 
            bool includeCurrentNode,
            bool includeRoot)
        {
            List<TreeNode<NavigationNode>> list = new List<TreeNode<NavigationNode>>();
            if(includeCurrentNode)
            {
                list.Add(currentNode);
            }

            TreeNode<NavigationNode> parentNode = currentNode.Parent;
            while(parentNode != null)
            {
                if(includeRoot ||(!parentNode.Value.IsRootNode))
                {
                    list.Add(parentNode);
                }
               
                parentNode = parentNode.Parent;
            }

            // this is used for breadcrumbs
            // so we want the sort from parent down to current node
            list.Reverse();

            return list;
        }

        public static bool EqualsNode(this TreeNode<NavigationNode> currentNode, TreeNode<NavigationNode> nodeToMatch)
        {
            if(currentNode.Value.Key == nodeToMatch.Value.Key) { return true; }
            if (currentNode.Value.Controller == nodeToMatch.Value.Controller && (currentNode.Value.Controller.Length > 0))
            {
                if (currentNode.Value.Action == nodeToMatch.Value.Action && (currentNode.Value.Action.Length > 0))
                {
                    return true;
                }
                    
            }
            if (currentNode.Value.Url == nodeToMatch.Value.Url) { return true; }

            return false;
        }



        public static string ToJsonIndented(this TreeNode<NavigationNode> node)
        {
            return JsonConvert.SerializeObject(
                node,
                Formatting.Indented,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }
                );
        }

        public static string ToJsonCompact(this TreeNode<NavigationNode> node)
        {
            return JsonConvert.SerializeObject(
                node,
                Formatting.None,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }
                );
        }
    }
}
