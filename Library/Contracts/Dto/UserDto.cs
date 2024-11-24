using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Contracts.Dto;

public class UserDto
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public required Guid Id { get; init; } = default!;

    public required string FullName { get; init; } = default!;

    public required string NickName { get; init; } = default!;

    public required string Password { get; init; } = default!;
}