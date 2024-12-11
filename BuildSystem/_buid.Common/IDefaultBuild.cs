using _buid.Common.Extensions;
using Nuke.Common;
using Serilog;

namespace _buid.Common;

public interface IDefaultBuild
    : IDockerBuild
    , INuGetBuild
{
    Target Default => _ => _
        .TryDependsOn<IDockerBuild>(x => x.PushDockerArtifacts)
        .TryDependsOn<INuGetBuild>(x => x.PushNuGetArtifacts)
        .Executes(() =>
        {
            var version = this.ResolveVersion();
            Log.Information("{ServiceName}:{ReleaseName} was successfully built", version.Version, ServiceName);
        });
}
