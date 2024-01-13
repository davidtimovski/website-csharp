using System.Collections.Generic;

namespace Website.Models;

public sealed class Expertise
{
    public required int Id { get; init; }
    public required string Tech { get; init; }
    public required string Answer { get; init; }
    public required string Description { get; init; }
    public required string ImageUri { get; init; }

    public required List<Tag> Tags { get; set; } = [];
}

public sealed class Tag
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public sealed class ExpertiseTag(int expertiseId, int tagId)
{
    public int ExpertiseId { get; } = expertiseId;
    public int TagId { get; } = tagId;
}
