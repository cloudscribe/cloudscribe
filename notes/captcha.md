
I implemented a controller extension method for validating the captcha server side.

var captchaResponse = await this.ValidateRecaptcha(Request, recpatchaSecretKey);

you can see working examples in login and registration views and AccountController

# References

http://www.codeproject.com/Articles/874150/Google-reCAPTCHA-in-ASP-NET-MVC
https://github.com/venkatbaggu/reCAPTCHA

http://recaptcha-net.mtd226.com/
https://github.com/tanveery/recaptcha-net
https://www.nuget.org/packages/RecaptchaNet/