// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-09
// Last Modified:			2015-07-11
// 
// borrowed/modified from Martijn Boland
// trying to replicate some of the tests that Martijn Boland did here
// https://github.com/martijnboland/MvcPaging/blob/master/src/MvcPaging.Tests/PagerTests.cs

using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Navigation;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace cloudscribe.Core.Web.Test.Navigation
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
            NavigationTreeBuilder builder = new NavigationTreeBuilder();

            //Act
            string json = builder.GetTree().ToJson();



            //Assert
            Assert.True(!string.IsNullOrEmpty(json));

            //using (StreamWriter stream = File.CreateText("dumpjson.txt"))
            //{
            //    stream.WriteLine(json);
            //}

                //System.Console.WriteLine(json);

            //output.WriteLine(json);
        }

    }
 }
