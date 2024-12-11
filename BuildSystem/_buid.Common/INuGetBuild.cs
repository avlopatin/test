using _buid.Common.Extensions;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace _buid.Common;

[ParameterPrefix(nameof(NuGet))]
public interface INuGetBuild: IBaseBuild
{
    [Parameter("NuGet API key"), Required]
    string ApiKey => this.GetValue(() => ApiKey);

    [Parameter("NuGet Source"), Required]
    string Source => this.GetValue(() => Source);

    AbsolutePath NuGetArtifactsPath => ArtifactsPath / "nuget";

    Target PushNuGetArtifacts => _ => _
        .Requires(() => ApiKey)
        .Requires(() => Source)
        .TryDependsOn<IDockerBuild>(x => x.BuildDockerfile)
        .Executes(() =>
        {
            DotNetNuGetPush(settings =>
                settings
                    .SetTargetPath(NuGetArtifactsPath / "*.nupkg")
                    .SetSource(Source)
                    .SetApiKey(ApiKey)
                    .EnableSkipDuplicate()
                    .EnableForceEnglishOutput());

            var pushedArtifacts = NuGetArtifactsPath.GlobFiles("*.nupkg")
                .Select(x => x.Name)
                .ToList();

            Log.Information("Nuget artifacts {NuGetArtifacts} were successfully pushed to {Source}", NuGetArtifactsPath, Source);
        });
}
