a good introduction to async await, at about 25 minutes it is also about using the codeitright product for code quality analysis
but shows the correct way to use async await and common wrong way mistakes to avoid
https://www.youtube.com/watch?v=k81OaT_P6K0#t=60

http://stackoverflow.com/questions/13489065/best-practice-to-call-configureawait-for-all-server-side-code


2015-06-08 now using the asp.net 5 bits and identity bits shipped with VS 2015 RC1
I noticed all the async methods on UserStore<TUser> now have CancellationToken in the method signatures.
such methods are supposed to check the token periodically to see if cancellation has been requested
https://johnbadams.wordpress.com/2012/03/10/understanding-cancellationtokensource-with-tasks/

http://www.davepaquette.com/archive/2015/07/19/cancelling-long-running-queries-in-asp-net-mvc-and-web-api.aspx

you can see how it is used in EF Identity implementation
https://github.com/aspnet/Identity/blob/dev/src/Microsoft.AspNet.Identity.EntityFramework/UserStore.cs

https://johnbadams.wordpress.com/2012/03/13/understanding-cancellationtokensource-with-tasks-part-2/
http://stackoverflow.com/questions/10134310/how-to-cancel-a-task-in-await
https://msdn.microsoft.com/en-us/library/dd997364.aspx


best practices in async
http://msdn.microsoft.com/en-us/magazine/jj991977.aspx

http://blog.stephencleary.com/2012/02/async-and-await.html
A good rule of thumb is to use ConfigureAwait(false) unless you know you do need the context.

Good document Task Based Async Pattern
https://www.microsoft.com/en-us/download/details.aspx?id=19957 (docx)


http://www.asp.net/web-forms/overview/performance-and-caching/using-asynchronous-methods-in-aspnet-45

http://stackoverflow.com/questions/22628087/calling-async-method-synchronously
http://blog.stephencleary.com/2012/07/dont-block-on-async-code.html

http://stackoverflow.com/questions/31390655/async-await-am-i-over-doing-it

public async Task<List<string>> GetMeesagesAsync()
{
   if(messageCache != null)
     return messageCache;
   return await _referralMessageData.GetMessagesAsync().ConfigureAwait(false);
}
Becomes:

public Task<List<string>> GetMeesagesAsync()
{
   if(messageCache != null)
     return Task.FromResult(messageCache);
   return _referralMessageData.GetMessagesAsync();
}
However, if at any point you need the results of a task to do further work, then awaiting is the way to go.

The sample repository and data code don't have much real logic in them (and none after the await), so they can be simplified to return the tasks directly, as other commenters have noted.

On a side note, the sample repository suffers from a common repository problem: doing nothing. If the rest of your real-world repository is similar, you might have one level of abstraction too many in your system. Note that Entity Framework is already a generic unit-of-work repository.

But regarding async and await in the general case, the code often has work to do after the await:

public async Task<IHttpActionResult> GetMessages()
{
  var result = await _messageRepository.GetMessagesAsync();
  return Ok(result);
}
Remember that async and await are just fancy syntax for hooking up callbacks. There isn't an easier way to express this method's logic asynchronously. There have been some experiments around, e.g., inferring await, but they have all been discarded at this point (I have a blog post describing why the async/await keywords have all the "cruft" that they do).

And this cruft is necessary for each method. Each method using async/await is establishing its own callback. If the callback isn't necessary, then the method can just return the task directly, avoiding async/await. Other asynchronous systems (e.g., promises in JavaScript) have the same restriction: they have to be asynchronous all the way.

