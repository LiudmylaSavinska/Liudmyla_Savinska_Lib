using System.Linq.Expressions;
using Library.Contracts.Domain;
using Library.Contracts.Dto;

namespace Library.Repositories;

public interface IUserRepository
{
    public Task<UserDto?> GetUser(Expression<Func<UserDto, bool>> filter);
    public Task<bool> AddUser(User user);
}