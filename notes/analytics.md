
https://philipwalton.com/articles/the-google-analytics-setup-i-use-on-every-site-i-build/

https://github.com/googleanalytics/autotrack

https://github.com/philipwalton/analyticsjs-boilerplate

http://googledevelopers.blogspot.com/2016/02/introducing-autotrack-for-analyticsjs.html

forceSSL
By default, tracking beacons sent from https pages will be sent using https while beacons sent from http pages will be sent using http. Setting forceSSL to true will force http pages to also send all beacons using https.

Display Features
https://developers.google.com/analytics/devguides/collection/analyticsjs/display-features
https://support.google.com/analytics/answer/3450482

https://www.themarketingtechnologist.co/setting-up-a-cookie-law-compliant-google-analytics-tracker/

## Tracking Users

https://support.google.com/analytics/answer/3123662?hl=en

You must make sure you have the full rights to use this service, to upload data, and to use it with your Google Analytics account.
You will give your end users proper notice about the implementations and features of Google Analytics you use (e.g. notice about what data you will collect via Google Analytics, and whether this data can be connected to other data you have about the end user). You will either get consent from your end users, or provide them with the opportunity to opt-out from the implementations and features you use.
You will not upload any data that allows Google to personally identify an individual (such as certain names, social security numbers, email addresses, or any similar data), or data that permanently identifies a particular device (such as a mobile phone’s unique device identifier if such an identifier cannot be reset).
If you upload any data that allows Google to personally identify an individual, your Google Analytics account can be terminated, and you may lose your Google Analytics data.
You will only session stitch authenticated and unauthenticated sessions of your end users if your end users have given consent to such stitch, or if such merger is allowed under applicable laws and regulations.

PII Viewer for Google Analytics is a Google Chrome extension which allows you to map the user ID stored in Google Analytics to locally stored personally identifiable information (PII) such as name and email address.
https://davidsimpson.me/pii-viewer-for-google-analytics/
https://davidsimpson.me/2014/04/20/tutorial-send-user-ids-google-analytics/

http://www.charlesfarina.com/login-and-signup-naming-conventions-for-google-analytics/

userid vs &uid  short answer they are equivalent in js usage
https://stackoverflow.com/questions/23379338/set-google-analytics-user-id-after-creating-the-tracker

ga('set', '&uid', '1234567');

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
https://www.cloudscribe.com/?utm_source=vsix&amp;utm_medium=referral&amp;utm_campaign=vsix

### server side tracking

https://stackoverflow.com/questions/9503329/is-there-any-way-to-post-events-to-google-analytics-via-server-side-api

https://developers.google.com/analytics/devguides/collection/protocol/v1/
https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide
https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters

http://localhost:50827/campaignimages/cloudscribe-logo650pt.png?utm_source=projecttemplate&utm_medium=referral&utm_campaign=newproject-mytest

http://localhost:50827/

## Views

About views
https://support.google.com/analytics/answer/2649553?hl=en

Adding views
https://support.google.com/analytics/answer/1009714?hl=en

If you enable UserId in a view then only traffic with a userid provided will appear in that view

## Filtering

https://www.lunametrics.com/blog/2015/12/10/basic-google-analytics-filters/

when you use a filter for ip address such as include or exclude internal traffic 
https://stackoverflow.com/questions/30872288/google-analytics-cant-seem-to-exclude-own-traffic
it won't work if ip anonymization is enabled

https://support.google.com/analytics/answer/2763052?hl=en

## Dimensions and Metrics

Dimensions are attributes of your data. For example, the dimension City indicates the city, for example, "Paris" or "New York", from which a session originates. The dimension Page indicates the URL of a page that is viewed.

Metrics are quantitative measurements. The metric Sessions is the total number of sessions. The metric Pages/Session is the average number of pages viewed per session.

Default Dimensions and Metrics
https://support.google.com/analytics/answer/1033861


Create and edit custom dimensions and metrics
https://support.google.com/analytics/answer/2709829?hl=en

## Custom Dimenstions and metircs used in cloudscribe

Define these in your google analytics per prooperty where using cloudscribe

### Custom Dimensions
Name = User ID (Hit Scoped)
   Index = 1
   Scope = Hit //Pass this on all hits while logged in for GA User-ID feature. See Simo for options and GA for benefits
   Value = <User ID> //Pass ID from your database
   
Name = Registered User
    Index = 2
    Scope = User
    Value = Yes
	
Name = Login/Register Source
    Index = 3
    Scope = Hit
    Value = <Method> //Facebook, Twitter, Onsite
	
	
### Custom Metrics

Name = Register Success
    Index = 1
    Value = 1
    Minimum Value = 1
    Maximum Value = 1
    Formating Type = Integer
	
