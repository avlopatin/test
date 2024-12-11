using _buid.Common.Extensions;
using Nuke.Common;
using Nuke.Common.IO;

namespace _buid.Common;

public interface IBaseBuild: INukeBuild
{
    string ServiceName { get; }

    int Major { get; }

    int Minor { get; }

    [Parameter("Build counter"), Required]
    string BuildCounter => this.GetValue(() => BuildCounter);

    [Parameter("Branch name"), Required]
    string Branch => this.GetValue(() => Branch);

    AbsolutePath ArtifactsPath => RootDirectory / ".artifacts";
}