It's possible - conceptually - to define a system in which any blocking operation would yield the thread automatically. My foremost argument against a system like this is that it would have implicit reentrancy. Particularly when considering third-party library changes, an auto-yielding system would be unmaintainable IMO. It's far better to have the asynchrony of an API explicit in its signature (i.e., if it returns Task, then it's asynchronous).

Now, @usr makes a good point that maybe you don't need asynchrony at all. That's almost certainly true if, e.g., your Entity Framework code is querying a single instance of SQL Server. This is because the primary benefit of async on ASP.NET is scalability, and if you don't need scalability (of the ASP.NET portion), then you don't need asynchrony. See the "not a silver bullet" section in my MSDN article on async ASP.NET.

However, I think there's also an argument to be made for "natural APIs". If an operation is naturally asynchronous (e.g., I/O-based), then its most natural API is an asynchronous API. Conversely, naturally synchronous operations (e.g., CPU-based) are most naturally represented as synchronous APIs. The natural API argument is strongest for libraries - if your repository / data access layer was its own dll intended to be reused in other (possibly desktop or mobile) applications, then it should definitely be an asynchronous API. But if (as is more likely the case) it is specific to this ASP.NET application which does not need to scale, then there's no specific need to make the API either asynchronous or synchronous.

But there's a good two-pronged counter-argument regarding developer experience. Many developers don't know their way around async at all; would a code maintainer be likely to mess it up? The other prong of that argument is that the libraries and tooling around async are still coming up to speed. Most notable is the lack of a causality stack when there are exceptions to trace down (on a side note, I wrote a library that helps with this). Furthermore, parts of ASP.NET are not async-compatible - most notably, MVC filters and child actions (they are fixing both of those with ASP.NET vNext). And ASP.NET has different behavior regarding timeouts and thread aborts for asynchronous handlers - adding yet a little more to the async learning curve.

Of course, the counter-counter argument would be that the proper response to behind-the-times developers is to train them, not restrict the technologies available.

In short:

The proper way to do async is "all the way". This is especially true on ASP.NET, and it's not likely to change anytime soon.
Whether async is appropriate, or helpful, is up to you and your application's scenario.


I think it is not possible to make async data access for sqlite or sqlce
update it seems async can be used even if the db only supports a single coonection thread at a time but more complex becase you ned to use locking
http://stackoverflow.com/questions/24186577/what-is-the-correct-usage-for-sqlite-on-locking-or-async
http://stackoverflow.com/questions/25945223/xamarin-forms-using-sqlite-net-async

async methods cannot have output parameters or ref parameters


good example here of async data access
http://codereview.stackexchange.com/questions/63480/simple-sqlhelper-which-wraps-ado-net-methods

http://blogs.msdn.com/b/adonet/archive/2012/04/20/using-sqldatareader-s-new-async-methods-in-net-4-5-beta.aspx
the above indicates that actually the async command methods like executereader have been there since 4.0 .net
with .NET 4.5 it can but does not have to be used even more async 
With .NET 4.5 developers using sequential access can further fine tune performance by selectively using NextResultAsync, ReadAsync, IsDBNullAsync, and GetFieldValueAsync<T>


    Use NextResultAsync whenever possible to handle packet processing asynchronously.
    Prefer ReadAsync in either mode. Again, much of the packet processing can be done asynchronously.
    Do NOT use IsDBNullAsync and GetFieldValueAsync in non-sequential mode. In this mode the columns are already buffered and you create Task objects for nothing.

For sequential mode, the decision to use GetFieldValueAsync is a bit more complicated. Daniel Paoliello explains,

    However, if you called Read in non-sequential access mode, or if you are using sequential access mode, then the decision is much harder as you need to consider how much data you need to read to get to your desired column and how much data that column may contain. If you’ve read the previous column, and the target column is small (like a Boolean, a DateTime or a numeric type) then you may want to consider using a synchronous method. Alternatively, if the target column is large (like a varbinary(8000)) or you need to read past large columns, then using an asynchronous method is much better. Finally, if the target column is massive (like a varbinary(MAX), varchar(MAX), nvarchar(MAX) or XML) then you should consider the new GetStream, GetTextReader or GetXmlReader methods instead.

There are advantages to using the stream-based methods when working large files stored in the database. For example, you can hand-off the stream to a WCF or ASP.NET response instead of bringing in the whole file into memory at one time. This is especially important for .NET developers because the large object heap is very susceptible to fragmentation.

http://www.tugberkugurlu.com/archive/asynchronous-database-calls-with-task-based-asynchronous-programming-model-tap-in-asp-net-mvc-4


http://stackoverflow.com/questions/23022573/calling-async-methods-from-a-synchronous-context

http://www.infoq.com/news/2012/05/ADO-Async

http://msdn.microsoft.com/en-us/library/hh191443.aspx

http://www.software-architects.com/devblog/2012/11/22/ADONET-45-Async-Data-Reader-and-IAsyncHttpHandler

http://stackoverflow.com/questions/21267580/asynchronous-programming-in-ado-net


http://blogs.msdn.com/b/rickandy/archive/2009/11/14/should-my-database-calls-be-asynchronous.aspx

