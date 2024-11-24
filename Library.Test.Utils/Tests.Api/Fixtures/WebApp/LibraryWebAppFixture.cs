using Library.Database;
using Library.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Library.Test.Utils.Tests.Api.Fixtures.WebApp;

public class LibraryWebAppFixture : WebApplicationFactory<IApiMarker>
{
    private readonly string _connectionString; 

    public LibraryWebAppFixture(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IMongoDbConnectionFactory));
            services.AddSingleton<IMongoDbConnectionFactory>(_ => new MongoDbFactory(GetMongoDbOptions()));
        });
    }
    
    private IOptions<MongoDbOptions> GetMongoDbOptions()
    {
        return new OptionsWrapper<MongoDbOptions>(new MongoDbOptions
        {
            ConnectionString = _connectionString,
            DbName = "LibraryV4"
        });
    }
}