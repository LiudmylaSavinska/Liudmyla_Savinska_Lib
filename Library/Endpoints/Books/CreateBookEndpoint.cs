using Library.Contracts.Domain;
using Library.Contracts.Mappings;
using Library.Repositories;
using Library.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Library.Endpoints.Books;

public static class CreateBookEndpoint
{
    public const string Name = "CreateBook";

    public static IEndpointRouteBuilder MapCreateBook(this IEndpointRouteBuilder app)
    {
        app
            .MapPost(ApiEndpoints.Books.Create, async (
                string token,
                Book book,
                IBookRepository repository,
                IUserAuthorizationService service) =>
            {
                if (!await service.IsAuthorizedByToken(token)) return Results.Unauthorized();

                if (await repository.Exists(book))
                    return Results.BadRequest($"{book.Title} by {book.Author}, {book.YearOfRelease} already exists");

                await repository.AddBook(book.ToDto());

                return TypedResults.CreatedAtRoute(book, Name, new { title = book.Title });
            })
            .WithName(Name)
            .Produces<Book>()
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}