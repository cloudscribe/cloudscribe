mkdir nupkgs
mkdir cloudscribe.AspNet.Identity\lib
mkdir cloudscribe.AspNet.Identity\lib\net45

copy ..\src\cloudscribe.AspNet.Identity\bin\Release\cloudscribe.AspNet.Identity.dll cloudscribe.AspNet.Identity\lib\net45
copy ..\src\cloudscribe.AspNet.Identity\bin\Release\cloudscribe.AspNet.Identity.pdb cloudscribe.AspNet.Identity\lib\net45

NuGet.exe pack cloudscribe.AspNet.Identity\cloudscribe.AspNet.Identity.nuspec -OutputDirectory "nupkgs"
