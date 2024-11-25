using Library.Contracts.Dto;
using Library.Repositories;
using Library.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MongoDB.Driver;

namespace Library.Endpoints.Books;

public static class DeleteBookEndpoint
{
    public const string Name = "DeleteBook";

    public static IEndpointRouteBuilder MapDeleteBook(this IEndpointRouteBuilder app)
    {
        app
            .MapDelete(ApiEndpoints.Books.Delete, async (
                string title,
                string author,
                string token,
                IBookRepository repository,
                IUserAuthorizationService service) =>
            {
                if (!await service.IsAuthorizedByToken(token)) return Results.Unauthorized();


                var book = await repository.GetBook(b => b.Title == title && b.Author == author);

                if (book is null) return Results.NotFound($"Book :{title} by {author} not found");

                var filter = Builders<BookDto>.Filter.Eq(b => b.Title, title)
                             & Builders<BookDto>.Filter.Eq(b => b.Author, author);

                await repository.Delete(filter);

                return Results.Ok($"{title} by {author} deleted");
            })
            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}