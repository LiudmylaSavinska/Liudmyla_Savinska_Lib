using Library.Contracts.Domain;

namespace Library.Services;

public interface IUserAuthorizationService
{
    public Task<bool> IsAuthorizedByToken(string authorizationToken);
    public Task<bool> IsAuthorizedByNickName(string nickName);

    public Task<AuthorizationToken?> GenerateToken(Guid userId);

    public Task<AuthorizationToken?> GetToken(Guid userId);
}