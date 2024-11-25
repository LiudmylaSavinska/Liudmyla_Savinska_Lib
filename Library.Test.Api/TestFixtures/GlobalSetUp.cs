using Library.Test.Utils.Tests.Api.Fixtures.Database;
using Library.Test.Utils.Tests.Api.Fixtures.Docker;
using Library.Test.Utils.Tests.Api.Fixtures.Http;
using Library.Test.Utils.Tests.Api.Fixtures.WebApp;
using NUnit.Framework;

namespace Library.Test.Api.TestFixtures;

[SetUpFixture]
public class GlobalSetUp
{
    private readonly DockerFixture _dockerFixture = new();
    private LibraryWebAppFixture _libraryWebAppFixture;
    protected MongoDbFixture MongoDbFixture;
    protected LibraryHttpService LibraryHttpService;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        //Starting MongoDb in Docker
        await _dockerFixture.StartMongo();
        var connectionString = _dockerFixture.GetMongoDbConnectionString();

        //Creating an instance of LibraryV4 Service 
        _libraryWebAppFixture = new LibraryWebAppFixture(connectionString);

        //Creating an instance of MongoDbFixture
        MongoDbFixture = new MongoDbFixture(connectionString, "LibraryV4");

        //Creating an HttpClient instance
        var httpClient = _libraryWebAppFixture.CreateClient();
        LibraryHttpService = new LibraryHttpService(httpClient);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _dockerFixture.StopMongo();
        _libraryWebAppFixture?.Dispose();
    }
}