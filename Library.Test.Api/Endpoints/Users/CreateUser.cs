using System.Net;
using Library.Contracts.Domain;
using Library.Contracts.Mappings;
using Library.Test.Api.TestFixtures;
using Library.Test.Utils.Tests.Api.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Library.Test.Api.Endpoints.Users;

[TestFixture]

public class CreateUser : GlobalSetUp
{
    [Test]
    public async Task Post_NewUser_ReturnsCreated()
    {
        // Arrange
        var userToCreate = DataHelper.CreateUser();
        var response = await LibraryHttpService.CreateUser(userToCreate);

        // Act
        var jsonSting = await response.Content.ReadAsStringAsync();
        var user = JsonConvert.DeserializeObject<User>(jsonSting);
        var userDto = await MongoDbFixture.Users.GetItem(u => u.NickName == user.NickName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(user.NickName, Is.EqualTo(userToCreate.NickName));
            Assert.That(user.FullName, Is.EqualTo(userToCreate.FullName));
            Assert.That(userDto, Is.Not.Null);
            Assert.That(userDto.NickName, Is.EqualTo(userToCreate.NickName));
        });
    }

    [Test]
    public async Task Post_ExistingUser_ReturnsBadRequest()
    {
        // Arrange
        var userDto = DataHelper.CreateUserDto();
        await MongoDbFixture.Users.InsertItem(userDto);
        
        // Act
        var response = await LibraryHttpService.CreateUser(userDto.ToDomain());
        var jsonSting = await response.Content.ReadAsStringAsync();
        var stringToAssert = jsonSting.Trim('"');
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(stringToAssert, Is.EqualTo($"User with nickname {userDto.ToDomain().NickName} already exists"));
        });
    }
}