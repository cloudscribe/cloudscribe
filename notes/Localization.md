

http://www.vodovnik.com/2015/09/20/localization-in-asp-net-5-mvc-6/

http://www.lhotka.net/weblog/NETCoreUsingExistingResxResourceFile.aspx

http://haacked.com/archive/2011/07/14/model-metadata-and-validation-localization-using-conventions.aspx/

this is a replacement for the resx system, worth considering
https://github.com/turquoiseowl/i18n


http://www.hanselman.com/blog/GlobalizationInternationalizationAndLocalizationInASPNETMVC3JavaScriptAndJQueryPart1.aspx

http://brianreiter.org/2011/03/23/simple-asp-net-mvc-globalization-with-graceful-fallback/

http://www.chambaud.com/2013/02/27/localization-in-asp-net-mvc-4/

there is a problem in unit testing things that use resx files because that is part of httpcontext and asp.net compilation process
http://www.codeproject.com/Tips/790565/Unit-testing-MVC-controllers-which-are-using-globa

login or sign in?
http://www.designcult.org/2011/08/why-do-we-call-in-logging-in.html

http://resxresourcemanager.codeplex.com/

Localization of currency is straightforward if a site uses a single specific currency that is connected to the single specific CultureInfo

It is challenging if you want to support additional currencies
http://stackoverflow.com/questions/784794/best-practice-format-multiple-currencies
http://stackoverflow.com/questions/850673/proper-currency-format-when-not-displaying-the-native-currency-of-a-culture

In mojoportal I was doing something that worked but was kind of funky, I would look up the first CultureInfo I could find that supported the currency.
I think in cloudscribe related work I want to store a specific culture code along with my currency code, and always use the specifci culture to format numbers as currency

how to get a list of cultures
http://stackoverflow.com/questions/5956898/how-to-make-a-dropdown-list-of-all-cultures-but-no-repeats
http://www.csharp-examples.net/culture-names/


