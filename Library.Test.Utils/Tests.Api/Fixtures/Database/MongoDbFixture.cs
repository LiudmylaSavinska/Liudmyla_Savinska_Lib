using Library.Contracts.Dto;
using MongoDB.Driver;

namespace Library.Test.Utils.Tests.Api.Fixtures.Database;

public sealed class MongoDbFixture
{
    public MongoDbCollection<UserDto> Users { get; }
    public MongoDbCollection<BookDto> Books { get; }
    public MongoDbCollection<AuthorizationTokenDto> AuthTokens { get; }
    
    public MongoDbFixture(string connectionString, string dbName)
    {
        var mongoClient = new MongoClient(connectionString);
        Users = new MongoDbCollection<UserDto>(mongoClient.GetDatabase(dbName), "users");
        Books = new MongoDbCollection<BookDto>(mongoClient.GetDatabase(dbName), "books");
        AuthTokens = new MongoDbCollection<AuthorizationTokenDto>(mongoClient.GetDatabase(dbName), "authorizationTokens");
    }
}