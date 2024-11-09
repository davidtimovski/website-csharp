namespace Website.Models.Configuration;

public sealed class SapphireNotesOptions
{
    public const string Section = "SapphireNotes";

    public required string Version { get; init; }
    public required string ReleaseDate { get; init; }
    public required string WindowsFileSize { get; init; }
    public required string DebianUbuntu64FileSize { get; init; }
    public required string DebianUbuntuARMFileSize { get; init; }
}
