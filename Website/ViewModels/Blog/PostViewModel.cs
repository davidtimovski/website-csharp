namespace Website.ViewModels.Blog;

public class PostViewModel
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string Date { get; set; }
    public int? PreviousPostId { get; set; }
    public string PreviousPostTitle { get; set; }
    public int? NextPostId { get; set; }
    public string NextPostTitle { get; set; }
}