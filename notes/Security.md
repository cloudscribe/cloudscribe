
https://www.troyhunt.com/passwords-evolved-authentication-guidance-for-the-modern-era/

http://jameschambers.com/2016/04/github-authentication-asp-net-core/
https://developer.github.com/v3/guides/getting-started/
https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/tree/dev/src/AspNet.Security.OAuth.GitHub

https://stackoverflow.com/questions/41242442/how-to-implement-windows-authentication-with-identityserver-4

https://paragonie.com/blog/2017/03/jwt-json-web-tokens-is-bad-standard-that-everyone-should-avoid

https://blogs.msdn.microsoft.com/webdev/2017/04/06/jwt-validation-and-authorization-in-asp-net-core/

https://blogs.msdn.microsoft.com/webdev/2017/04/27/jwt-validation-and-authorization-in-asp-net-core-2/

https://arstechnica.co.uk/security/2017/02/now-sites-can-fingerprint-you-online-even-when-you-use-multiple-browsers/

https://blog.benroux.me/running-multiple-https-domains-from-the-same-server/

http://odetocode.com/blogs/scott/archive/2017/02/06/anti-forgery-tokens-and-asp-net-core-apis.aspx

http://sakurity.com/blog/2016/12/10/serviceworker_botnet.html

https://docs.microsoft.com/en-us/aspnet/core/hosting/dataprotection
To automatically persist keys for an application hosted in IIS, you must create registry hives for each application pool. Use the Provisioning PowerShell script for each application pool you will be hosting ASP.NET Core applications under. This script will create a special registry key in the HKLM registry that is ACLed only to the worker process account. Keys are encrypted at rest using DPAPI.
https://github.com/aspnet/DataProtection/blob/dev/Provision-AutoGenKeys.ps1

https://blogs.msdn.microsoft.com/webdev/2016/10/27/bearer-token-authentication-in-asp-net-core/


http://en.xn--mgbz4cf.com/post/secure-your-aspnet-core-application-from-image-hotlinking

http://blog.securityps.com/2016/08/aspnet-core-basic-security-settings.html

https://www.smashingmagazine.com/2016/09/content-security-policy-your-future-best-friend/

http://andrewlock.net/exploring-the-cookieauthenticationmiddleware-in-asp-net-core/

http://andrewlock.net/a-look-behind-the-jwt-bearer-authentication-middleware-in-asp-net-core/

http://andrewlock.net/a-look-behind-the-jwt-bearer-authentication-middleware-in-asp-net-core/

http://en.هشام.com/post/secure-your-aspnet-core-application-from-image-hotlinking

https://dev.to/ben/the-targetblank-vulnerability-by-example

https://blog.mariusschulz.com/2016/07/19/securing-authentication-cookies-in-asp-net-core

http://stackoverflow.com/questions/35307143/simple-jwt-authentication-in-asp-net-core-1-0-web-api

## Authorization
https://blogs.msdn.microsoft.com/webdev/2016/03/15/get-started-with-asp-net-core-authorization-part-1-of-2/
https://blogs.msdn.microsoft.com/webdev/2016/03/23/get-started-with-asp-net-core-authorization-part-2-of-2/

http://benjamincollins.com/blog/practical-permission-based-authorization-in-asp-net-core/

https://goblincoding.com/2016/07/03/issuing-and-authenticating-jwt-tokens-in-asp-net-core-webapi-part-i/
https://goblincoding.com/2016/07/07/issuing-and-authenticating-jwt-tokens-in-asp-net-core-webapi-part-ii/

http://code.tutsplus.com/tutorials/securing-aspnet-web-api--cms-26012

https://www.troyhunt.com/everything-you-need-to-know-about-loading-a-free-lets-encrypt-certificate-into-an-azure-website/

https://float-middle.com/json-web-tokens-jwt-vs-sessions/

https://stormpath.com/blog/token-authentication-asp-net-core


ssl example
https://github.com/aspnet/Security/blob/dev/samples/SocialSample/Program.cs


http://moneyandstate.com/looting-of-the-fox/

https://medium.com/@jitbit/target-blank-the-most-underestimated-vulnerability-ever-96e328301f4c#.disn238f9

http://jameschambers.com/2016/04/github-authentication-asp-net-core/

https://github.com/Lone-Coder/letsencrypt-win-simple

Fully automated dockerized Let's Encrypt reverse proxy
https://advancedweb.hu/2016/05/10/lets-encrypt/

