using System.Linq.Expressions;
using Library.Contracts.Domain;
using Library.Contracts.Dto;
using MongoDB.Driver;

namespace Library.Repositories;

public interface IBookRepository
{
    public Task AddBook(BookDto book);
    public Task<BookDto?> GetBook(Expression<Func<BookDto, bool>> filter);
    public Task<List<BookDto>> GetMany(Expression<Func<BookDto, bool>> filter);
    public Task<bool> Delete(FilterDefinition<BookDto> filterDefinition);
    public Task<bool> Exists(Book book);
}