using System.Collections.Generic;

namespace Website.ViewModels.Expertise;

public class ExpertiseDto
{
    public int Id { get; set; }
    public string Tech { get; set; }
    public string Answer { get; set; }
    public string Description { get; set; }
    public string ImageUri { get; set; }

    public List<string> Tags { get; set; } = new List<string>();
}
