
https://miniprofiler.com/dotnet/AspDotNetCore
https://www.nuget.org/packages/MiniProfiler.AspNetCore.Mvc/


https://blogs.msdn.microsoft.com/dotnet/2018/08/02/tiered-compilation-preview-in-net-core-2-1/



https://weblog.west-wind.com/posts/2012/Nov/03/Back-to-Basics-When-does-a-NET-Assembly-Dependency-get-loaded
In a nutshell, referenced assemblies are not immediately loaded - they are loaded on the fly as needed. So regardless of whether you have an assembly reference in a top level project, or a dependent assembly assemblies typically load on an as needed basis, unless explicitly loaded by user code. The same is true of dependent assemblies.


https://medium.com/@indy_singh/strings-are-evil-a803d05e5ce3
