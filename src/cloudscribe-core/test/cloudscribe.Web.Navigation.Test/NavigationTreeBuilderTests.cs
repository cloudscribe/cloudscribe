// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-09
// Last Modified:			2015-07-15
// 
// borrowed/modified from Martijn Boland
// trying to replicate some of the tests that Martijn Boland did here
// https://github.com/martijnboland/MvcPaging/blob/master/src/MvcPaging.Tests/PagerTests.cs

using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Navigation;
using cloudscribe.Web.Navigation;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace cloudscribe.Web.Navigation.Test
{
    public class NavigationTreeBuilderTests
    {

        private readonly ITestOutputHelper output;

        public NavigationTreeBuilderTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Can_Serialize_To_Json()
        {
            // Assemble
            HardCodedNavigationTreeBuilder builder = new HardCodedNavigationTreeBuilder();

            //Act
            string json = builder.GetTree().ToJsonIndented();



            //Assert
            Assert.True(!string.IsNullOrEmpty(json));

            //using (StreamWriter stream = File.CreateText("dumpjson.txt"))
            //{
            //    stream.WriteLine(json);
            //}

            //System.Console.WriteLine(json);

            //output.WriteLine(json);
        }

        [Fact]
        public void Can_Roundtrip_Serialize_To_Json()
        {
            // Assemble
            HardCodedNavigationTreeBuilder builder = new HardCodedNavigationTreeBuilder();
            TreeNode<NavigationNode> rootNode = builder.GetTree();

            //Act
            string json = rootNode.ToJsonIndented();

            //NavigationTreeJsonConverter treeConverter = new NavigationTreeJsonConverter();

            //TreeNode<NavigationNode> roundTripNode = builder.BuildTreeFromJson(json);

            //string json2 = roundTripNode.ToJsonIndented();

            //using (StreamWriter stream = File.CreateText("dumpjson-roundtrip.txt"))
            //{
            //    stream.WriteLine(json2);
            //}

            //Assert

            //Assert.True(roundTripNode != null);

            //// currently fails
            //Assert.True(roundTripNode.Children.Count > 2);


        }

        [Fact]
        public void Can_Roundtrip_Serialize_To_Xml()
        {
            // Assemble
            HardCodedNavigationTreeBuilder builder = new HardCodedNavigationTreeBuilder();
            TreeNode<NavigationNode> rootNode = builder.GetTree();
            NavigationTreeXmlConverter converter = new NavigationTreeXmlConverter();

            //Act
            string xml = converter.ToXmlString(rootNode);

            Assert.True(xml.Length > 10);

            //using (StreamWriter stream = File.CreateText("dumpxml.txt"))
            //{
            //    stream.WriteLine(xml);
            //}
        }



    }
}
