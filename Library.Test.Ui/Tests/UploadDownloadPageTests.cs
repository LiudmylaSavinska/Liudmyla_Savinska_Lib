using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework;

namespace Library.Test.Ui.Tests;


[TestFixture]
public class UploadDownloadPageTests
{
    private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private UploadDownloadPage Page { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Page = await _browserSetUpBuilder
            .WithBrowser(BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(false)
            .WithTimeout(10000)
            .WithArgs("--start-maximized")
            .OpenNewPage<UploadDownloadPage>();
        _browserSetUpBuilder.AddRequestResponseLogger();
        await Page.Open();
    }
    
    [Test]
    public async Task GoToUploadDownloadPage_TitleIsCorrect()
    {
        var title = await Page.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo("Upload and Download"));
    }

    [Test]
    public async Task DownloadFile_DownloadIsCorrect()
    {
        var download = await Page.FileDownload();
        
        Assert.That(download, Is.Not.Null);
    }

    [Test]

    public async Task UploadFile_UploadIsCorrect()
    {
        var upload = await Page.FileUpload();
        
        Assert.That(upload, Is.Not.Null);
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _browserSetUpBuilder.Context!.CloseAsync();
        await Page.ClosePage();
    }
}