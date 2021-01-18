using System.Collections.Generic;

namespace Website.Models
{
    public class Expertise
    {
        public int Id { get; set; }
        public string Tech { get; set; }
        public string Answer { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }

        public List<Tag> Tags { get; set; } = new List<Tag>();
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ExpertiseTag
    {
        public ExpertiseTag(int expertiseId, int tagId)
        {
            ExpertiseId = expertiseId;
            TagId = tagId;
        }

        public int ExpertiseId { get; set; }
        public int TagId { get; set; }
    }
}
