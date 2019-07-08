
# General

https://www.smashingmagazine.com/2018/11/guide-pwa-progressive-web-applications/

https://kruschecompany.com/blog/post/the-web-apps-that-will-eat-mobile


http://www.pwabuilder.com/

Progressive web apps and the Windows ecosystem
https://channel9.msdn.com/Events/Build/2017/B8075


https://developers.googleblog.com/2017/05/the-modern-mobile-web-state-of-union.html

Progressive Web Apps: Great Experiences Everywhere (Google I/O '17)
https://www.youtube.com/watch?v=m-sCdS0sQO8


https://alistapart.com/article/yes-that-web-project-should-be-a-pwa

https://joreteg.com/blog/betting-on-the-web

https://medium.com/samsung-internet-dev/6-myths-of-progressive-web-apps-81e28ca9d2b1


# Installation - aka Add to home screen (A2HS)


https://developers.google.com/web/progressive-web-apps/desktop

https://developers.google.com/web/fundamentals/app-install-banners/#criteria


https://dockyard.com/blog/2017/09/27/encouraging-pwa-installation-on-ios

https://developers.google.com/web/fundamentals/app-install-banners/

https://developers.google.com/web/fundamentals/app-install-banners/promoting-install-mobile

not working on FF desktop
https://bugzilla.mozilla.org/show_bug.cgi?id=1407202

https://github.com/docluv/add-to-homescreen



# ServiceWorker
https://redfin.engineering/service-workers-break-the-browsers-refresh-button-by-default-here-s-why-56f9417694

https://redfin.engineering/how-to-fix-the-refresh-button-when-using-service-workers-a8e27af6df68


https://github.com/googlecodelabs/workbox-lab/

https://blog.johnnyreilly.com/2017/11/the-typescript-webpack-pwa.html

https://developers.google.com/web/tools/workbox/

https://developers.google.com/web/tools/workbox/modules/workbox-window

https://developers.google.com/web/tools/workbox/modules/workbox-google-analytics


https://blog.elmah.io/turning-an-aspnet-core-website-into-a-progressive-web-app-pwa/
https://www.nuget.org/packages/WebEssentials.AspNetCore.PWA/
https://github.com/madskristensen/WebEssentials.AspNetCore.ServiceWorker

https://madskristensen.net/blog/offline-aspnet-core-apps-with-service-workers/


## dynamic pre-cache

https://stackoverflow.com/questions/51651002/service-worker-add-files-from-api-call-to-precache

Service worker updates are usually triggered when you call navigator.serviceWorker.register() and the browser detects that the service worker file has changed. That means if you change what /Assets returns but the contents of your service worker files haven't change, you're service worker won't update. This is why most people hard-code their precache list in their service worker (since any changes to those files will trigger a new service worker installation).

https://github.com/NascHQ/dsw

Since serviceworker is only updated if the sw file changes why not make the sw file itself dynamic, ie served and generated from a controller.

The service worker is immediately downloaded when a user first accesses a service worker–controlled site/page.
After that, it is downloaded every 24 hours or so. It may be downloaded more frequently, but it must be downloaded every 24 hours to prevent bad scripts from being annoying for too long.

## Broadcast channel

https://developers.google.com/web/updates/2016/09/broadcastchannel

The Broadcast Channel API is a simple API that makes communicating between browsing contexts easier. That is, communicating between windows/tabs, iframes, web workers, and service workers. Messages which are posted to a given channel are delivered to all listeners of that channel.


## Push Notification

https://developers.google.com/web/ilt/pwa/introduction-to-push-notifications

https://developers.google.com/web/fundamentals/codelabs/push-notifications/

https://developers.google.com/web/fundamentals/push-notifications/

The Notifications API lets the app display system notifications to the user. 
The Push API allows a service worker to handle Push Messages from a server, even while the app is not active.

https://developer.mozilla.org/en-US/docs/Web/API/Notifications_API

https://developer.mozilla.org/en-US/docs/Web/API/Push_API

https://web-push-book.gauntface.com/demos/notification-examples/

https://medium.com/commencis/web-push-notifications-with-service-workers-cf6ec8005a6c

https://developers.google.com/web/ilt/pwa/live-data-in-the-service-worker

https://itnext.io/indexeddb-your-second-step-towards-progressive-web-apps-pwa-dcbcd6cc2076


It's important to understand that there is a third party in web push protocol flow: push service. Push service acts as intermediary which ensures reliable and efficient delivery of push messages to the client.

Each browser manages push notifications through their own system, called a "push service". 

Chrome currently uses Firebase Cloud Messaging (FCM) as its push service.
https://firebase.google.com/docs/cloud-messaging/

For Chrome to route FCM messages to the correct service worker, it needs to know the Sender ID. Supply this by adding a gcm_sender_id property to your app's manifest.json file. For example, the manifest could look like this:

{
  "name": "Push Notifications app",
  "gcm_sender_id": "370072803732"
}

When we receive a push notification with a payload, the data is available directly on the event object. This data can be of any type, and you can access the data as a JSON result, a BLOB, a typed array, or raw text.

### VAPID

What is important now is that VAPID requires Application Server Keys (public and private key pair). The easiest way to generate those keys is to use an online generator. The public key has to be delivered to the client. In this post I will put it directly into snippets but in real life I would suggest delivering it on demand (the demo application is doing exactly that), preferably over HTTPS.
https://web-push-libs.github.io/vapid/js/

Using VAPID also lets you avoid the FCM-specific steps for sending a push message. You no longer need a Firebase project, a gcm_sender_id, or an Authorization header.
https://tools.ietf.org/html/draft-ietf-webpush-vapid-01
https://github.com/tpeczek/Demo.AspNetCore.PushNotifications

Using VAPID
The process is pretty simple:

Your application server creates a public/private key pair. The public key is given to your web app.
When the user elects to receive pushes, add the public key to the subscribe() call's options object.
When your app server sends a push message, include a signed JSON web token along with the public key.

To subscribe a Chrome user for push with the VAPID public key, pass the public key as a Uint8Array using the applicationServerKey parameter of the subscribe() method.

const publicKey = new Uint8Array([0x4, 0x37, 0x77, 0xfe, .... ]);
serviceWorkerRegistration.pushManager.subscribe(
  {
    userVisibleOnly: true,
    applicationServerKey: publicKey
  }
);
You'll know if it has worked by examining the endpoint in the resulting subscription object; if the origin is fcm.googleapis.com, it's working.

Note: Even though this is an FCM URL, use the Web Push Protocol not the FCM protocol, this way your server-side code will work for any push service.

To send a message using VAPID, you make a normal Web Push Protocol request with two additional HTTP headers: an Authorization header and a Crypto-Key header. Let's look at these new headers in detail.

Note: This is where web push libraries really shine, as the process of signing and sending a message can be quite complex. We include an example of sending a message with VAPID using the web-push library for Node.js at the end of this section.

### Push Best Practices

While it's relatively simple to get notifications up and running, making an experience that users really value is trickier. There are also many edge cases to consider when building an experience that works well.

notifications should be timely, precise, and relevant

If you ask the user for permission to send push notifications when they first land on your site, they might dismiss it. Once they have denied permission, they can't be asked again. Case studies show that when a user has context when the prompt is shown, they are more likely to grant permission.

The following interaction patterns are good times to ask for permission to show notifications:

When the user is configuring their communication settings, you can offer push notifications as one of the options.
After the user completes a critical action that needs to deliver timely and relevant updates to the user. For example, if the user purchased an item from your site, you can offer to notify the user of delivery updates.
When the user returns to your site they are likely to be a satisfied user and more understanding of the value of your service.

Another pattern that works well is to offer a very subtle promotion area on the screen that asks the user if they would like to enable notifications. Be careful not to distract too much from your site's main content. Clearly explain the benefits of what notifications offers the user.

group messages that are contextually relevant into one notification. For example, if you are building a social app, group notifications by sender and show one per person. If you have an auction site, group notifications by the item being bid on.

The notification object includes a tag attribute that is the grouping key. When creating a notification with a tag and there is already a notification with the same tag visible to the user, the system automatically replaces it without creating a new notification. 

Managing Notifications at the Server
So far, we've been assuming the user is around to see our notifications. But consider the following scenario:

The user's mobile device is offline
Your site sends user's mobile device a message for something time sensitive, such as breaking news or a calendar reminder
The user turns the mobile device on a day later. It now receives the push message.
That scenario is a poor experience for the user. The notification is neither timely or relevant. Our site shouldn't display the notification because it's out of date.

You can use the time_to_live (TTL) parameter, supported in both HTTP and XMPP requests, to specify the maximum lifespan of a message. The value of this parameter must be a duration from 0 to 2,419,200 seconds, corresponding to the maximum period of time for which FCM stores and tries to deliver the message. Requests that don't contain this field default to the maximum period of 4 weeks. If the message is not sent within the TTL, it is not delivered.

Another advantage of specifying the lifespan of a message is that FCM never throttles messages with a time_to_live value of 0 seconds. In other words, FCM guarantees best effort for messages that must be delivered "now or never". Keep in mind that a time_to_live value of 0 means messages that can't be delivered immediately are discarded. However, because such messages are never stored, this provides the best latency for sending notifications.


### ASP.NET Core push

https://github.com/web-push-libs/web-push-csharp

https://github.com/coryjthompson/WebPushDemo

https://www.tpeczek.com/2017/12/push-notifications-and-aspnet-core-part.html

https://www.tpeczek.com/2018/01/push-notifications-and-aspnet-core-part.html

https://www.tpeczek.com/2018/01/push-notifications-and-aspnet-core-part_18.html

https://www.tpeczek.com/2018/03/push-notifications-and-aspnet-core-part.html

https://www.tpeczek.com/2019/02/push-notifications-and-aspnet-core-part.html

https://github.com/tpeczek/Demo.AspNetCore.PushNotifications

https://github.com/tpeczek/Lib.Net.Http.WebPush


## Background sync

https://developers.google.com/web/updates/2015/12/background-sync

Background sync is a new web API that lets you defer actions until the user has stable connectivity. This is useful for ensuring that whatever the user wants to send, is actually sent.
