::mkdir BrockAllen.MembershipReboot\lib\net45
::xcopy ..\build\BrockAllen.MembershipReboot.dll BrockAllen.MembershipReboot\lib\net45 /y
::xcopy ..\build\BrockAllen.MembershipReboot.pdb BrockAllen.MembershipReboot\lib\net45 /y
NuGet.exe pack cloudscribe.Core.Web\cloudscribe.Core.Web.nuspec -OutputDirectory .
