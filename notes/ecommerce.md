# Ecommerce Notes

## General

https://www.cnet.com/news/companies-must-let-customers-cancel-subscriptions-online-california-law-says/

https://blogs.dropbox.com/tech/2017/09/handling-system-failures-during-payment-communication/


## Square aka squareup.com

https://squareup.com/pos/payments
https://squareup.com/help/us/en/article/3853-square-s-e-commerce-api-faqs
2.75% PER SWIPE, DIP, OR TAP 
3.5% + 15¢ PER KEYED-IN TRANSACTION
2.9% + .30 per api tramsaction

Square does not yet support asp.net core
https://github.com/square/connect-csharp-sdk
I ported it myself
https://github.com/joeaudette/square.netstandard
https://www.nuget.org/packages?q=Square.NetStandard

javascript form
https://docs.connect.squareup.com/payments/sqpaymentform/sqpaymentform-overview

https://docs.connect.squareup.com/payments/sqpaymentform/sqpaymentform-setup

https://docs.connect.squareup.com/articles/using-sandbox

https://github.com/square/connect-csharp-sdk/blob/master/docs/TransactionsApi.md#charge
https://github.com/square/connect-csharp-sdk/blob/master/docs/TransactionsApi.md#createrefund
https://squareup.com/dashboard/business/receipt
https://github.com/square/connect-csharp-sdk/blob/master/docs/TransactionsApi.md#listtransactions
https://github.com/square/connect-csharp-sdk/blob/master/docs/OrdersApi.md#batchretrieveorders

Refunds
https://squareup.com/help/us/en/article/5060

https://github.com/square/connect-api-examples/tree/master/connect-examples/v2/csharp_payment

https://connect.squareup.com/apps
https://github.com/square/connect-csharp-sdk
https://docs.connect.squareup.com/
https://docs.connect.squareup.com/api/connect/v2
https://docs.connect.squareup.com/articles/processing-payment-rest

Test Card
Card Number 4532 7597 3454 5858
Card CVV 111
Card Expiration (Any time in the future)
Card Postal Code (Any valid US postal code)

### Recurring Payments/Subscriptions

https://squareup.com/townsquare/how-to-recurring-payments
> I’m a developer and I want to bill my customers automatically in app - can I integrate Square’s recurring billing feature via API?
Not yet - but we plan to! Stay in touch with new releases and updates to our payments APIs by following our Developers Portal.

https://squareup.com/help/us/en/article/5096-process-recurring-or-subscription-payments


## Stripe

https://stripe.com

https://github.com/stripe/stripe-dotnet
https://www.nuget.org/packages/Stripe.net/

https://stripe.com/us/pricing
2.9%  + 30¢ per transaction
per successful card charge
fees icon No setup, monthly, or hidden fees

https://stripe.com/docs/checkout

Learn how to migrate from the legacy version of Checkout to the new version.
https://stripe.com/docs/payments/checkout/migration

https://stripe.com/docs/stripe-js/elements/quickstart

https://stripe.com/docs/saving-cards

https://stripe.com/docs/recipes/updating-customer-cards

https://stripe.com/docs/testing#cards

The API supports idempotency for safely retrying requests without accidentally performing the same operation twice. 
https://stripe.com/docs/api?lang=curl#idempotent_requests

https://stripe.com/docs/disputes/prevention#make-use-of-verification-checks

### Customer flow

if the site user does not yet have a stripe customer id we show the credit card form, and use the token to create the customer.

For future transactions we just use the customer id to charge the customer.
We can have a form where the customer can change his payment method or add a new different card and make it the default.

### Recurring Payments/Subscriptions

it is not possible for one Customer to have subscriptions with different payment sources. All invoices for subscriptions are always billed to the current default_source for the Customer. So if you change the default as Giles Bennett suggested, you'll be changing it for all subscriptions, regardless of what the default was at time of creation.

If you need one user to have subscriptions with more than one source, you need to create multiple stripe Customer objects for that user, with a different default_source for each.

Plans
Plans define the base price, currency, and billing cycle for subscriptions. For example, you might have a $5/month plan that provides limited access to your products, and a $15/month plan that allows full access.
https://stripe.com/docs/api#plans

Subscriptions
Subscriptions allow you to charge a customer on a recurring basis. A subscription ties a customer to a particular plan you’ve created.
https://stripe.com/docs/api#subscriptions

https://stripe.com/docs/billing/quickstart

https://stripe.com/docs/billing/testing

https://stripe.com/docs/webhooks

## Linking to Stripe Dashboard

Production:
https://dashboard.stripe.com/customers/cus_DP7oxTdvsJSrr9
Test:
https://dashboard.stripe.com/test/customers/cus_DME5qmiPcAeicU

## Stripe Webhooks 
https://www.nuget.org/packages/Microsoft.AspNetCore.WebHooks.Receivers.Stripe/
https://github.com/aspnet/WebHooks
https://github.com/aspnet/WebHooks/tree/master/samples/StripeCoreReceiver

https://stripe.com/docs/webhooks

Webhook endpoints can only be configured from the dashboard unfortunately.
UPDATE: now it is possible to create web hooks via the api 
https://stackoverflow.com/questions/41820731/create-stripe-webhooks-using-api/41824203?noredirect=1#comment93351643_41824203

Stripe can send webhook events that notify your application any time an event happens on your account. This is especially useful for events—like disputed charges and many recurring billing events—that are not triggered by a direct API request. This mechanism is also useful for services that are not directly responsible for making an API request, but still need to know the response from that request.

You can register webhook URLs that we will notify any time an event happens in your account. When the event occurs—a successful charge is made on a customer’s subscription, a transfer is paid, your account is updated, etc.—Stripe creates an Event object.

This Event object contains all the relevant information about what just happened, including the type of event and the data associated with that event. Stripe then sends the Event object, via an HTTP POST request, to any endpoint URLs that you have defined in your account’s Webhooks settings. You can have Stripe send a single event to many webhook endpoints.

You might also use webhooks as the basis to:

Update a customer's membership record in your database when a subscription payment succeeds
Email a customer when a subscription payment fails
Examine the Dashboard if you see that a dispute was filed
Make adjustments to an invoice when it's created (but before it's been paid)
Log an accounting entry when a transfer is paid

https://www.masteringmodernpayments.com/stripe-webhook-event-cheatsheet

https://github.com/stripe/stripe-webhook-monitor

An alternative to webhooks is to query events 
https://stripe.com/docs/api#retrieve_event

## Stripe Connect

https://stripe.com/docs/recipes/store-builder

At the end of the OAuth workflow, you’ll be provided with authorization credentials for the connected account:

{
  ...
  "stripe_publishable_key": "pk_live_h9xguYGf2GcfytemKs5tHrtg",
  "access_token": "sk_live_AxSI9q6ieYWjGIeRbURf6EG0",
  "stripe_user_id": "acct_Z3pHiNex3M2Fz2",
  ...
}
You’ll want to store the stripe_user_id, as this is used to identify the account when making API requests. The stripe_publishable_key will be necessary for requesting tokens


## Tax

https://www.taxjar.com/

https://www.taxjar.com/stripe-sales-tax/

