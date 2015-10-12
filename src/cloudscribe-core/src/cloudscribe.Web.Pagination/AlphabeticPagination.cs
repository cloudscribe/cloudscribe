// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-12-10
// Last Modified:			2015-09-06



using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudscribe.Web.Pagination
{
    public static class AlphabeticPagination
    {
        public static HtmlString AlphabeticPager(
            this IHtmlHelper html,
            string selectedLetter,
            string alphabet,
            IEnumerable<string> firstLetters,
            string allLabel,
            string allValue,
            bool includeNumbers,
            Func<string, string> pageLink)
        {
            //TagBuilder got funky in beta7 http://stackoverflow.com/questions/32416425/tagbuilder-innerhtml-in-asp-net-5-mvc-6
            // so just using stringbuilder as of 2015-09-06
            // probably can change back to TagBuilder after beta 8 as it should then have an .Append method to set innerhtml

            var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
            List<string> alphabetList;
            if (string.IsNullOrEmpty(alphabet))
            {
                alphabetList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToStringList();
            }
            else
            {
                alphabetList = alphabet.ToCharArray().ToStringList();
            }

            if (string.IsNullOrEmpty(allLabel))
            {
                allLabel = "All";
            }
            alphabetList.Insert(0, allLabel);
            if (includeNumbers)
            {
                alphabetList.Insert(1, "0-9");
            }

            var sb = new StringBuilder();

            //var ul = new TagBuilder("ul");
            //ul.AddCssClass("pagination");
            //ul.AddCssClass("alpha");
            sb.Append("<ul class='pagination alpha'>");


            //BufferedHtmlContent innerHtml = new BufferedHtmlContent();

            foreach (var letter in alphabetList)
            {
                //var li = new TagBuilder("li");
                sb.Append("<li");

                // firstletters is a list of which alpha chars actually have results
                // so that ones without data can be non links
                // if not provided then always make a link for every letter

                if (letter == "All" || firstLetters == null || firstLetters.Contains(letter)
                    || (firstLetters.Intersect(numbers).Any() && letter == "0-9"))
                {
                    if (selectedLetter == letter || string.IsNullOrEmpty(selectedLetter) && letter == "All")
                    {
                        //li.AddCssClass("active");
                        //var span = new TagBuilder("span");
                        //span.SetInnerText(letter);
                        //li.InnerHtml = span;
                        sb.Append(" class='active'>");
                        sb.Append("<span>" + letter + "</span>");
                        sb.Append("</li>");

                    }
                    else
                    {
                        sb.Append(">");

                        //var a = new TagBuilder("a");
                        sb.Append("<a href='");

                        if (letter == allLabel)
                        {
                            sb.Append(pageLink(allValue) + "'");
                            //a.MergeAttribute("href", pageLink(allValue));
                        }
                        else
                        {
                            sb.Append(pageLink(letter) + "'");
                            // a.MergeAttribute("href", pageLink(letter));
                        }
                        sb.Append(">");

                        //a.SetInnerText(letter);
                        sb.Append(letter);
                        sb.Append("</a>");
                        //li.InnerHtml = a;
                        sb.Append("</li>");

                    }
                }
                else
                {
                    sb.Append(" class='inactive'>");
                    sb.Append("<span>" + letter + "</span>");
                    sb.Append("</li>");

                    //li.AddCssClass("inactive");
                    //var span = new TagBuilder("span");
                    //span.SetInnerText(letter);
                    //li.InnerHtml = span;

                    

                }
                //sb.Append(li.ToString());
            }
            //ul.InnerHtml = sb.ToString();
            //ul.InnerHtml = new HtmlString(sb.ToString());
            sb.Append("</ul>");



            return new HtmlString(sb.ToString());
        }

    }
}