The OWASP Zed Attack Proxy (ZAP) is one of the world�s most popular free security tools and is actively maintained by hundreds of international volunteers*. It can help you automatically find security vulnerabilities in your web applications while you are developing and testing your applications. Its also a great tool for experienced pentesters to use for manual security testing.
https://www.owasp.org/index.php/OWASP_Zed_Attack_Proxy_Project

https://blog.binarymist.net/


https://technet.microsoft.com/en-us/library/bb727085.aspx


How you can get a free SSL certificate in 4min - letsencrypt
https://blog.sverrirs.com/2016/03/windows-app-to-automate-ssl-cert.html

https://pierrekim.github.io/blog/2016-02-16-why-i-stopped-using-startssl-because-of-qihoo-360.html

xss tutorial
http://excess-xss.com/

http://www.troyhunt.com/2016/03/understanding-csrf-video-tutorial.html


A goal of cloudscribe core should be to follow the OWASP security guidelines

Automated Security Analyzer for ASP.NET Websites by Troy  Hunt
https://asafaweb.com

https://www.owasp.org/index.php/Authentication_Cheat_Sheet

## Email Verification
https://www.owasp.org/index.php/Input_Validation_Cheat_Sheet#Email_Address_Validation

* email addresses should be considered to be public data
* Check for presence of at least one @ symbol in the address 
* Ensure the local-part is no longer than 64 octets (local part is to the left of rightmost @)
* Ensure the domain is no longer than 255 octets 
* Ensure the address is deliverable 

To ensure an address is deliverable, the only way to check this is to send the user an email and have the user take action to confirm receipt. Beyond confirming that the email address is valid and deliverable, this also provides a positive acknowledgement that the user has access to the mailbox and is likely to be authorized to use it. This does not mean that other users cannot access this mailbox, for example when the user makes use of a service that generates a throw away email address.

*    Email verification links should only satisfy the requirement of verify email address ownership and should not provide the user with an authenticated session (e.g. the user must still authenticate as normal to access the application).
*   Email verification codes must expire after the first use or expire after 8 hours if not used.
 
As the local-part of email addresses are, in fact - case sensitive, it is important to store and compare email addresses correctly. To normalise an email address input, you would convert the domain part ONLY to lowercase.

Unfortunately this does and will make input harder to normalise and correctly match to a users intent. It is reasonable to only accept one unique capitalisation of an otherwise identical address, however in this case it is critical to:

*  Store the user-part as provided and verified by user verification
*  Perform comparisons by lowercase(provided)==lowercase(persisted)


## Security Questions

https://www.owasp.org/index.php/Choosing_and_Using_Security_Questions_Cheat_Sheet

http://stackoverflow.com/questions/28332344/mvc-using-asp-net-identity-individual-accounts-how-to-add-security-questions

http://brockallen.com/2014/02/11/concerns-with-two-factor-authentication-in-asp-net-identity-v2/
"an identity management system should support password reset secret questions and answers"

"there really are NO GOOD security questions; only fair or bad questions"

Any security questions or identity information presented to users to reset forgotten passwords should ideally have the following four characteristics:

* Memorable: If users can't remember their answers to their security questions, you have achieved nothing.
* Consistent: The user's answers should not change over time. For instance, asking "What is the name of your significant other?" may have a different answer 5 years from now.
* Nearly universal: The security questions should apply to a wide an audience of possible.
* Safe: The answers to security questions should not be something that is easily guessed, or research (e.g., something that is matter of public record).

For enhanced security, you may wish to consider asking the user for their email address first and then send an email that takes them to a private page that requests the other 2 (or more) identity factors. That way the email itself isn�t that useful because they still have to answer a bunch of �secret� questions after they get to the landing page.

On the other hand, if you host a web site that targets the general public, such as social networking sites, free email sites, news sites, photo sharing sites, etc., then you likely to not have this identity information and will need to use some sort of the ubiquitous "security questions". However, also be sure that you collect some means to send the password reset information to some out-of-band side-channel, such as a (different) email address, an SMS texting number, etc.

Believe it or not, there is a certain merit to allow your users to select from a set of several "canned" questions. We generally ask users to fill out the security questions as part of completing their initial user profile and often that is the very time that the user is in a hurry; they just wish to register and get about using your site. If we ask users to create their own question(s) instead, they then generally do so under some amount of duress, and thus may be more likely to come up with extremely poor questions. 

However, there is also some strong rationale to requiring users to create their own question(s), or at least one such question. The prevailing legal opinion seems to be if we provide some sort of reasonable guidance to users in creating their own questions and then insist on them doing so, at least some of the potential liabilities are transferred from our organizations to the users. In such cases, if user accounts get hacked because of their weak security questions (e.g., "What is my favorite ice cream flavor?", etc.) then the thought is that they only have themselves to blame and thus our organizations are less likely to get sued.

