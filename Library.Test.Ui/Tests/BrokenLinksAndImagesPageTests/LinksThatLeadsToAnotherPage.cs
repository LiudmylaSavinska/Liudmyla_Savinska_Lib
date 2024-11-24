using Library.Test.Utils.Tests.Ui.Fixtures;
using NUnit.Framework;

namespace Library.Test.Ui.Tests.BrokenLinksAndImagesPageTests;

[TestFixture]

public class LinksThatLeadsToAnotherPage
{
    private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private Test.Utils.Tests.Ui.PageObjects.LinksPage Page { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Page = await _browserSetUpBuilder
            .WithBrowser(BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(true)
            .WithTimeout(10000)
            .WithSlowMo(100)
            .WithArgs("--start-maximized")
            .OpenNewPage<Test.Utils.Tests.Ui.PageObjects.LinksPage>();
        _browserSetUpBuilder.AddRequestResponseLogger();
        await Page.Open();
    }

    [Test]
    public async Task GoToLinksPage_TitleIsCorrect()
    {
        var title = await Page.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo("Links"));
    }

    [Test]
    public async Task ClickOnHomeLink_ReturnsCorrectTab()
    {
        await Page.HomeLink.ClickAsync();
        var expectedPageUrl = Page.Page!.Url;

        Assert.That(expectedPageUrl, Is.EqualTo(Page.Url));
    }


    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _browserSetUpBuilder.Context!.CloseAsync();
        await Page.ClosePage();
    }
}