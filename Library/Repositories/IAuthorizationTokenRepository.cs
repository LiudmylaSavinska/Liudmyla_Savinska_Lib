using Library.Contracts.Dto;

namespace Library.Repositories;

public interface IAuthorizationTokenRepository
{
    public Task<AuthorizationTokenDto?> GetTokenByUserId(Guid userId);

    public Task<AuthorizationTokenDto?> GetToken(string token);

    public Task AddToken(AuthorizationTokenDto token);
    public Task<bool> DeleteToken(Guid userId);
}