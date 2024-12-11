using _buid.Common.Extensions;
using Nuke.Common;
using Nuke.Common.Tools.Docker;
using Serilog;
using static Nuke.Common.Tools.Docker.DockerTasks;

namespace _buid.Common;

[ParameterPrefix(nameof(Docker))]
public interface IDockerBuild : IBaseBuild
{
    const string DockerContainerArtifactsPath = "/app/artifacts";
    string DockerfilePath => RootDirectory / "build" / "Dockerfile";

    [Parameter("Docker user name"), Required]
    string Username => this.GetValue(() => Username);

    [Parameter("Docker project name"), Required]
    string ProjectName => this.GetValue(() => ProjectName);

    /// <summary>
    /// Dockerfile processing pipeline: build -> create container -> copy artifacts -> remove container
    /// </summary>
    Target BuildDockerfile => _ => _
        .Requires(() => Username)
        .Requires(() => ProjectName)
        .Executes(() =>
        {
            this.BuildDocker();

            var containerId = this.CreateDockerContainer();
            this.CopyArtifactsFromContainer(containerId);

            this.RemoveDockerContainer(containerId);
        });

    /// <summary>
    /// Push Docker image to the repository
    /// </summary>
    Target PushDockerArtifacts => _ => _
        .Requires(() => Username)
        .TryDependsOn<IDockerBuild>(x => x.BuildDockerfile)
        .Executes(() =>
        {
            var imageTag = ResolveDockerImageNameTag();
            DockerPush(settings => settings.SetName(imageTag));
            Log.Information("Docker image {DockerImageName} was pushed to {DockerImageTag}", ProjectName, imageTag);
        });

    string ResolveDockerImageNameTag()
    {
        var version = this.ResolveVersion();
        return $"{Username}/{ProjectName}:{version.Version}";
    }
}
