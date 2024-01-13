using System.Collections.Generic;

namespace Website.ViewModels.Expertise;

public sealed class ExpertiseDto
{
    public required int Id { get; init; }
    public required string Tech { get; init; }
    public required string Answer { get; init; }
    public required string Description { get; init; }
    public required string ImageUri { get; init; }

    public required List<string> Tags { get; init; } = [];
}
