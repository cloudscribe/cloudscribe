# Storage 

There are so many options for data storage and different people recommending their favorites.

http://renesd.blogspot.com/2017/02/is-postgresql-good-enough.html

## Aaron Stannard of Akk.NET fame recommends Cassandra
http://cassandra.apache.org/
The Apache Cassandra database is the right choice when you need scalability and high availability without compromising performance. Linear scalability and proven fault-tolerance on commodity hardware or cloud infrastructure make it the perfect platform for mission-critical data.Cassandra's support for replicating across multiple datacenters is best-in-class, providing lower latency for your users and the peace of mind of knowing that you can survive regional outages.

http://db-engines.com/en/system/Cassandra#a32
Apache Cassandra is the leading NoSQL, distributed database management system driving many of today's modern business applications by offering continuous availability, high scalability and performance, strong security, and operational simplicity while lowering overall cost of ownership.

apparently this only runs on linux and requires java
Prerequisites
The latest version of Java 8, either the Oracle Java Standard Edition 8 or OpenJDK 8. To verify that you have the correct version of java installed, type java -version.
For using cqlsh, the latest version of Python 2.7. To verify that you have the correct version of Python installed, type python --version.

Probably the best way to run this on windows dev and in production is using Docker
https://hub.docker.com/_/cassandra/

http://cassandra.apache.org/doc/latest/

there is a beta nuget driver for .NET Core
https://www.nuget.org/packages/CassandraCSharpDriver/3.1.0-beta1
https://github.com/datastax/csharp-driver

Given that I want to use Akka.NET, Aaron Stannard's recommendation carries a lot of weight with me. The only downside is it does not run on windows and requires java. This is ok for enterprise deployment but a drawback in terms of my apps being adopted by windows developers because the whole stack cannot be installed on a single dev machine except with docker. So, I think the best idea is to support it but also support other options


## ReactiveTrader demo uses this
Functional Database
Event Store stores your data as a series of immutable events over time, making it easy to build event-sourced applications.
https://geteventstore.com/
"EventStore.ClientAPI.DotNetCore": "1.0.0" supports .NET Core
Event Store is permissively licensed under the 3-clause BSD license

downloads available for windows, linux, and osx, but still might be better to use docker
https://hub.docker.com/r/adbrowne/eventstore/~/dockerfile/

http://docs.geteventstore.com/
http://docs.geteventstore.com/introduction/3.9.0//
http://docs.geteventstore.com/introduction/3.9.0/event-sourcing-basics/

looks like it can be used with Akka.NET
http://www.4deeptech.com/blog/event-sourcing-with-akka-net-and-event-store
https://github.com/4deeptech/AkkaESSample