Since OWASP recommends in the Forgot Password Cheat Sheet that multiple security questions should be posed to the user and successfully answered before allowing a password reset, a good practice might be to require the user to select 1 or 2 questions from a set of canned questions as well as to create (a different) one of their own and then require they answer one of their selected canned questions as well as their own question. 

While most developers would generally first review any potential questions with whatever relevant business unit, it may not occur to them to review the questions with their legal department or chief privacy officer. However, this is advisable because there may be applicable laws or regulatory / compliance issues to which the questions must adhere. For example, in the telecommunications industry, the FCC's Customer Proprietary Network Information (CPNI) regulations prohibit asking customers security questions that involve "personal information", so questions such as "In what city were you born?" are generally not allowed. 

Step 3) Insist on a Minimal Length for the Answers

Even if you pose decent security questions, because users generally dislike putting a whole lot of forethought into answering the questions, they often will just answer with something short. Answering with a short expletive is not uncommon, nor is answering with something like "xxx" or "1234". If you tell the user that they should answer with a phrase or sentence and tell them that there is some minimal length to an acceptable answer (say 10 or 12 characters), you generally will get answers that are somewhat more resistant to guessing. 

Step 4) Consider How To Securely Store the Questions and Answers

There are two aspects to this...storing the questions and storing the answers. Obviously, the questions must be presented to the user, so the options there are store them as plaintext or as reversible ciphertext. The answers technically do not need to be ever viewed by any human so they could be stored using a secure cryptographic hash (although in principle, I am aware of some help desks that utilize the both the questions and answers for password reset and they insist on being able to read the answers rather than having to type them in; YMMV). Either way, we would always recommend at least encrypting the answers rather than storing them as plaintext. This is especially true for answers to the "create your own question" type as users will sometimes pose a question that potentially has a sensitive answer (e.g., "What is my bank account # that I share with my wife?"). 

In addition, if you explain to your customers that you are encrypting their questions and hashing their answers, they might feel safer about asking some questions that while potentially embarrassing, might be a bit more secure. (Use your imagination. Do we need to spell it out for you? Really???) 

Step 5) Periodically Have Your Users Review their Questions

Many companies often ask their users to update their user profiles to make sure contact information such as email addresses, street address, etc. is still up-to-date. Use that opportunity to have your users review their security questions. (Hopefully, at that time, they will be in a bit less of a rush, and may use the opportunity to select better questions.) If you had chosen to encrypt rather than hash their answers, you can also display their corresponding security answers at that time.

If you keep statistics on how many times the respective questions has been posed to someone as part of a Forgot Password flow (recommended), it would be advisable to also display that information. (For instance, if against your advice, they created a question such as "What is my favorite hobby?" and see that it had been presented 113 times and they think they might have only reset their password 5 times, it would probably be advisable to change that security question and probably their password as well.)
Step 6) Authenticate Requests to Change Questions

Many web sites properly authenticate change password requests simply by requesting the current password along with the desired new password. If the user cannot provide the correct current password, the request to change the password is ignored. The same authentication control should be in place when changing security questions. The user should be required to provide the correct password along with their new security questions & answers. If the user cannot provide the correct password, then the request to change the security questions should be ignored. This control prevents both Cross-Site Request Forgery attacks, as well as changes made by attackers who have taken control over a users workstation or authenticated application session. 

Using Security Questions

Requiring users to answer security questions is most frequently done under two quite different scenarios:

*  As a means for users to reset forgotten passwords. (See Forgot Password Cheat Sheet.)
*  As an additional means of corroborating evidence used for authentication.

If at anytime you intend for your users to answer security questions for both of these scenarios, it is strongly recommended that you use two different sets of questions / answers


    briefly describe the importance of selecting a good security question / answer.
    provide some guidance, along with some examples, of what constitutes bad vs. fair security questions.

You may wish to refer your users to the [Good Security Questions] web site for the latter. 
goodsecurityquestions.com

Lastly, you should consider whether or not you should treat the security questions that a user will type in as a "password" type or simply as regular "text" input. The former can prevent shoulder-surfing attacks, but also cause more typos, so there is a trade-off. Perhaps the best advice is to give the user a choice; hide the text by treating it as "password" input type by default, but all the user to check a box that would display their security answers as clear text when checked. 


# Password Reset 

