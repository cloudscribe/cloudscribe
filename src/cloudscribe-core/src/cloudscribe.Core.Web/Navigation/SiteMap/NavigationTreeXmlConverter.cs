// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-07-14
// 

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationTreeXmlConverter
    {
        public string ToXmlString(TreeNode<NavigationNode> node)
        {
            StringBuilder s = new StringBuilder();
            Encoding encoding = Encoding.Unicode;
            using (StringWriter stringWriter = new StringWriter(s))
            {

                using (XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    writer.WriteStartDocument();
                    WriteNode(node, writer);
                }
            }

            return s.ToString();
        }

        private void WriteNode(TreeNode<NavigationNode> node, XmlWriter writer)
        {
            writer.WriteStartElement("NavNode");

            if (node.Value.Key.Length > 0)
            {
                writer.WriteAttributeString("key", node.Value.Key);
            }

            if (node.Value.ParentKey.Length > 0)
            {
                writer.WriteAttributeString("parentKey", node.Value.ParentKey);
            }

            if (node.Value.Controller.Length > 0)
            {
                writer.WriteAttributeString("controller", node.Value.Controller);
            }

            if (node.Value.Action.Length > 0)
            {
                writer.WriteAttributeString("action", node.Value.Action);
            }

            if (node.Value.Text.Length > 0)
            {
                writer.WriteAttributeString("text", node.Value.Text);
            }

            if (node.Value.Url.Length > 0)
            {
                writer.WriteAttributeString("url", node.Value.Url);
            }

            if (node.Value.PreservedRouteParameters.Length > 0)
            {
                writer.WriteAttributeString("preservedRouteParameters", node.Value.PreservedRouteParameters);
            }

            if (node.Value.ComponentVisibility.Length > 0)
            {
                writer.WriteAttributeString("componentVisibility", node.Value.ComponentVisibility);
            }

            if (node.Value.ViewRoles.Length > 0)
            {
                writer.WriteAttributeString("viewRoles", node.Value.ViewRoles);
            }

            if (node.Value.IsRootNode)
            {
                writer.WriteAttributeString("isRootNode", "true");
            }

            if (node.Value.ResourceName.Length > 0)
            {
                writer.WriteAttributeString("resourceName", node.Value.ResourceName);
            }

            if (node.Value.ResourceTextKey.Length > 0)
            {
                writer.WriteAttributeString("resourceTextKey", node.Value.ResourceTextKey);
            }

            if (node.Value.ResourceTitleKey.Length > 0)
            {
                writer.WriteAttributeString("resourceTitleKey", node.Value.ResourceTitleKey);
            }

            if (!node.Value.IncludeAmbientValuesInUrl)
            {
                writer.WriteAttributeString("includeAmbientValuesInUrl", "false");
            }


            // children
            writer.WriteStartElement("Children");

            WriteChildNodes(node, writer);

            writer.WriteEndElement(); //Children


            writer.WriteEndElement(); //NavNode
        }

        private void WriteChildNodes(TreeNode<NavigationNode> node, XmlWriter writer)
        {
            foreach (TreeNode<NavigationNode> child in node.Children)
            {
                WriteNode(child, writer);
            }
        }

        public TreeNode<NavigationNode> FromXml(XmlDocument xml)
        {
            XmlAttributeCollection attributeCollection = xml.DocumentElement.Attributes;

            if(xml.DocumentElement.Name != "NavNode") { throw new ArgumentException("Expected NavNode"); }

            NavigationNode rootNav = BuildNavNode(xml.DocumentElement.Attributes);

            TreeNode<NavigationNode> treeRoot = new TreeNode<NavigationNode>(rootNav);

            foreach (XmlNode childNode in xml.DocumentElement.ChildNodes)
            {
                if (childNode.Name == "Children")
                {
                    AddChildNode(treeRoot, childNode);
                }
            }

            return treeRoot;
        }

        private void AddChildNode(TreeNode<NavigationNode> node, XmlNode xmlNode)
        {
            NavigationNode navNode = BuildNavNode(xmlNode.Attributes);
            TreeNode<NavigationNode> navNodeT = node.AddChild(navNode);

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.Name == "Children")
                {
                    AddChildNode(navNodeT, childNode); //recursion
                }
            }
        }

        private NavigationNode BuildNavNode(XmlAttributeCollection attributeCollection)
        {
            NavigationNode navNode = new NavigationNode();

            if (attributeCollection["key"] != null)
            {
                navNode.Key = attributeCollection["key"].Value;
            }

            if (attributeCollection["parentKey"] != null)
            {
                navNode.ParentKey = attributeCollection["parentKey"].Value;
            }

            if (attributeCollection["controller"] != null)
            {
                navNode.Controller = attributeCollection["controller"].Value;
            }

            if (attributeCollection["action"] != null)
            {
                navNode.Action = attributeCollection["action"].Value;
            }

            if (attributeCollection["text"] != null)
            {
                navNode.Text = attributeCollection["text"].Value;
            }

            if (attributeCollection["title"] != null)
            {
                navNode.Title = attributeCollection["title"].Value;
            }

            if (attributeCollection["url"] != null)
            {
                navNode.Url = attributeCollection["url"].Value;
            }
            else
            {
                navNode.Url = navNode.ResolveUrl();
            }

            if (attributeCollection["isRootNode"] != null)
            {
                navNode.IsRootNode = Convert.ToBoolean(attributeCollection["isRootNode"].Value);
            }

            if (attributeCollection["includeAmbientValuesInUrl"] != null)
            {
                navNode.IncludeAmbientValuesInUrl = Convert.ToBoolean(attributeCollection["includeAmbientValuesInUrl"].Value);
            }

            if (attributeCollection["resourceName"] != null)
            {
                navNode.ResourceName = attributeCollection["resourceName"].Value;
            }

            if (attributeCollection["resourceTextKey"] != null)
            {
                navNode.ResourceTextKey = attributeCollection["resourceTextKey"].Value;
            }

            if (attributeCollection["resourceTitleKey"] != null)
            {
                navNode.ResourceTitleKey = attributeCollection["resourceTitleKey"].Value;
            }

            if (attributeCollection["preservedRouteParameters"] != null)
            {
                navNode.PreservedRouteParameters = attributeCollection["preservedRouteParameters"].Value;
            }

            if (attributeCollection["componentVisibility"] != null)
            {
                navNode.ComponentVisibility = attributeCollection["componentVisibility"].Value;
            }

            if (attributeCollection["viewRoles"] != null)
            {
                navNode.ViewRoles = attributeCollection["viewRoles"].Value;
            }

            return navNode;
        }

        



    }
}
