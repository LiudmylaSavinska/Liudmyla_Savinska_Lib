using System.Net;
using Library.Contracts.Domain;
using Library.Test.Api.TestFixtures;
using Library.Test.Utils.Tests.Api.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Library.Test.Api.Endpoints.Books;

[TestFixture]

public class DeleteBooks : GlobalSetUp

{
    [Test]
    [Description("This test checks if the book is deleted successfully")]
    public async Task DeleteBookAsync_ReturnOK()
    {
        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.AuthorizeLikeDefaultUser();
        
        var book = DataHelper.CreateBook();
            
        var httpResponseMessage = 
            await LibraryHttpService.PostBook(LibraryHttpService.AuthorizationToken.Token.ToString(), book);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<Book>(content);
        
        var deleteResponseMessage = 
            await LibraryHttpService
                .DeleteBook(bookFromResponse.Title, bookFromResponse.Author);
        
        Assert.That(deleteResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}