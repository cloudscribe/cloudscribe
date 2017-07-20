
http://www.hossambarakat.net/2016/02/03/configuring-redis-as-asp-net-core-1-0-session-store/

https://www.devtrends.co.uk/blog/a-guide-to-caching-in-asp.net-core

0 Things To Know About In-Memory Caching In ASP.NET Core
http://www.binaryintellect.net/articles/a7d9edfd-1f86-45f8-a668-64cc86d8e248.aspx

Distributed cache
https://github.com/aspnet/Caching/blob/dev/src/Microsoft.Framework.Caching.Abstractions/IDistributedCache.cs

Memory cache
https://github.com/aspnet/Caching/blob/dev/src/Microsoft.Framework.Caching.Abstractions/IMemoryCache.cs

when using distributed cache on your local machine you can fake it using LocalCache
which is an implementation of IDistributedCache but internally implemented with memory cache.

for deployment you can swap it out by dependency injection with Redis or another implementation

https://github.com/aspnet/Caching

http://www.morgantechspace.com/2013/08/convert-object-to-byte-array-and-vice.html

http://stackoverflow.com/questions/27304210/how-do-i-apply-the-outputcache-attribute-on-a-method-in-a-vnext-project

when to use memory cache or distributed cache

if the site is running on a single machine it always makes sense to use memory cache

in distributed environments aka web farms aka azure aka the cloud
you generally do want to use distributed cache
especially in azure you want to cache data retrieved from sqlazure because data store egress as the gu called it is expensive.
accessing the cache should be faster than accessing sqlazure
distributed cache such as azure cach costs money too, I suspect it still saves money on sqlazure hits to cache most things that are frequently accessed.

"Caching works best for application workloads that do more reading than writing of data, and when the data model supports the key/value organization that you use to store and retrieve data in cache. It's also more useful when application users share a lot of common data; for example, cache would not provide as many benefits if each user typically retrieves data unique to that user. An example where caching could be very beneficial is a product catalog, because the data does not change frequently, and all customers are looking at the same data.

The benefit of caching becomes increasingly measurable the more an application scales, as the throughput limits and latency delays of the persistent data store become more of a limit on overall application performance. However, you might implement caching for other reasons than performance as well. For data that doesn't have to be perfectly up-to-date when shown to the user, cache access can serve as a circuit breaker for when the persistent data store is unresponsive or unavailable." Scott Guthrie

http://www.asp.net/aspnet/overview/developing-apps-with-windows-azure/building-real-world-cloud-apps-with-windows-azure/distributed-caching

It seems with distributed cache the api just accepts byte[] input and output whereas memory cache provides the more convenient object class for in and out so you just cast it back to the original object.

with distributed cache you have to pass in a byte[] which is easy to create from a string so basically serialize to string, convert to byte array and store.
often/typically I suspect the cache provider perists the object as a string as well but that is their internal business not ours
retrieval is the reverse we get back a byte[] convert it to string and then deserialize it back to object.

so clearly we hope our serialization and deserialization process is not itself expensive
and if it were we would not solve it with caching since that would just add more layers of serialization/deserialization

in the case of our navigation.xml and navigation.json files, these would live on each node of the web farm or azure cloud
so it might seem best to just use memory cache to reduce local file IO on each node
but we expect other implementations of INavigationTreeBuilder later from cms that will build its tree from the database
and definitely that will want to be in distributed cache
in fact the node can be chained together with multiple INavigationTreeBuilders adding more nodes to the tree and the whole object should be cached

so even for this scenario it seems distributed cache is what we should use

Is there any guidance for caching patterns in ASP.NET 5
http://stackoverflow.com/questions/31458950/is-there-any-guidance-for-caching-patterns-in-asp-net-5

For my semi expensive BuildTree method that deserializes from string, suppose I register my ITreebuilder as a singleton in DI.
It can check its local variable for the tree so only on first request would it need to build since it would be kept in memory for the life of the app right? each server node has its own copy in memory in this case right? it would check the cache before building, just need a way to know if the cache was invalidated so it can rebuild the internal copy if needed.

How to configured nested dependency in ASP.NET 5 DI?

I would like to wrap a repository inside another repository that will handle caching while internally using the passed in repository.
This way my caching logic can be completely separate from the repository implementation. This pattern would also allow me to easily change from memory cache to distributed cache, that is I could have different caching repositories that use different cache types so I can plug them in depending on the environment. On Azure I could use distributed cache but on a single server I could use memory cache for example.

public sealed class CachingFooRepository : IFooRepository
{
	private IFooRepository repo;
	private IMemoryCache  cache;

	public CachingFooRepository(IFooRepository implementation, IMemoryCache  cache, IFooCachingRules)
	{
		if ((implementation == null)||(implementation is CachingFooRepository))
		{
			throw new ArgumentException("you must pass in an implementation of IFooRpository");
		}
		repo = implementation;
		this.cache = cache;
	}

	public async Task<bool> Save(IFooItem foo)
	{
		// TODO throw if foo is null
		bool result = await repo.Save(user);
		string key = "Key-" + foo.Id.ToString();
		cache.Remove(key);
		return result;
	}
	
	public async Task<IFooItem> Fetch(int fooId)
    {
		string key = "Key-" + fooId.ToString();
		object result = cache.Get(key);
		if(result != null) { return (foo)result; }
		foo = await repo.Fetch(fooId);
		if(foo != null)
		cache.Set(key, foo);
		
		return foo;
	}
	
}

Obviously while CachingFooRepository implements IFooRepository, I must make sure that a different implementation of IFooRepository is passed into the constructor for CachingFooRepository since it isn't a real implementation of IFooREpository but depends on a real implementation.

This example is simplified, pseudo-ish code, whether to cache and how long to cache can be passed in as IFooCachingRules or IOptions<FooCachingRepository> or something like that.

So my question is how can I reigster the services in such a way that things that depend on IFooRepository will get an instance of CachingFooRepository, but CachingFooRepository will get some other implementation of IFooRepository such as SqlFooRepository?
I would likke to keep other classes depending only on IFooRepository, I don't want to make anything depend specifically on CachingFooRepository.

Is this possible, feasible, a good idea, a bad idea?

what if I just define public interface IFooRepositoryWrapper : IFooRepository, with no additional methods or properties vs IFooRepository. Then I make a DefaultFooRepositoryWrapper that takes IFooRepository and delegates all the methods to it. I can make most classes depend on IFooRepositoryWrapper instead of on IFooRepository, then I can implement CachingFooWrapper. This I think should work with the default DI.

http://stackoverflow.com/questions/31492976/how-to-configured-nested-dependency-in-asp-net-5-di

ok so IFooRepositoryWrapper is probably a bad idea
in order to be able to use decorator pattern I probably should just switch to AutoFac instead of the standard DI
http://druss.co/2015/05/vnext-asp-net-5-dependency-injection/
then I can wire up FooCachingRepository as in this example:
http://nblumhardt.com/2011/01/decorator-support-in-autofac-2-4/

