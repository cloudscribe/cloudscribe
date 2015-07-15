// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-07-15
// 

using System;
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

            NavigationNode rootNav = BuildNavNode(xml.Root);

            TreeNode<NavigationNode> treeRoot = new TreeNode<NavigationNode>(rootNav);

            foreach (XElement childrenNode in xml.Root.Elements(XName.Get("Children")))
            {
                foreach (XElement childNode in childrenNode.Elements(XName.Get("NavNode")))
                {
                    AddChildNode(treeRoot, childNode);
                }
                    
            }

            return treeRoot;
        }

        private void AddChildNode(TreeNode<NavigationNode> node, XElement xmlNode)
        {
            NavigationNode navNode = BuildNavNode(xmlNode);
            TreeNode<NavigationNode> navNodeT = node.AddChild(navNode);

            foreach (XElement childrenNode in xmlNode.Elements(XName.Get("Children")))
            {
                foreach (XElement childNode in childrenNode.Elements(XName.Get("NavNode")))
                {
                    AddChildNode(navNodeT, childNode); //recursion   
                }
            }
        }

        private NavigationNode BuildNavNode(XElement xmlNode)
        {
            NavigationNode navNode = new NavigationNode();

            var a = xmlNode.Attribute("key");
            if(a != null) {  navNode.Key = a.Value; }

            a = xmlNode.Attribute("parentKey");
            if (a != null) { navNode.ParentKey = a.Value; }

            a = xmlNode.Attribute("controller");
            if (a != null) { navNode.Controller = a.Value; }

            a = xmlNode.Attribute("action");
            if (a != null) { navNode.Action = a.Value; }

            a = xmlNode.Attribute("text");
            if (a != null) { navNode.Text = a.Value; }

            a = xmlNode.Attribute("title");
            if (a  != null) { navNode.Title = a.Value; }

            a = xmlNode.Attribute("url");
            if (a != null) { navNode.Url = a.Value; }
            else
            {
                navNode.Url = navNode.ResolveUrl();
            }

            a = xmlNode.Attribute("isRootNode");
            if (a != null) { navNode.IsRootNode = Convert.ToBoolean(a.Value); }

            a = xmlNode.Attribute("includeAmbientValuesInUrl");
            if (a != null) { navNode.IncludeAmbientValuesInUrl = Convert.ToBoolean(a.Value); }

            a = xmlNode.Attribute("resourceName");
            if (a != null) { navNode.ResourceName = a.Value; }

            a = xmlNode.Attribute("resourceTextKey");
            if (a != null) { navNode.ResourceTextKey = a.Value; }

            a = xmlNode.Attribute("resourceTitleKey");
            if (a != null) { navNode.ResourceTitleKey = a.Value; }

            a = xmlNode.Attribute("preservedRouteParameters");
            if (a != null) { navNode.PreservedRouteParameters = a.Value; }

            a = xmlNode.Attribute("componentVisibility");
            if (a != null) { navNode.ComponentVisibility = a.Value; }

            a = xmlNode.Attribute("viewRoles");
            if (a != null) { navNode.ViewRoles = a.Value; }

            
            
            return navNode;
        }

    }
}
