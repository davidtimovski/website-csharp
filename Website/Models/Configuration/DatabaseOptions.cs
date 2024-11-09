namespace Website.Models.Configuration;

public sealed class DatabaseOptions
{
    public const string Section = "ConnectionStrings";

    public required string DefaultConnectionString { get; init; }
}
