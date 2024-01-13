using System.Collections.Generic;
using Website.Models;

namespace Website.ViewModels.Expertise;

public sealed class EditExpertiseViewModel
{
    public required int Id { get; init; }
    public required string Tech { get; init; }
    public required string Answer { get; init; }
    public required string Description { get; init; }
    public required string ImageUri { get; init; }

    public required IEnumerable<int> TagIds { get; init; }
    public required IEnumerable<Tag> TagOptions { get; init; }
}
