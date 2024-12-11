namespace _buid.Common.Extensions;

internal record ApplicationVersion(
    string AssemblyVersion,
    string Version,
    string FileVersion,
    string InformationalVersion);