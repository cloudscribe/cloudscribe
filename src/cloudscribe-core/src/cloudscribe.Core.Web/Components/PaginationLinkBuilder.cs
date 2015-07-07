// Author:					Joe Audette
// Created:					2015-07-06
// Last Modified:			2015-07-06
//
// borrowed some code for building the list of links from Martijn Boland
// https://github.com/martijnboland/MvcPaging/blob/master/src/MvcPaging/Pager.cs MIT License

using cloudscribe.Core.Web.ViewModels;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.Components
{
    public class PaginationLinkBuilder : IBuildPaginationLinks
    {


        public List<PaginationLink> BuildPaginationLinks(
            PaginationSettings paginationSettings, 
            Func<int, string> generateUrl,
            string firstPageText,
            string firstPageTitle,
            string previousPageText,
            string previousPageTitle,
            string nextPageText,
            string nextPageTitle,
            string lastPageText,
            string lastPageTitle,
            string spacerText = "...")
        {
            List<PaginationLink> paginationLinks = new List<PaginationLink>();

            int totalPages = (int)Math.Ceiling(paginationSettings.TotalItems / (double)paginationSettings.ItemsPerPage);

            // First page
            if (paginationSettings.ShowFirstLast)
            {
                paginationLinks.Add(
                    new PaginationLink
                    {
                        Active = (paginationSettings.CurrentPage > 1 ? true : false),
                        DisplayText = firstPageText,
                        DisplayTitle = firstPageTitle,
                        PageIndex = 1,
                        Url = generateUrl(1)
                    });
            }

            // Previous page
            paginationLinks.Add(
                paginationSettings.CurrentPage > 1 ? new PaginationLink
                {
                    Active = true,
                    DisplayText = previousPageText,
                    DisplayTitle = previousPageTitle,
                    PageIndex = paginationSettings.CurrentPage - 1,
                    Url = generateUrl(paginationSettings.CurrentPage - 1)
                } : new PaginationLink { Active = false, DisplayText = previousPageText });

            var start = 1;
            var end = totalPages;
            var nrOfPagesToDisplay = paginationSettings.MaxPagerItems;

            if (totalPages > nrOfPagesToDisplay)
            {
                var middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
                var below = (paginationSettings.CurrentPage - middle);
                var above = (paginationSettings.CurrentPage + middle);

                if (below < 2)
                {
                    above = nrOfPagesToDisplay;
                    below = 1;
                }
                else if (above > (totalPages - 2))
                {
                    above = totalPages;
                    below = (totalPages - nrOfPagesToDisplay + 1);
                }

                start = below;
                end = above;
            }

            if (start > 1)
            {
                paginationLinks.Add(new PaginationLink
                {
                    Active = true,
                    PageIndex = 1,
                    DisplayText = "1",
                    Url = generateUrl(1)
                });

                if (start > 3)
                {
                    paginationLinks.Add(new PaginationLink
                    {
                        Active = true,
                        PageIndex = 2,
                        DisplayText = "2",
                        Url = generateUrl(2)
                    });
                }

                if (start > 2)
                {
                    paginationLinks.Add(new PaginationLink
                    {
                        Active = false,
                        DisplayText = spacerText,
                        IsSpacer = true
                    });
                }
            }

            for (var i = start; i <= end; i++)
            {
                if (i == paginationSettings.CurrentPage || (paginationSettings.CurrentPage <= 0 && i == 1))
                {
                    paginationLinks.Add(new PaginationLink
                    {
                        Active = true,
                        PageIndex = i,
                        IsCurrent = true,
                        DisplayText = i.ToString()
                    });
                }
                else
                {
                    paginationLinks.Add(new PaginationLink
                    {
                        Active = true,
                        PageIndex = i,
                        DisplayText = i.ToString(),
                        Url = generateUrl(i)
                    });
                }
            }

            if (end < totalPages)
            {
                if (end < totalPages - 1)
                {
                    paginationLinks.Add(new PaginationLink
                    {
                        Active = false,
                        DisplayText = spacerText,
                        IsSpacer = true
                    });
                }
                if (totalPages - 2 > end)
                {
                    paginationLinks.Add(new PaginationLink
                    {
                        Active = true,
                        PageIndex = totalPages - 1,
                        DisplayText = (totalPages - 1).ToString(),
                        Url = generateUrl(totalPages - 1)
                    });
                }

                paginationLinks.Add(new PaginationLink
                {
                    Active = true,
                    PageIndex = totalPages,
                    DisplayText = totalPages.ToString(),
                    Url = generateUrl(totalPages)
                });
            }

            // Next page
            paginationLinks.Add(
                paginationSettings.CurrentPage < totalPages ? new PaginationLink
                {
                    Active = true,
                    PageIndex = paginationSettings.CurrentPage + 1,
                    DisplayText = nextPageText,
                    DisplayTitle = nextPageTitle,
                    Url = generateUrl(paginationSettings.CurrentPage + 1)
                }
                : new PaginationLink
                {
                    Active = false,
                    DisplayText = nextPageText
                });

            // Last page
            if (paginationSettings.ShowFirstLast)
            {
                paginationLinks.Add(new PaginationLink
                {
                    Active = (paginationSettings.CurrentPage < totalPages ? true : false),
                    DisplayText = lastPageText,
                    DisplayTitle = lastPageTitle,
                    PageIndex = totalPages,
                    Url = generateUrl(totalPages)
                });
            }

            
            return paginationLinks;

        }

        

    }
}
