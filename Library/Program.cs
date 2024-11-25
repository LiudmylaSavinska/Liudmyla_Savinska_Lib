using Library.Database;
using Library.Endpoints.Books;
using Library.Endpoints.User;
using Library.Options;
using Library.Repositories;
using Library.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", false)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDbOptions"));
builder.Services.AddSingleton<IMongoDbConnectionFactory, MongoDbFactory>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IAuthorizationTokenRepository, AuthorizationTokenRepository>();
builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IUserAuthorizationService, UserAuthorizationService>();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapCreateUser();
app.MapLogIn();
app.MapCreateBook();
app.MapGetBooksByTitle();
app.MapGetBooksByAuthor();
app.MapDeleteBook();
app.Run();