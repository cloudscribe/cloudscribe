
## REST

https://stackoverflow.com/questions/630453/put-vs-post-in-rest
Both PUT and POST can be used for creating.

You have to ask "what are you performing the action to?" to distinguish what you should be using. Let's assume you're designing an API for asking questions. If you want to use POST then you would do that to a list of questions. If you want to use PUT then you would do that to a particular question.

Great both can be used, so which one should I use in my RESTful design:

You do not need to support both PUT and POST.

Which is used is left up to you. But just remember to use the right one depending on what object you are referencing in the request.

Some considerations:

    Do you name your URL objects you create explicitly, or let the server decide? If you name them then use PUT. If you let the server decide then use POST.
    PUT is idempotent, so if you PUT an object twice, it has no effect. This is a nice property, so I would use PUT when possible.
    You can update or create a resource with PUT with the same object URL
    With POST you can have 2 requests coming in at the same time making modifications to a URL, and they may update different parts of the object.



## OpenApi 

https://www.openapis.org/
https://github.com/OAI/OpenAPI-Specification
https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md

https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-2.1

https://blogs.msdn.microsoft.com/webdev/2018/08/23/asp-net-core-2-20-preview1-open-api-analyzers-conventions/


https://github.com/RSuter/NSwag



http://buildingbettersoftware.blogspot.com/2018/04/implementing-hateos-functionality-in.html
https://github.com/DavidCBerry13/FoodTruckNationApi/

https://aspdotnetcodehelp.wordpress.com/2017/03/27/how-to-access-wcf-service-in-asp-net-core-application/

https://github.com/blog/2412-learn-graphql-with-github

https://services.github.com/on-demand/graphql/

https://codeopinion.com/building-self-descriptive-http-api-asp-net-core/
https://codeopinion.com/self-descriptive-http-api-in-asp-net-core-object-as-resource/
https://codeopinion.com/self-descriptive-http-api-in-asp-net-core-hypermedia/
https://codeopinion.com/self-descriptive-http-api-in-asp-net-core-hateoas/

https://github.com/dcomartin/Migrap.AspNetCore.Hateoas

http://hyperschema.org/mediatypes/siren

https://www.markus-lanthaler.com/hydra/

https://github.com/shieldfy/API-Security-Checklist



https://tomhofman.nl/lets-create-versioned-documented-asp-net-core-web-api/

https://andrewlock.net/model-binding-json-posts-in-asp-net-core/

https://www.smashingmagazine.com/2017/04/guide-http2-server-push/

http://www.dotnetcurry.com/aspnet-mvc/1149/convert-aspnet-webapi2-aspnet5-mvc6

https://goblincoding.com/2016/07/03/issuing-and-authenticating-jwt-tokens-in-asp-net-core-webapi-part-i/

https://goblincoding.com/2016/07/07/issuing-and-authenticating-jwt-tokens-in-asp-net-core-webapi-part-ii/

http://code.tutsplus.com/tutorials/securing-aspnet-web-api--cms-26012

SPA? https://medium.com/@wob/the-sad-state-of-web-development-1603a861d29f#.ks06316vl

http://gunnarpeipman.com/2016/04/why-azure-rest-api-s-and-how-to-prepare-for-using-them/

http://wildermuth.com/2016/03/16/Content_Negotiation_in_ASP_NET_Core

http://damienbod.com/2016/03/02/angular2-openid-connect-implicit-flow-with-identityserver4/
https://github.com/damienbod/AspNet5IdentityServerAngularImplicitFlow

Jam API is a service that allows you to turn any site into a JSON accessible api using CSS selectors.
http://www.jamapi.xyz/

https://royaljay.com/development/angular2-tutorial/

http://www.strathweb.com/2016/02/formatfilter-and-mediatypemappings-in-asp-net-core-1-0-mvc/

https://github.com/chsakell/aspnet5-angular2-typescript
http://chsakell.com/2015/12/31/cross-platform-single-page-applications-with-asp-net-5-angular-2-typescript/

http://stephenwalther.com/archive/2015/01/13/asp-net-5-and-angularjs-part-2-using-the-mvc-6-web-api

http://capesean.co.za/blog/asp-net-5-jwt-tokens/



http://damienbod.com/2015/12/13/asp-net-5-mvc-6-api-documentation-using-swagger/

http://www.dotnetcurry.com/aspnet/1223/secure-aspnet-web-api-using-tokens-owin-angularjs

http://stackoverflow.com/questions/29048122/token-based-authentication-in-asp-net-5-vnext
http://stackoverflow.com/questions/34612631/prevent-token-based-authorization-of-preflight-options-requests-in-asp-net-5-vn

https://github.com/mrsheepuk/ASPNETSelfCreatedTokenAuthExample

https://github.com/mrsheepuk/ASPNETMVC6AngularExample

https://github.com/johnpapa/angular-styleguide
http://mrsheepuk.github.io/ASPNETMVC6AngularExample/
http://mrsheepuk.github.io/ASPNETMVC6AngularExample/part01/
http://mrsheepuk.github.io/ASPNETMVC6AngularExample/part02/
http://mrsheepuk.github.io/ASPNETMVC6AngularExample/part03/
http://mrsheepuk.github.io/ASPNETMVC6AngularExample/part04/
http://mrsheepuk.github.io/ASPNETMVC6AngularExample/part05/
https://github.com/mrsheepuk/ASPNETMVC6AngularExample/tree/Part05Final

