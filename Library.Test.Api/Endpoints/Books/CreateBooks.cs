using System.Net;
using Library.Contracts.Domain;
using Library.Test.Api.TestFixtures;
using Library.Test.Utils.Tests.Api.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Library.Test.Api.Endpoints.Books;

public class CreateBooks : GlobalSetUp
{
    private Book _book;

    [OneTimeSetUp]
    public new async Task OneTimeSetUp()
    {
        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.AuthorizeLikeDefaultUser();
        _book = DataHelper.CreateBook();
    }

    [Test]
    [Description("This test checks if the book is created successfully")]
    
    public async Task CreateBook_WhenDataIsValid_ReturnCreated()
    {
        HttpResponseMessage response = await LibraryHttpService.PostBook(_book);

        var jsonString = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<Book>(jsonString);

        var booksDto = await MongoDbFixture.Books.GetItem(b => b.Title == books.Title);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(books.Title, Is.EqualTo(_book.Title));
            Assert.That(books.Author, Is.EqualTo(_book.Author));
            Assert.That(books.YearOfRelease, Is.EqualTo(_book.YearOfRelease));
            Assert.That(booksDto, Is.Not.Null);
            Assert.That(booksDto.Title, Is.EqualTo(_book.Title));
        });
    }

    [Test]
    public async Task CreateBook_WhenTokenIsInvalid_ReturnUnauthorized()
    {
        HttpResponseMessage response = await LibraryHttpService.PostBook("invalid", _book);

        var jsonString = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        });
    }

    
}