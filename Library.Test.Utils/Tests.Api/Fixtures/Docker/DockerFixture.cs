using DotNet.Testcontainers.Builders;
using Testcontainers.MongoDb;

namespace Library.Test.Utils.Tests.Api.Fixtures.Docker;

public sealed class DockerFixture
{
    private readonly MongoDbContainer MongoDbContainer =
        new MongoDbBuilder()
            .WithPortBinding(27017, 27017)
            .WithName("LibraryV4")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017))
            .Build();

    public string GetMongoDbConnectionString() => MongoDbContainer.GetConnectionString();

    public async Task StartMongo()
    {
        await MongoDbContainer.StartAsync();
    }

    public async Task StopMongo()
    {
        await MongoDbContainer.StopAsync();
        await MongoDbContainer.DisposeAsync();
    }
}