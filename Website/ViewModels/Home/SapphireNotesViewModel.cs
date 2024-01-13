namespace Website.ViewModels.Home;

public sealed class SapphireNotesViewModel
{
    public required string Version { get; init; }
    public required string ReleaseDate {  get; init; }
    public required string WindowsFileSize { get; init; }
    public required string DebianUbuntu64FileSize { get; init; }
    public required string DebianUbuntuARMFileSize { get; init; }
}
