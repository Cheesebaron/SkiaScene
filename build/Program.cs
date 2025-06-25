using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.DotNet.Restore;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

return new CakeHost()
    .UseContext<BuildContext>()
    .Run(args);

public class BuildContext : FrostingContext
{
    public string Target { get; set; }
    public string BuildConfiguration { get; set; }
    public string OutputDirArgument { get; set; }
    public string RootFolder { get; set; }
    public DirectoryPath OutputDir { get; set; }
    public DotNetVerbosity Verbosity { get; set; }
    public FilePath Solution { get; set; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        Target = context.Argument("target", "Default");
        BuildConfiguration = context.Argument("configuration", "Release");
        var verbosityArg = context.Argument("verbosity", "Normal");
        OutputDirArgument = context.Argument("outputDir", "./artifacts");
        OutputDir = new DirectoryPath(OutputDirArgument);
        Verbosity = DotNetVerbosity.Normal;
        RootFolder = context.Argument("rootDir", "../");
        Solution = new FilePath($"{RootFolder}source/SkiaScene.sln");
    }

    public DotNetMSBuildSettings GetDefaultMSBuildSettings()
    {
        var msBuildSettings = new DotNetMSBuildSettings
        {
            ContinuousIntegrationBuild = true
        };

        return msBuildSettings;
    }

    public DotNetBuildSettings GetDefaultBuildSettings(DotNetMSBuildSettings? msBuildSettings = null)
    {
        msBuildSettings ??= GetDefaultMSBuildSettings();

        var buildSettings = new DotNetBuildSettings
        {
            Configuration = BuildConfiguration,
            MSBuildSettings = msBuildSettings,
            Verbosity = Verbosity
        };

        return buildSettings;
    }
}

[TaskName("Clean")]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.CleanDirectories($"{context.RootFolder}source/**/bin");
        context.CleanDirectories($"{context.RootFolder}source/**/obj");
        context.CleanDirectories($"{context.RootFolder}samples/**/bin");
        context.CleanDirectories($"{context.RootFolder}samples/**/obj");
        context.CleanDirectory(context.OutputDir.FullPath);

        context.EnsureDirectoryExists(context.OutputDir);
    }
}

[TaskName("Restore")]
public sealed class RestoreTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetRestore(context.Solution.ToString());
    }
}

[TaskName("Build")]
[IsDependentOn(typeof(CleanTask))]
[IsDependentOn(typeof(RestoreTask))]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var settings = context.GetDefaultBuildSettings();
        context.DotNetBuild(context.Solution.ToString(), settings);
    }
}

[TaskName("CopyPackages")]
[IsDependentOn(typeof(BuildTask))]
public sealed class CopyPackagesTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var nugetFiles = context.GetFiles($"{context.RootFolder}source/**/*.nupkg");
        context.CopyFiles(nugetFiles, context.OutputDir);
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(CopyPackagesTask))]
public sealed class DefaultTask : FrostingTask<BuildContext>
{
}