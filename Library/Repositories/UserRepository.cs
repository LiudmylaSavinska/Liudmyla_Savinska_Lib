using System.Linq.Expressions;
using Library.Contracts.Domain;
using Library.Contracts.Dto;
using Library.Contracts.Mappings;
using Library.Database;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Library.Repositories;

public class UserRepository : IUserRepository
{
    private const string CollectionName = "users";
    private readonly ILogger<UserRepository> _logger;
    private readonly IMongoCollection<UserDto> _collection;

    public UserRepository(
        ILogger<UserRepository> logger,
        IMongoDbConnectionFactory connectionFactory)
    {
        _logger = logger;
        _collection = connectionFactory
            .GetDatabase()
            .GetCollection<UserDto>(CollectionName);
    }

    public async Task<UserDto?> GetUser(Expression<Func<UserDto, bool>> filter)
    {
        UserDto? userDto = null;
        try
        {
            userDto = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
        }
        catch (MongoException e)
        {
            _logger.LogError(e, "InnerError is {inner}", e.InnerException);
        }

        return userDto;
    }

    public async Task<bool> AddUser(User user)
    {
        try
        {
            var userDto = await _collection
                .Find(u => u.NickName == user.NickName)
                .FirstOrDefaultAsync();

            if (userDto != null) return false;

            await _collection.InsertOneAsync(user.ToDto());
        }
        catch (MongoException e)
        {
            _logger.LogError(e, "InnerError is {inner}", e.InnerException);
        }

        return true;
    }
}