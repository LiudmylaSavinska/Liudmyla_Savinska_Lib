using Library.Test.Api.TestFixtures;
using NUnit.Framework;

namespace Library.Test.Api.Endpoints.Users;

public class LoginUser : GlobalSetUp
{
    [Test]
    public async Task LogInAsync_ReturnOK()
    {
        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.AuthorizeLikeDefaultUser();
        
        // Assert
        Assert.That(LibraryHttpService.AuthorizationToken, Is.Not.Null);
    }
}