http://ux.stackexchange.com/questions/72259/should-passwords-expire
http://security.stackexchange.com/questions/4704/how-does-changing-your-password-every-90-days-increase-security
http://security.stackexchange.com/questions/19458/requiring-regular-password-change-but-storing-previous-paswords

The first page of a secure Forgot Password feature asks the user for multiple pieces of hard data that should have been previously collected (generally when the user first registers). Steps for this are detailed in the identity section the Choosing and Using Security Questions Cheat Sheet here.

At a minimum, you should have collected some data that will allow you to send the password reset information to some out-of-band side-channel, such as a (possibly different) email address or an SMS text number, etc. to be used in Step 3. 
https://www.owasp.org/index.php/Forgot_Password_Cheat_Sheet

Step 3) Send a Token Over a Side-Channel

After step 2, lock out the user's account immediately. Then SMS or utilize some other multi-factor token challenge with a randomly-generated code having 8 or more characters. This introduces an �out-of-band� communication channel and adds defense-in-depth as it is another barrier for a hacker to overcome. If the bad guy has somehow managed to successfully get past steps 1 and 2, he is unlikely to have compromised the side-channel. It is also a good idea to have the random code which your system generates to only have a limited validity period, say no more than 20 minutes or so. That way if the user doesn't get around to checking their email and their email account is later compromised, the random token used to reset the password would no longer be valid if the user never reset their password and the "reset password" token was discovered by an attacker. Of course, by all means, once a user's password has been reset, the randomly-generated token should no longer be valid. 

Step 4) Allow user to change password in the existing session

Step 4 requires input of the code sent in step 3 in the existing session where the challenge questions were answered in step 2, and allows the user to reset his password. Display a simple HTML form with one input field for the code, one for the new password, and one to confirm the new password. Verify the correct code is provided and be sure to enforce all password complexity requirements that exist in other areas of the application. As before, avoid sending the username as a parameter when the form is submitted. Finally, it's critical to have a check to prevent a user from accessing this last step without first completing steps 1 and 2 correctly. Otherwise, a forced browsing attack may be possible. 

Step 5) Logging

It is important to keep audit records when password change requests were submitted. This includes whether or not security questions were answered, when reset messages were sent to users and when users utilize them. It is especially important to log failed attempts to answer security questions and failed attempted use of expired tokens. This data can be used to detect abuse and malicious behavior. Data such as time, IP address, and browser information can be used to spot trends of suspicious use.
Other Considerations

    Whenever a successful password reset occurs, the session should be invalidated and the user redirected to the login page.
    Strength of questions used for reset should vary based on the nature of the credential. Administrator credentials should have a higher requirement.
    The ideal implementation should rotate the questions asked in order to avoid automation.

	
Anti-Automation
haveibeenpwned.com

captcha

asp.net data protection stack
https://vimeo.com/153102690

http://lockmedown.com/preventing-sensitive-data-exposure-aspnet-part1/
http://lockmedown.com/preventing-sensitive-data-exposure-aspnet-part2/

https://letsencrypt.org/

https://docs.asp.net/en/latest/security/authorization/resourcebased.html

http://leastprivilege.com/2015/10/12/the-state-of-security-in-asp-net-5-and-mvc-6-authorization/

http://docs.asp.net/projects/mvc/en/latest/security/cors-policy.html

https://github.com/aspnet/Security/issues/35

http://stackoverflow.com/questions/31464359/custom-authorizeattribute-in-asp-net-5-mvc-6

https://github.com/aspnet/MusicStore/blob/master/src/MusicStore/Startup.cs#L118-L121

http://www.troyhunt.com/2012/05/everything-you-ever-wanted-to-know.html

http://blogs.msdn.com/b/webdev/archive/2014/01/06/implementing-custom-password-policy-using-asp-net-identity.aspx

http://stackoverflow.com/questions/31476361/sharing-authentication-cookie-in-asp-net-5-across-subdomains

http://stackoverflow.com/questions/30768015/configure-the-authorization-server-endpoint


http://www.asp.net/aspnet/overview/web-development-best-practices/what-not-to-do-in-aspnet-and-what-to-do-instead#validation

using openidconnect in cloudscribe against identity server-endpoint
in the op server setup the client
Allow access tokens via the browser 

add redirect url:
https://localhost:44399/signin-oidc 

https://localhost:44399/two/signin-oidc
add logout redirect url
https://localhost:44399/signout-callback-oidc 

https://localhost:44399/two/signout-callback-oidc
add cors origin:
https://localhost:44399 
add client secret, only value is needed
allowed grant types use hybrid
add allowed scopes
openid and profile which also must be created as identoty resources

optional add scopes for protected resource apis if applicable