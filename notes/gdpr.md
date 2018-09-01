
https://www.gdpreu.org/the-regulation/key-concepts/consent/

Hanselminutes
https://media.simplecast.com/episodes/audio/180697/hm647.mp3

Instead of a small text banner on the side that can easily be ignored or overlooked, implement a gated popup upon site visit that highlights the permissions that you seek, with a link to a detailed page for users to learn more. Before the user is able to interact with the site content, he or she would be required to take one of three possible actions on this gated popup:

Click “Accept”. This dismisses the popup, and constitutes consent for the controller.
Click “Visit settings to decline”. This dismisses the popup, and opens up a new window to a technical settings page for data subjects to decline or revoke previously granted consent.
Click “x” to dismiss the popup. This lets the user to access the web content, but does not in itself grant the controller consent. The website must wait for the user to click accept on a future site visit before processing his or her personal data.

3. The data subject shall have the right to withdraw his or her consent at any time. The withdrawal of consent shall not affect the lawfulness of processing based on consent before its withdrawal. Prior to giving consent, the data subject shall be informed thereof. It shall be as easy to withdraw as to give consent.


https://kruschecompany.com/blog/post/gdpr-smart-practices

https://docs.microsoft.com/en-us/aspnet/core/security/gdpr?view=aspnetcore-2.1

https://blogs.msdn.microsoft.com/webdev/2018/03/04/asp-net-core-2-1-0-preview1-gdpr-enhancements/

https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/transparent-data-encryption?view=sql-server-2017

Add, download, and delete custom user data to Identity in an ASP.NET Core project
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/add-user-data?view=aspnetcore-2.1&tabs=visual-studio

https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/transparent-data-encryption?view=sql-server-2017

https://www.usatoday.com/story/tech/columnist/saltzman/2018/04/04/amazon-and-alexa-know-whole-lot-you-heres-how-download-and-delete-info/482286002/



https://github.com/blowdart/AspNetCoreIdentityEncryption
To have Identity encrypt your custom IdentityUser model, annotate your model fields with [ProtectedPersonalData].

https://www.tabsoverspaces.com/233700-custom-encryption-of-field-with-entity-framework-core

https://www.tabsoverspaces.com/233708-using-value-converter-for-custom-encryption-of-field-on-entity-framework-core-2-1

https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/security-considerations

## Personal Data

https://eugdprcompliant.com/personal-data/



Properties decorated with the PersonalData attribute are:

    Deleted when the Areas/Identity/Pages/Account/Manage/DeletePersonalData.cshtml Razor Page calls UserManager.Delete.
    Included in the downloaded data by the Areas/Identity/Pages/Account/Manage/DownloadPersonalData.cshtml Razor Page.

https://github.com/aspnet/Identity/blob/master/src/Stores/IdentityUser.cs
[ProtectedPersonalData]
public virtual string Email { get; set; }


## Third Party Cookies

If there is no consent for 1st party cookie we should not let 3rd party cookies in.
Google Analytics
Recaptcha
AddThis
Gravatar
Disqus

https://law.stackexchange.com/questions/27908/gdpr-recaptcha-with-users-consent
https://www.google.com/about/company/user-consent-policy.html

https://brianclifton.com/blog/2018/04/16/google-analytics-gdpr-and-consent/
Summary of Google’s Advice:
If you use these Advertising features in GA, you must request explicit consent. If you do not, then you don’t.

any other embedded content, youtube etc

https://www.mcbeev.com/Blog/May-2018/Analytics-Scripts-in-GDPR-With-Kentico-EMS
