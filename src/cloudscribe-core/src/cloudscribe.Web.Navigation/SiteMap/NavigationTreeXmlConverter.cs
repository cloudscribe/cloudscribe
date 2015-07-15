// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-07-15
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace cloudscribe.Web.Navigation
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

        public TreeNode<NavigationNode> FromXml(XDocument xml)
        { 
            if(xml.Root.Name != "NavNode") { throw new ArgumentException("Expected NavNode"); }

            NavigationNode rootNav = BuildNavNode(xml.Root.Attributes());

            TreeNode<NavigationNode> treeRoot = new TreeNode<NavigationNode>(rootNav);

            foreach (XElement childNode in xml.Root.Elements())
            {
                if (childNode.Name == "Children")
                {
                    AddChildNode(treeRoot, childNode);
                }
            }

            return treeRoot;
        }

        private void AddChildNode(TreeNode<NavigationNode> node, XElement xmlNode)
        {
            NavigationNode navNode = BuildNavNode(xmlNode.Attributes());
            TreeNode<NavigationNode> navNodeT = node.AddChild(navNode);

            foreach (XElement childNode in xmlNode.Elements())
            {
                if (childNode.Name == "Children")
                {
                    AddChildNode(navNodeT, childNode); //recursion
                }
            }
        }

        private NavigationNode BuildNavNode(IEnumerable<XAttribute> attributeCollection)
        {
            NavigationNode navNode = new NavigationNode();

            foreach(XAttribute attribute in attributeCollection)
            {
                if(attribute.Name == "key")
                { 
                    navNode.Key = attribute.Value;
                }

                if (attribute.Name == "parentKey")
                {
                    navNode.ParentKey = attribute.Value;
                }

                if (attribute.Name == "controller")
                {
                    navNode.Controller = attribute.Value;
                }

                if (attribute.Name == "action")
                {
                    navNode.Action = attribute.Value;
                }

                if (attribute.Name == "text")
                {
                    navNode.Text = attribute.Value;
                }

                if (attribute.Name == "title")
                {
                    navNode.Title = attribute.Value;
                }

                if (attribute.Name == "url")
                {
                    navNode.Url = attribute.Value;
                }
                else
                {
                    navNode.Url = navNode.ResolveUrl();
                }

                if (attribute.Name == "isRootNode")
                {
                    navNode.IsRootNode = Convert.ToBoolean(attribute.Value);
                }

                if (attribute.Name == "includeAmbientValuesInUrl")
                {
                    navNode.IncludeAmbientValuesInUrl = Convert.ToBoolean(attribute.Value);
                }

                if (attribute.Name == "resourceName")
                {
                    navNode.ResourceName = attribute.Value;
                }

                if (attribute.Name == "resourceTextKey")
                {
                    navNode.ResourceTextKey = attribute.Value;
                }

                if (attribute.Name == "resourceTitleKey")
                {
                    navNode.ResourceTitleKey = attribute.Value;
                }

                if (attribute.Name == "preservedRouteParameters")
                {
                    navNode.PreservedRouteParameters = attribute.Value;
                }

                if (attribute.Name == "componentVisibility")
                {
                    navNode.ComponentVisibility = attribute.Value;
                }

                if (attribute.Name == "viewRoles")
                {
                    navNode.ViewRoles = attribute.Value;
                }

            }
            
            
            return navNode;
        }

        



    }
}
