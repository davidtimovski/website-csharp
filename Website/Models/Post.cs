using System;

namespace Website.Models;

public class Post
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required string Body { get; init; }
    public required byte Status { get; init; }
    public required DateTime DateCreated { get; init; }
    public required DateTime DateModified { get; init; }
}
