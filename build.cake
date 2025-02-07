var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var verbosityArg = Argument("verbosity", "Normal");
var outputDirArgument = Argument("outputDir", "./artifacts");
var outputDir = new DirectoryPath(outputDirArgument);
var verbosity = DotNetVerbosity.Normal;
var sln = new FilePath("./source/SkiaScene.sln");

Task("Clean")
    .Does(() =>
{
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
    CleanDirectory(outputDir.FullPath);

    EnsureDirectoryExists(outputDir);
});

Task("Restore").Does(() => DotNetRestore(sln.ToString()));

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = GetDefaultBuildSettings();
    DotNetBuild(sln.ToString(), settings);
});

Task("CopyPackages")
    .IsDependentOn("Build")
    .Does(() =>
{
    var nugetFiles = GetFiles("./source/**/*.nupkg");
    CopyFiles(nugetFiles, outputDir);
});

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("CopyPackages");

RunTarget(target);

DotNetMSBuildSettings GetDefaultMSBuildSettings()
{
    var msBuildSettings = new DotNetMSBuildSettings
    {
        ContinuousIntegrationBuild = true
    };

    return msBuildSettings;
}

DotNetBuildSettings GetDefaultBuildSettings(DotNetMSBuildSettings? msBuildSettings = null)
{
    msBuildSettings ??= GetDefaultMSBuildSettings();

    var buildSettings = new DotNetBuildSettings
    {
        Configuration = configuration,
        MSBuildSettings = msBuildSettings,
        Verbosity = verbosity
    };

    return buildSettings;
}
