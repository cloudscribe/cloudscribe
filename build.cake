#tool nuget:?package=xunit.runner.console&version=2.3.0
#tool nuget:?package=xunit.runner.visualstudio&version=2.3.0

#load cake/paths.cake

var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");


Task("Restore")
    .Does(() =>
{
    NuGetRestore(Paths.SolutionFile);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    MSBuild(
        Paths.SolutionFile,
        settings => settings.SetConfiguration(configuration)
                            .WithTarget("Build"));
});

Task("Test")  
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("./test/**/*.csproj");
        foreach(var project in projects)
        {
            DotNetCoreTool(
                projectPath: project.FullPath, 
                command: "xunit", 
                arguments: $"-configuration {configuration} -diagnostics -stoponfail"
            );
        }
    });

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);
