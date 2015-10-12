// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Authors:					Martijn Boland/Joe Audette
// Created:					2015-07-08
// Last Modified:			2015-07-15
// 
// borrowed/modified from Martijn Boland
// trying to replicate some of the tests that Martijn Boland did here
// https://github.com/martijnboland/MvcPaging/blob/master/src/MvcPaging.Tests/PagerTests.cs

using cloudscribe.Web.Pagination;
using System.Collections.Generic;
using Xunit;



namespace cloudscribe.Web.Navigation.Test
{
    //2015-07-08 just trying to get unit testing infrastructure setup with working examples.
    // currently I'm unable to get projects supporting dnx451 on beta5
    // and unforutnately Moq currently does not work on dnxcore50
    // so we may be limited for the moment until the dnx451 packaging issues can be resolved
    // perhaps beta6 will solve that and then we can use dnx451 to run tests (as the aspnet team does currently I think)

    public class PaginationTests
    {

        [Fact]
        public void Can_Build_Correct_Model_For_5_Items_With_2_Item_Per_Page()
        {
            // Assemble

            var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = false, Text = previousPageText, PageNumber = 1, Url = "/test/1" },
                new PaginationLink { Active = true, Text = "1", PageNumber = 1, Url = "/test/1", IsCurrent = true },
                new PaginationLink { Active = true, Text = "2", PageNumber = 2, Url = "/test/2" },
                new PaginationLink { Active = true, Text = "3", PageNumber = 3, Url = "/test/3" },
                new PaginationLink { Active = true, Text = nextPageText, PageNumber = 2, Url = "/test/2" }
            };

            var settings = new PaginationSettings { CurrentPage = 1, ItemsPerPage = 2, TotalItems = 5 };
            var builder = new PaginationLinkBuilder();

            //Act
            var result = builder.BuildPaginationLinks(
                settings,
                generateUrl,
                firstPageText,
                firstPageTitle,
                previousPageText,
                previousPageTitle,
                nextPageText,
                nextPageTitle,
                lastPageText,
                lastPageTitle,
                spacerText);


