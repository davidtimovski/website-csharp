namespace Website.ViewModels.Home;

public sealed class TeamSketchViewModel
{
    public required string Version { get; init; }
    public required string ReleaseDate {  get; init; }
    public required string WindowsFileSize { get; init; }
    public required string DebianUbuntu64FileSize { get; init; }
    public required string DebianUbuntuARMFileSize { get; init; }
}
