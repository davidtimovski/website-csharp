namespace Website.Models
{
    public class Bookmark
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte Type { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
    }
}
