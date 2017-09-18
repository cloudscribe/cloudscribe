
https://philipwalton.com/articles/the-google-analytics-setup-i-use-on-every-site-i-build/

https://github.com/googleanalytics/autotrack

https://github.com/philipwalton/analyticsjs-boilerplate

http://googledevelopers.blogspot.com/2016/02/introducing-autotrack-for-analyticsjs.html

## Tracking Users

https://support.google.com/analytics/answer/3123662?hl=en

You must make sure you have the full rights to use this service, to upload data, and to use it with your Google Analytics account.
You will give your end users proper notice about the implementations and features of Google Analytics you use (e.g. notice about what data you will collect via Google Analytics, and whether this data can be connected to other data you have about the end user). You will either get consent from your end users, or provide them with the opportunity to opt-out from the implementations and features you use.
You will not upload any data that allows Google to personally identify an individual (such as certain names, social security numbers, email addresses, or any similar data), or data that permanently identifies a particular device (such as a mobile phone’s unique device identifier if such an identifier cannot be reset).
If you upload any data that allows Google to personally identify an individual, your Google Analytics account can be terminated, and you may lose your Google Analytics data.
You will only session stitch authenticated and unauthenticated sessions of your end users if your end users have given consent to such stitch, or if such merger is allowed under applicable laws and regulations.

PII Viewer for Google Analytics is a Google Chrome extension which allows you to map the user ID stored in Google Analytics to locally stored personally identifiable information (PII) such as name and email address.
https://davidsimpson.me/pii-viewer-for-google-analytics/

## Tracking code notes

https://support.google.com/analytics/answer/1033863?visit_id=1-636413345736273758-1215674834&rd=1

utm_source: Identify the advertiser, site, publication, etc. that is sending traffic to your property, for example: google, newsletter4, billboard.

utm_medium: The advertising or marketing medium, for example: cpc, banner, email newsletter.
most common: 
    referral = link from a website
	organic = organic search no advertisement boost
	none - directly typed url
	not set - no parameter

utm_campaign: The individual campaign name, slogan, promo code, etc. for a product.

utm_term: Identify paid search keywords. If you're manually tagging paid keyword campaigns, you should also use utm_term to specify the keyword.

utm_content: Used to differentiate similar content, or links within the same ad. For example, if you have two call-to-action links within the same email message, you can use utm_content and set different values for each so you can tell which version is more effective.



https://www.cloudscribe.com/?utm_source=trailwise.org.uk&amp;utm_medium=referral&amp;utm_campaign=poweredby

### server side tracking

https://stackoverflow.com/questions/9503329/is-there-any-way-to-post-events-to-google-analytics-via-server-side-api
