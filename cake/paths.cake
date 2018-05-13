public static class Paths
{
    public static FilePath SolutionFile => "cloudscribe.sln";
    
}

public static FilePath Combine(DirectoryPath directory, FilePath file)
{
    return directory.CombineWithFilePath(file);
}

public DirectoryPath VS2017InstallDirectory(ICakeContext context)
{
    var programFilesX86 = context.Environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
    string[] editions  = { "Enterprise", "Professional", "Community" };

    return editions
        .Select(edition => Directory($"{programFilesX86}/Microsoft Visual Studio/2017/{edition}"))
        .FirstOrDefault(path => context.DirectoryExists(path));
}
