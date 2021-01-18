using System.Collections.Generic;
using Website.Models;

namespace Website.ViewModels.Expertise
{
    public class CreateExpertiseViewModel
    {
        public CreateExpertiseViewModel() { }

        public CreateExpertiseViewModel(IEnumerable<Tag> tagOptions)
        {
            TagOptions = tagOptions;
        }

        public string Tech { get; set; }
        public string Answer { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }

        public IEnumerable<int> TagIds { get; set; }
        public IEnumerable<Tag> TagOptions { get; set; }
    }
}
