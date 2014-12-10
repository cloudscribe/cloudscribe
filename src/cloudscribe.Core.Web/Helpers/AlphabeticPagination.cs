// Author:					Joe Audette
// Created:					2014-12-10
// Last Modified:			2014-12-10
// TODO: make this templated

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.Helpers
{
    public static class AlphabeticPagination
    {
        public static MvcHtmlString AlphabeticPager(
            this HtmlHelper html, 
            string selectedLetter,
            string alphabet,
            IEnumerable<string> firstLetters, 
            string allLabel,
            string allValue,
            bool includeNumbers,
            Func<string, string> pageLink)
        {
            var sb = new StringBuilder();
            var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
            List<string> alphabetList;
            if(string.IsNullOrEmpty(alphabet))
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
            

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");
            ul.AddCssClass("alpha");

            foreach (var letter in alphabetList)
            {
                var li = new TagBuilder("li");
                
                // firstletters is a list of which alpha chars actually have results
                // so that ones without data can be non links
                // if not provided then always make a link for every letter

                if (letter == "All" || firstLetters == null || firstLetters.Contains(letter) 
                    || (firstLetters.Intersect(numbers).Any() && letter == "0-9"))
                {
                    if (selectedLetter == letter || string.IsNullOrEmpty(selectedLetter) && letter == "All")
                    {
                        li.AddCssClass("active");
                        var span = new TagBuilder("span");
                        span.SetInnerText(letter);
                        li.InnerHtml = span.ToString();
                    }
                    else
                    {
                        var a = new TagBuilder("a");
                        
                        if(letter == allLabel)
                        {
                            a.MergeAttribute("href", pageLink(allValue));
                        }
                        else
                        {
                            a.MergeAttribute("href", pageLink(letter));
                        }
                        a.InnerHtml = letter;
                        li.InnerHtml = a.ToString();
                    }
                }
                else
                {
                    li.AddCssClass("inactive");
                    var span = new TagBuilder("span");
                    span.SetInnerText(letter);
                    li.InnerHtml = span.ToString();
                }
                sb.Append(li.ToString());
            }
            ul.InnerHtml = sb.ToString();
            return new MvcHtmlString(ul.ToString());
        }

    }
}
