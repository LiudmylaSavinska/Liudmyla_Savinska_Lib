using MongoDB.Driver;

namespace Library.Database;

public interface IMongoDbConnectionFactory
{
    IMongoDatabase GetDatabase();
}