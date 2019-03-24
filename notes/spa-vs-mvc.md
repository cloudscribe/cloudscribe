# Modern Web Development - Thoughts on SPA vs MVC in ASP.NET

Just writing this to think through and take notes of challenges of meeting my goals for integrating "app" features with website content.
~ Joe Audette

SPA (Single Page Application) is ideal for mobile applications. Using polymer web components is a beautiful way to build front end apps along with web api on the backend, with the front end being entirely client side code and markup. I am very attracted to this development model. I know, most people are focused on reactjs, angular,or aurelia, but I'm more attracted to web components, vanillajs, and polymer for front end development.

The down side of SPA is that nothing works at all without javascript. It seems like today most web developers just take it for granted that javascript will be enabled, and in general it is true especially for mobile.

However for security minded users of the web, we know that almost all cases where your computer gets a virus or gets hacked, are cases where javascript is what enables the hack. It is very dangerous to visit random web sites with javascript enabled, especially when searching for information that will land you on sites whose ownership is unkown and untrusted. Lots of sites host malware and the bad guys are trying to make their sites land at the top of any searches that they can capture good ranking for. You may be fully patched but there are zero day exploits traded on the black market and you should not get a flase sense of security just because your software is up to date. There is a lot of clickbait on social media and visiting such links is risky. Similarly, url shorteners are dangerous because you have no idea where it will redirect you to. Lets face it the web is dangerous.

When I do research on the web I use Firefox with the uMatrix plugin which blocks scripts, cookies, css, xhr, etc., allowing you to enable just as much as needed on sites you trust. By blocking cookies it helps with privacy in addition to security. I used to use the NoScript plugin, but uMatrix is much better. I believe that using this approach significantly reduces my risk of getting infected with malware.

However, using this approach I also often get frustrated by sites where you cannot read the content at all with script disabled because the content is loaded from script which as mentioned is typical of SPA. I usually just move on to another search result when I encounter this. 

As a security conscious web developer perhaps I represent a small percentage of users so maybe most stakeholders in a web project don't care about this, but it still bothers me. I think "content" should be visible without javascript enabled, it is sort of a pet peeve of mine. So I see a difference between a website which is content centric and a web application which is task oriented. For a web application that I have decided to use, of course I will enable javascript, but for web content I find through searching I'm not likely to enable it unless it is a well known brand or domain.

## The Problem with SPA

The problem with this as I see it is when you have an app and content combined and you won't know if you want to use or trust the app without first seeing the content. In ecommerce sites for example, if I decide to purchase from the site of course I can enable javascript and I don't mmind if that is required for me to use the cart or checkout features. But, if I cannot browse the product catalog without javascript then I am not likely to get to the point of enabling script (again, unless it is a well known brand site). So if I search for something that lands me on a product page that I cannot view without first enabling javascript, it becomes unlikely that I will be converted to a customer on a site that is not known to me already.

So this is my quandry, how can I make the product catalog accessible without script if I use the SPA approach? It seems I cannot. With MVC approach I can easily do this but it becomes more challenging then to get the benefits that would come with a SPA approach.

DOES THIS EVEN MATTER? 
AM I THE ONLY ONE CONCERNED BY THIS? 
SHOULD I JUST GET OVER IT AND DO SPA LIKE EVERYONE ELSE?

## Possible Solution Strategies

** 1. Don't worry about it, just build a SPA, if later you think sales are being lost because of this you can always make a secondary MVC product catalog and link to it from inside a <NOSCRIPT> element. The problem is that you may not be able to measure this since analytics also requires scripts and cookies enabled. You can comfort yourself with the idea that a very small number of users will be impacted by this and therefore potential losses are very small.

** 2. For content heavy "apps" such as ecommerce, build the product catalog and other content heavy features with MVC and make the checkout and cart links go to a SPA that has a link back to the product catalog and other content areas.


Probably #1 is a reasonable determination and default strategy. Most likely you will only be concerned about the small percentage loss if overall sales are very high such that the small percentage is still a significant amount of revenue. It is reasonble to defer building a secondary more accessible product catalog until then.

## Integration

Marketing and other content centric websites that are not applications per se, probably are still best built with MVC. Often we may want to link together such sites with related apps. It can be a bit jarring to go visually from a website design to an application UI and some care should be taken to make the transition seem smoother by having some common visual cues between both. The transition will typically also entail a full page load to a new page since the SPA is a separate page unto itself.

### Related links

https://journal.plausible.io/you-probably-dont-need-a-single-page-app

https://whatisjasongoldstein.com/writing/help-none-of-my-projects-want-to-be-spas/

http://blog.bloomca.me/2018/02/04/spa-is-not-silver-bullet.html

Ask HN: Anyone getting sick of all the 'web apps'? aka spa style apps
https://news.ycombinator.com/item?id=15506763

http://stackoverflow.com/questions/21097592/mixing-angular-and-asp-net-mvc-web-api

http://stackoverflow.com/questions/20208388/integrating-angularjs-1-2-into-existing-mvc-site

https://medium.freecodecamp.org/why-i-hate-your-single-page-app-f08bb4ff9134

https://tinnedfruit.com/articles/create-your-own-dysfunctional-single-page-app.html

https://dev.to/winduptoy/a-javascript-free-frontend-2d3e




