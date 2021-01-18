using Website.Models;

namespace Website.ViewModels.Bookmarks
{
    public class BookmarkViewModel
    {
        public string Title { get; set; }
        public BookmarkType Type { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
    }
}