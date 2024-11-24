using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Library.Contracts.Dto;

public class BookDto
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public required Guid Id { get; init; } = default!;

    public required string Title { get; init; } = default!;

    public required string Author { get; init; } = default!;

    public required int YearOfRelease { get; init; } = default!;
}