using System.Collections.Generic;
using Website.Models;

namespace Website.ViewModels.Expertise;

public class EditExpertiseViewModel
{
    public int Id { get; set; }
    public string Tech { get; set; }
    public string Answer { get; set; }
    public string Description { get; set; }
    public string ImageUri { get; set; }

    public IEnumerable<int> TagIds { get; set; }
    public IEnumerable<Tag> TagOptions { get; set; }
}
