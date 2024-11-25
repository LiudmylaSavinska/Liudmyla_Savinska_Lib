using System.Net;
using Bogus;
using Library.Contracts.Domain;
using Library.Test.Api.TestFixtures;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Library.Test.Api.Endpoints.Books;

[TestFixture]

public class GetBooks : GlobalSetUp
{
    private Book _newBook;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.AuthorizeLikeDefaultUser();

        var faker = new Faker();
        _newBook = new Book
        {
            Author = "Joan Rowling",
            Title = faker.Random.AlphaNumeric(4),
            YearOfRelease = 2004
        };

        await LibraryHttpService.PostBook(_newBook);
    }

    [Test]
    public async Task GetBooksByTitle_WhenBookExists_ReturnOk()
    {
        HttpResponseMessage response = await LibraryHttpService.GetBooksByTitle(_newBook.Title);

        var jsonString = await response.Content.ReadAsStringAsync();

        var books = JsonConvert.DeserializeObject<List<Book>>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(books.Count, Is.GreaterThan(0));
            Assert.That(books[0].Title, Is.EqualTo(_newBook.Title));
            Assert.That(books[0].Author, Is.EqualTo("Joan Rowling"));
            Assert.That(books[0].YearOfRelease, Is.EqualTo(2004));
        });
    }

    [Test]
    public async Task GetBooksByAuthor_WhenBookExists_ReturnOk()
    {
        HttpResponseMessage response = await LibraryHttpService.GetBooksByAuthor("Joan Rowling");

        var jsonString = await response.Content.ReadAsStringAsync();

        var books = JsonConvert.DeserializeObject<List<Book>>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(books.Count, Is.GreaterThan(0));
            Assert.That(books[0].Title, Is.EqualTo(_newBook.Title));
            Assert.That(books[0].Author, Is.EqualTo("Joan Rowling"));
            Assert.That(books[0].YearOfRelease, Is.EqualTo(2004));
        });
    }

    [OneTimeTearDown]
    public new async Task OneTimeTearDown()
    {
        await LibraryHttpService.DeleteBook(_newBook.Title, _newBook.Author);
    }

}