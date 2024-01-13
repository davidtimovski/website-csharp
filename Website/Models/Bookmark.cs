namespace Website.Models;

public class Bookmark
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required byte Type { get; init; }
    public required string Author { get; init; }
    public required string Url { get; init; }
}
