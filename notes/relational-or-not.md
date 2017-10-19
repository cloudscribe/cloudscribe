
Marko Hrovatic @brgrz Apr 30 2016 07:56
@brgrz
@joeaudette just read about your cloudscribe project. How do you intend to support both relational and non-relational datastores? The concepts are quite different. I have extended experience with SQL and some experience with Mongo and Raven DB. Personally I don't think the concepts are interhangeable.
@joeaudette I'm embarking on a similar project to your cloudscribe. I'm trying to decide whether to go through all of the trouble of implementing and supporting auth system like you intend to do (I can see a lot of cloudscribe revolves around managing users, adding support for IdentityServer, etc.)
The other option for me would be to just use Auth0 or similar service..relatively cheap and everything is solved for us. I don't see us implementing and managing a cross-cutting concern like auth ourselves. Not even employeing an OSS solution like IdentityServer.

Joe Audette @joeaudette Apr 30 2016 08:24
@brgrz sorry for confusion, I answered in the saaskit gitter room since it is really just theoretical data modeling question and since Ben was also participating in the discussion, but if you have more questions about cloudscribe feel free to ask them here

Marko Hrovatic @brgrz Apr 30 2016 15:58
@joeaudette since you said you weren't using navigation properties either, could you point me to a file in your source code where you do EF lookups (queries)?
@joeaudette coz you probably know off the top of your head while I'd have to scan dozens of files..

Joe Audette @joeaudette May 01 2016 07:10
@brgrz ok but some of my queries might not be the prettiest or most optimized, I've only started using EF about 4 months ago. My UserRepository is here. My User object has a SiteGuid for example and I can lookup users by site based on that without any navigation property, it is indexed. I don't even use foreign keys, I manage deleting child related objects from application code, no cascading deletes. I would say the most difficult query was GetUsersNotInRole method which uses 3 tables Users cross join Roles, join UserRoles. So I do use a UserRole entity to represent the join, it only has the ids though, no navigation property of User or Role. I do remember struggling with this query and similar ones until I got it working, it probably would have been easier if there were navigation properties but I did not want to model it with navigation properties

Marko Hrovatic @brgrz May 01 2016 07:23
I see. Took a look at your UserRepository and it looks kinda cool doing it that way. Everything's nicely separated. The two issues I have with your approach are: 1. you're basically throwing the "relational" in EF out the window (not using relations, FKs, etc.) and 2. your code will get messy when you'll have to do even more lookups for tables that might be related to numerous other tables - and your code or data layer will be very chatty this way (instead of using navigation properties, linq or lambdas to construct a single query), it'll be doing lots of small queries against the datastore (which I won't say it's wrong but it makes me wonder)

Joe Audette @joeaudette May 01 2016 07:38
@brgrz I'm not going to worry about imaginary future tables or imaginary problems that might exist later, I know the tradeoffs I have chosen. EF is just rapid development storage tool. The whole NoSql movement is born out of the fact that rules of relational are over rated for many scenarios. I have 6 more different repository implementations here that use actual ado and sql, the MSSQL version uses stored procs. Those implementations are not currently up to date and not working but they were working about 4 months ago and existed before I started working with EF. for example if I needed better perf than EF I could revive the MS SQL one and use stored procs and tune things in the db rather than in the application code. My modal started out from my older mojoportal project and most of that data access code was refactored from existing mojoportal code. I've been violating some of the ideals of relational for a long time and it has worked out fine for me. Even EF team is planning on supporting non-relational storage in the future so I would loosen up on notions that EF implementations must follow relational ideals at all times