Name = Register Fail
    Index = 2
    Value = 1
    Minimum Value = 1
    Maximum Value = 1
    Formating Type = Integer
	
Name = Register Submit
    Index = 3
    Value = 1
    Minimum Value = 1
    Maximum Value = 1
    Formating Type = Integer
	
Name = Login Success
    Index = 4
    Value = 1
    Minimum Value = 1
    Maximum Value = 1
    Formating Type = Integer
	
Name = Login Fail
    Index = 5
    Value = 1
    Minimum Value = 1
    Maximum Value = 1
    Formating Type = Integer
	
Name = Login Submit
   Index = 6
   Value = 1
   Minimum Value = 1
   Maximum Value = 1
   Formating Type = Integer
   
 Name = Newsletter Signup Submit
   Index = 7
   Value = 1
   Minimum Value = 1
   Maximum Value = 1
   Formating Type = Integer
   
 Name = Newsletter Signup Confirmed
   Index = 8
   Value = 1
   Minimum Value = 1
   Maximum Value = 1
   Formating Type = Integer
   
Name = Newsletter Signup Success
   Index = 9
   Value = 1
   Minimum Value = 1
   Maximum Value = 1
   Formating Type = Integer
   
Name = Newsletter Signup Fail
   Index = 10
   Value = 1
   Minimum Value = 1
   Maximum Value = 1
   Formating Type = Integer
   

## Events

https://support.google.com/analytics/answer/1033068?hl=en

https://developers.google.com/analytics/devguides/collection/analyticsjs/events

Anatomy of Events 
https://support.google.com/analytics/answer/1033068#Anatomy

ga('send', 'event', [eventCategory], [eventAction], [eventLabel], [eventValue], [fieldsObject]);
Event fields

The following table summarizes the event fields:

Field Name	Value Type	Required	Description
eventCategory	text	yes	Typically the object that was interacted with (e.g. 'Video')
eventAction	text	yes	The type of interaction (e.g. 'play')
eventLabel	text	no	Useful for categorizing events (e.g. 'Fall Campaign')
eventValue	integer	no	A numeric value associated with the event (e.g. 42)

## Setting Goals

https://www.google.com/search?q=setting+goals+in+google+analytics

https://blog.kissmetrics.com/critical-goal-types/

http://online-metrics.com/analytics-goals/

## Tracking Email opens and non-interaction events

https://www.lunametrics.com/blog/2014/05/06/noninteraction-events-google-analytics/

https://developers.google.com/analytics/devguides/collection/protocol/v1/email

Event
    Category = Offsite Interactions
    Action = Project Created
    Label = newproject-mssql, newproject-pgsql, newproject-nodb, newproject-mysql
    Non-Interaction = true
  Custom Dimensions
    Name = Project Type (Hit Scoped)
      Index = 4
      Scope = Hit 
      Value = newproject-mssql, newproject-pgsql, newproject-nodb, newproject-mysql
  Custom Metrics
    Name = Project Creation
      Index = 7
      Value = 1
      Minimum Value = 1
      Maximum Value = 1
      Formating Type = Integer
	  
## Tracking Ecommerce

https://developers.google.com/analytics/devguides/collection/analyticsjs/ecommerce

ga('require', 'ecommerce');

Adding a Transaction

Once the plugin has been loaded, it creates a transparent shopping cart object. You can add transaction and item data to the shopping cart, and once fully configured, you send all the data at once.

You add transaction data to the shopping cart using the ecommerce:addTransaction command:

ga('ecommerce:addTransaction', {
  'id': '1234',                     // Transaction ID. Required.
  'affiliation': 'Acme Clothing',   // Affiliation or store name.
  'revenue': '11.99',               // Grand Total.
  'shipping': '5',                  // Shipping.
  'tax': '1.29'                     // Tax.
});

Adding Items

Next, to add items to the shopping cart, you use the ecommerce:addItem command:

ga('ecommerce:addItem', {
  'id': '1234',                     // Transaction ID. Required.
  'name': 'Fluffy Pink Bunnies',    // Product name. Required.
  'sku': 'DD23444',                 // SKU/code.
  'category': 'Party Toys',         // Category or variation.
  'price': '11.99',                 // Unit price.
  'quantity': '1'                   // Quantity.
});

Sending Data

Finally, once you have configured all your ecommerce data in the shopping cart, you send the data to Google Analytics using the ecommerce:send command:

ga('ecommerce:send');

This command will go through each transaction and item in the shopping cart and send the respective data to Google Analytics. Once complete, the shopping cart is cleared and ready to send data for a new transaction. If a previous ecommerce:send command was issued, only new transaction and item data will be sent.

Note: While most implementations will send both transaction and item data, you can send transactions without items, and items without transactions. If you send an item hit without a transaction, a transaction hit with only the ID will be sent automatically.
	  