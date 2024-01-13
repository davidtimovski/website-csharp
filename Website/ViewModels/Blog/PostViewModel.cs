namespace Website.ViewModels.Blog;

public sealed class PostViewModel
{
    public required string Title { get; init; }
    public required string Body { get; init; }
    public required string Date { get; init; }
    public int? PreviousPostId { get; set; }
    public string? PreviousPostTitle { get; set; }
    public int? NextPostId { get; set; }
    public string? NextPostTitle { get; set; }
}