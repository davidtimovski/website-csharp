using Website.Models;

namespace Website.ViewModels.Bookmarks;

public sealed class BookmarkViewModel
{
    public required string Title { get; init; }
    public required BookmarkType Type { get; init; }
    public required string Author { get; init; }
    public required string Url { get; init; }
}