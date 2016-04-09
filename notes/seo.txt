
https://ma.ttias.be/technical-guide-seo/


http://www.dailyblogtips.com/change-wordpress-permalinks/


https://yoast.com/rss-feeds-panda-penguin/
https://blog.superfeedr.com/feeds/rss/atom/best%20practice/feed-publishing-best-practices/
http://www.webmonkey.com/2010/02/rss_for_beginnners/


https://www.elegantthemes.com/blog/tips-tricks/wordpress-permalinks

http://www.wpbeginner.com/opinion/why-you-should-not-remove-dates-from-your-wordpress-blog-posts/

http://webmasters.stackexchange.com/questions/49781/benefit-of-date-in-url-segments

//[Route("date/{pubdate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
//[Route("date/{*pubdate:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]  // new
http://www.asp.net/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing


http://weblogs.asp.net/jongalloway/looking-at-asp-net-mvc-5-1-and-web-api-2-1-part-2-attribute-routing-with-custom-constraints
routes.MapRoute("blog", "{year}/{month}/{day}",
    new { controller = "blog", action = "index" },
    new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" });
	
http://stackoverflow.com/questions/2348554/trouble-using-url-action-when-trying-to-create-friendly-hackable-urls

 3
down vote
accepted
	

The issue there is that the month you're passing in to Url.Action is a single-digit month, and thus doesn't match the month constraint on the route definition. Constraints are typically run not only on incoming URLs but also when generating URLs.

The fix would be to manually call .ToString() on the month and format it as a two-digit number. You'll need to do the same for the day as well. For the year it's not an issue since all the years in our lifetimes will be four-digit numbers.

Here's sample code to format numbers:

int month = 2;
string formattedMonth = month.ToString("00", CultureInfo.InvariantCulture);
// formattedMonth == "02"

Please note that when formatting the number manually that you must use the Invariant Culture so that different cultures and languages won't affect how it gets formatted.

You'll also need to set default values for month and day so that they are not required in the URL:

routes.MapRoute( 
  "Blog", 
  "blog/{year}/{month}/{day}", 
  new { controller = "Blog", action = "Index", month = "00", day = "00" }, 
  new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" } 
);

And in your controller action check if the month or day are 0, in which case you should show an entire month or entire year.

	

Do dates in URLs determine freshness? 
https://www.youtube.com/watch?v=lIfCERXLlDM

http://blog.2partsmagic.com/restful-uri-design/

This is a good URL design.

    The URLs are can be persistent (they contain no parts that are likely to change; name changes are rare).
    The URLs are descriptive (users can read the URL and they’ll get an idea what the page is about).
    The URLs are browsable (users can remove path segments from right to left, and they’ll get no 404).

It’s also good that they don’t contain any "unnecessary" parts (like, for example, cryptic IDs for person/gallery names). Which, of course, means that you have to take care of edge cases like duplicate person names (maybe append an integer) or same person’s duplicate gallery names (maybe forbid these).

I assume that users get some kind of list of all teachers/students when visiting /teacher resp. /student. In this case, I’d probably use the plural forms /teachers* and /students*:

/teachers
/teachers/john-doe
/teachers/john-doe/biography

http://www.hanselman.com/blog/AreBlogURLsImportant.aspx

http://blog.codinghorror.com/dont-devalue-the-address-bar/

http://stackoverflow.com/questions/335575/simple-hackable-url-implementation-for-asp-net-3-5

https://msdn.microsoft.com/en-us/library/ms972974.aspx#urlrewriting_topic7

http://blog.ploeh.dk/2013/05/01/rest-lesson-learned-avoid-hackable-urls/