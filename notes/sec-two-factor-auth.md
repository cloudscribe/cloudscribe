## Two Factor Auth

A lot changed in TFA for asp.net core 2.0
In 1.x SMS was used to send a code to the user phone for 2 factor auth.
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/2fa

but in 2.0 now we are not supposed to use SMS, but instead use an authenticator app
There is a Microsoft Authenticator app and a Google Authenticator app that can be installed on phones
Then a QR code can be generated on the web page and scanned with the phone app.
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-enable-qrcodes


Download a two-factor authenticator app like Microsoft Authenticator for
<a href="https://go.microsoft.com/fwlink/?Linkid=825072">Android</a> and
<a href="https://go.microsoft.com/fwlink/?Linkid=825073">iOS</a> or
Google Authenticator for
<a href="https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2&amp;hl=en">Android</a> and
<a href="https://itunes.apple.com/us/app/google-authenticator/id388497605?mt=8">iOS</a>.

After enabling 2 factor with the authenticator app, a screen shows some codes and says keep them safe, these are from an example app:

bdc735d0 560694b3
37c95595 ceab5911
83ffde4c 97a3bfd8
afb9f284 8ecfca43
2a2226ac 85521868

Q: how are people supposed to do that? Seems cumbersome for non-techy users

After refreshing the page it shows buttons for Disable2FA, Reset Recovery Codes, Configure Authenticator App, and Reset Authenticator Key

When you log out then login again you are prompted for authenticator code and a checkbox for remember this machine. 
You just look in the authenticator app for the current code.

### Background information on Why SMS is no longer recommended

https://www.wired.com/2016/06/hey-stop-using-texts-two-factor-authentication/
https://www.theverge.com/2017/7/10/15946642/two-factor-authentication-online-security-mess
https://nakedsecurity.sophos.com/2016/08/12/sms-or-authenticator-app-which-is-better-for-two-factor-authentication/