            //Assert
            Assert.Equal(expectedPagination.Count, result.Count);
            PaginationComparer comparer = new PaginationComparer();
            for (int i = 0; i < expectedPagination.Count; i++)
            {
                Assert.Equal(expectedPagination[i], result[i], comparer);
            }


        }

        [Fact]
        public void Can_Build_Correct_Model_For_10_Items_With_2_Item_Per_Page()
        {
            // Assemble
            var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = true, Text = "«", PageNumber = 2, Url = "/test/2" },
                new PaginationLink { Active = true, Text = "1", PageNumber = 1, Url = "/test/1" },
                new PaginationLink { Active = true, Text = "2", PageNumber = 2, Url = "/test/2" },
                new PaginationLink { Active = true, Text = "3", PageNumber = 3, Url = "/test/3", IsCurrent = true },
                new PaginationLink { Active = true, Text = "4", PageNumber = 4, Url = "/test/4" },
                new PaginationLink { Active = true, Text = "5", PageNumber = 5, Url = "/test/5" },
                new PaginationLink { Active = true, Text = "»", PageNumber = 4, Url = "/test/4" }
            };

            var settings = new PaginationSettings { CurrentPage = 3, ItemsPerPage = 2, TotalItems = 10 };
            var builder = new PaginationLinkBuilder();

            //Act
            var result = builder.BuildPaginationLinks(
                settings,
                generateUrl,
                firstPageText,
                firstPageTitle,
                previousPageText,
                previousPageTitle,
                nextPageText,
                nextPageTitle,
                lastPageText,
                lastPageTitle,
                spacerText);


            //Assert
            Assert.Equal(expectedPagination.Count, result.Count);
            PaginationComparer comparer = new PaginationComparer();
            for (int i = 0; i < expectedPagination.Count; i++)
            {
                Assert.Equal(expectedPagination[i], result[i], comparer);
            }
        }

        [Fact]
        public void Can_Build_Correct_Model_For_33_Items_With_2_Item_Per_Page()
        {
            // Assemble
            var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = true, Text = "«", PageNumber = 12, Url = "/test/12" },
                new PaginationLink { Active = true, Text = "1", PageNumber = 1, Url = "/test/1" },
                new PaginationLink { Active = true, Text = "2", PageNumber = 2, Url = "/test/2" },
                new PaginationLink { Active = false, Text = "...", Url = "", IsSpacer = true },
                new PaginationLink { Active = true, Text = "8", PageNumber = 8, Url = "/test/8" },
                new PaginationLink { Active = true, Text = "9", PageNumber = 9, Url = "/test/9" },
                new PaginationLink { Active = true, Text = "10", PageNumber = 10, Url = "/test/10" },
                new PaginationLink { Active = true, Text = "11", PageNumber = 11, Url = "/test/11" },
                new PaginationLink { Active = true, Text = "12", PageNumber = 12, Url = "/test/12" },
                new PaginationLink { Active = true, Text = "13", PageNumber = 13, Url = "/test/13", IsCurrent = true },
                new PaginationLink { Active = true, Text = "14", PageNumber = 14, Url = "/test/14" },
                new PaginationLink { Active = true, Text = "15", PageNumber = 15, Url = "/test/15" },
                new PaginationLink { Active = true, Text = "16", PageNumber = 16, Url = "/test/16" },
                new PaginationLink { Active = true, Text = "17", PageNumber = 17, Url = "/test/17" },
                new PaginationLink { Active = true, Text = "»", PageNumber = 14, Url = "/test/14" }
            };

            var settings = new PaginationSettings { CurrentPage = 13, ItemsPerPage = 2, TotalItems = 33 };
            var builder = new PaginationLinkBuilder();

            //Act
            var result = builder.BuildPaginationLinks(
                settings,
                generateUrl,
                firstPageText,
                firstPageTitle,
                previousPageText,
                previousPageTitle,
                nextPageText,
                nextPageTitle,
                lastPageText,
                lastPageTitle,
                spacerText);


            //Assert
            Assert.Equal(expectedPagination.Count, result.Count);
            PaginationComparer comparer = new PaginationComparer();
            for (int i = 0; i < expectedPagination.Count; i++)
            {
                Assert.Equal(expectedPagination[i], result[i], comparer);
            }

        }

        /// <summary>
        /// This test by Martijn taught me that MaxPagerItems is not what I thought
        /// the expectedPagination shows that 10 PaginationLinks are expected when the MaxPagerItems = 5
        /// and the test passes so it does what Martijn expected
        /// perhaps I should name it something different than MaxPagerItems
        /// it is really max pager links before spacer and leap links
        /// </summary>
        [Fact]
        public void Can_Build_Correct_Model_For_33_Items_With_2_Item_Per_Page_And_Max_5_Pages()
        {
            // Assemble
            var expectedPagination = new List<PaginationLink>()
            {
                new PaginationLink { Active = false, Text = "«", PageNumber = 1, Url = "/test/1" },
                new PaginationLink { Active = true, Text = "1", PageNumber = 1, Url = "/test/1", IsCurrent = true },
                new PaginationLink { Active = true, Text = "2", PageNumber = 2, Url = "/test/2"},
                new PaginationLink { Active = true, Text = "3", PageNumber = 3, Url = "/test/3" },
                new PaginationLink { Active = true, Text = "4", PageNumber = 4, Url = "/test/4" },
                new PaginationLink { Active = true, Text = "5", PageNumber = 5, Url = "/test/5" },
                new PaginationLink { Active = false, Text = "...", Url = "", IsSpacer = true },
                new PaginationLink { Active = true, Text = "16", PageNumber = 16, Url = "/test/16" },
                new PaginationLink { Active = true, Text = "17", PageNumber = 17, Url = "/test/17" },
                new PaginationLink { Active = true, Text = "»", PageNumber = 2, Url = "/test/2" }
            };

            var settings = new PaginationSettings { CurrentPage = 1, ItemsPerPage = 2, TotalItems = 33, MaxPagerItems = 5 };
            var builder = new PaginationLinkBuilder();

            //Act
            var result = builder.BuildPaginationLinks(
                settings,
                generateUrl,
                firstPageText,
                firstPageTitle,
                previousPageText,
                previousPageTitle,
                nextPageText,
                nextPageTitle,
                lastPageText,
                lastPageTitle,
                spacerText);


            //Assert
            Assert.Equal(expectedPagination.Count, result.Count);
            PaginationComparer comparer = new PaginationComparer();
            for (int i = 0; i < expectedPagination.Count; i++)
            {
                Assert.Equal(expectedPagination[i], result[i], comparer);
            }

        }

        private string generateUrl(int pageNumber)
        {
            return string.Format("/test/{0}", pageNumber);
        }

        private const string firstPageText = "<";
        private const string firstPageTitle = "First Page";
        private const string previousPageText = "«";
        private const string previousPageTitle = "Previous page";
        private const string nextPageText = "»";
        private const string nextPageTitle = "Next page";
        private const string lastPageText = ">";
        private const string lastPageTitle = "Last page";
        private const string spacerText = "...";

        internal class PaginationComparer : IEqualityComparer<PaginationLink>
        {

            bool IEqualityComparer<PaginationLink>.Equals(PaginationLink x, PaginationLink y)
            {
                var first = (PaginationLink)x;
                var second = (PaginationLink)y;

                var displayTextResult = first.Text.CompareTo(second.Text);
                if (displayTextResult != 0) return false;

                var urlResult = first.Url.CompareTo(second.Url);
                if (urlResult != 0) return false;

                var pageIndexResult = first.PageNumber.CompareTo(second.PageNumber);
                if (pageIndexResult != 0) return false;

                var activeResult = first.Active.CompareTo(second.Active);
                if (activeResult != 0) return false;

                var isCurrentResult = first.IsCurrent.CompareTo(second.IsCurrent);
                if (isCurrentResult != 0) return false;

                return true;
            }

            int IEqualityComparer<PaginationLink>.GetHashCode(PaginationLink obj)
            {
                return obj.GetHashCode();
            }
        }


    }
}
