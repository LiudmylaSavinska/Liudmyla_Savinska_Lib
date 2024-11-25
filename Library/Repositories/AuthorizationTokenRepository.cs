using Library.Contracts.Dto;
using Library.Database;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Library.Repositories;

public class AuthorizationTokenRepository : IAuthorizationTokenRepository
{
    private const string CollectionName = "tokens";
    private readonly ILogger<AuthorizationTokenRepository> _logger;
    private readonly IMongoCollection<AuthorizationTokenDto> _collection;

    public AuthorizationTokenRepository(
        ILogger<AuthorizationTokenRepository> logger,
        IMongoDbConnectionFactory connectionFactory)
    {
        _logger = logger;
        _collection = connectionFactory
            .GetDatabase()
            .GetCollection<AuthorizationTokenDto>(CollectionName);
    }

    public async Task<AuthorizationTokenDto?> GetTokenByUserId(Guid userId)
    {
        AuthorizationTokenDto? tokenDto = null;
        try
        {
            tokenDto = await _collection
                .Find(t => t.UserId.ToString() == userId.ToString())
                .FirstOrDefaultAsync();
        }
        catch (MongoException e)
        {
            _logger.LogError(e, "InnerError is {inner}", e.InnerException);
        }

        return tokenDto;
    }

    public async Task<AuthorizationTokenDto?> GetToken(string token)
    {
        AuthorizationTokenDto? tokenDto = null;
        try
        {
            tokenDto = await _collection
                .Find(t => t.Token == token)
                .FirstOrDefaultAsync();
        }
        catch (MongoException e)
        {
            _logger.LogError(e, "InnerError is {inner}", e.InnerException);
        }

        return tokenDto;
    }

    public async Task AddToken(AuthorizationTokenDto token)
    {
        try
        {
            var tokenDto = await _collection
                .Find(t => t.Token == token.Token)
                .FirstOrDefaultAsync();

            if (tokenDto is not null)
            {
                _logger.LogWarning("Token with value {token} already exists", token.Token);
            }
            else
            {
                await _collection.InsertOneAsync(token);
            }
        }
        catch (MongoException e)
        {
            _logger.LogError(e, "InnerError is {inner}", e.InnerException);
        }
    }

    public async Task<bool> DeleteToken(Guid userId)
    {
        var result = await _collection.DeleteOneAsync(t => t.UserId == userId);
        return result.IsAcknowledged;
    }
}