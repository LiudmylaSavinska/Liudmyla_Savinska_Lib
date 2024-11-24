using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework;

namespace Library.Test.Ui.Tests.BrokenLinksAndImagesPageTests;

[TestFixture]

public class RequestsTests
{
    private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private LinksPage LinksPage { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        LinksPage = await _browserSetUpBuilder
            .WithBrowser(BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(true)
            .WithTimeout(10000)
            .WithSlowMo(500)
            .WithArgs("--start-maximized")
            .OpenNewPage<LinksPage>();
        _browserSetUpBuilder.AddRequestResponseLogger();
        await LinksPage.Open();
    }

    [Test]
    public async Task ClickOnCreatedLink_ReturnsCorrectResponse()
    {
        //TODO: Example of how to check response status of request that triggered by click
        //With wrapped method from page object
        await LinksPage.CLickAndWaiteForRequest(async () =>
            {
                await LinksPage.CreatedLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/created") && r.Status == 201);

        //With default Playwright method
        await LinksPage.Page!.RunAndWaitForResponseAsync(async () =>
            {
                await LinksPage.CreatedLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/created") && r.Status == 201);
        var responseMessage = await LinksPage.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage,
            Is.EqualTo("Link has responded with staus 201 and status text Created"));
    }

    [Test]
    public async Task ClickOnNoContentLink_ReturnsCorrectResponse()
    {
        await LinksPage.CLickAndWaiteForRequest(async () =>
            {
                await LinksPage.NoContentLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/no-content") && r.Status == 204);

        var responseMessage = await LinksPage.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage,
            Is.EqualTo("Link has responded with staus 204 and status text No Content"));
    }

    [Test]
    public async Task MovedLink_ReturnsCorrectResponse()
    {
        await LinksPage.CLickAndWaiteForRequest(async () =>
            {
                await LinksPage.MovedLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/moved") && r.Status == 301);

        var responseMessage = await LinksPage.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage,
            Is.EqualTo("Link has responded with staus 301 and status text Moved Permanently"));
    }

    [Test]
    public async Task BadRequestLink_ReturnsCorrectResponse()
    {
        await LinksPage.CLickAndWaiteForRequest(async () =>
            {
                await LinksPage.BadRequestLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/bad-request") && r.Status == 400);
       
        var responseMessage = await LinksPage.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage,
            Is.EqualTo("Link has responded with staus 400 and status text Bad Request"));
    }

    [Test]
    public async Task UnauthorizedLink_ReturnsCorrectResponse()
    {
        await LinksPage.CLickAndWaiteForRequest(async () =>
            {
                await LinksPage.UnauthorizedLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/unauthorized") && r.Status == 401);
        
        var responseMessage = await LinksPage.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage,Is.EqualTo("Link has responded with staus 401 and status text Unauthorized"));
    }

    [Test]
    public async Task ForbiddenLink_ReturnsCorrectResponse()
    {
        await LinksPage.CLickAndWaiteForRequest(async () =>
            {
                await LinksPage.ForbiddenLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/forbidden") && r.Status == 403);
       
        var responseMessage = await LinksPage.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage,
            Is.EqualTo("Link has responded with staus 403 and status text Forbidden"));
    }

    [Test]
    public async Task NotFoundLink_ReturnsCorrectResponse()
    {
        await LinksPage.CLickAndWaiteForRequest(async () =>
            {
                await LinksPage.NotFoundLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/invalid-url") && r.Status == 404);
        
        var responseMessage = await LinksPage.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage,
            Is.EqualTo("Link has responded with staus 404 and status text Not Found"));
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _browserSetUpBuilder.Context!.CloseAsync();
        await LinksPage.ClosePage();
    }
}