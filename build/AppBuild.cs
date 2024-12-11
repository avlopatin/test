using _buid.Common;
using Nuke.Common;

class AppBuild : NukeBuild, IDefaultBuild
{
    public string ServiceName => "Api.Endpoint";

    public int Major => 1;

    public int Minor => 5;

    public static int Main()
        => Execute<AppBuild>(x => ((IDefaultBuild)x).Default);
}
