using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;
using NUnit.Framework;

namespace Library.Test.Ui.Tests;

[TestFixture]

public class TextBoxPageTests
{
    private IBrowser Browser { get; set; }
    private IBrowserContext Context { get; set; }
    private IPage Page { get; set; }

    [OneTimeSetUp]
    public async Task Setup()
    {
        var playwright = await CreateAsync();
        playwright.Selectors.SetTestIdAttribute("aria-label");
        
        Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 50,
            Timeout = 10000,
            Args = ["--start-maximized"],
            
        });
        
        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = ViewportSize.NoViewport,
            Locale = "en-US",
            ColorScheme = ColorScheme.NoPreference,
        });

        Page = await Context.NewPageAsync();
        Page.SetDefaultTimeout(10000);
        await Page.GotoAsync("https://demoqa.com/text-box");
    }
    
    [Test]
    public async Task GoToTextBoxPage_TitleIsCorrect()
    {
        var title = await Page.TextContentAsync("h1");

        Assert.That(title, Is.EqualTo("Text Box"));
    }
    
    [Test]
    public async Task CheckCorrectLabelsAreDisplayed()
    {
        var textBoxLabelName = await Page.TextContentAsync("#userName-label");
        var textBoxLabelEmail = await Page.TextContentAsync("#userEmail-label");
        var textBoxLabelCurrentAddress = await Page.TextContentAsync("#currentAddress-label");
        var textBoxLabelPermanentAddress = await Page.TextContentAsync("#permanentAddress-label");

        Assert.Multiple(() =>
        {
            Assert.That(textBoxLabelName, Is.EqualTo("Full Name"));
            Assert.That(textBoxLabelEmail, Is.EqualTo("Email"));
            Assert.That(textBoxLabelCurrentAddress, Is.EqualTo("Current Address"));
            Assert.That(textBoxLabelPermanentAddress, Is.EqualTo("Permanent Address"));
        });
    }
    
    [Test]
    public async Task CompleteTheForm_EnteredDataIsDisplayed()
    {
       //id selector
       await Page.Locator("id=userName").FillAsync("Liudmyla Savinska");
       //css selector
       await Page.Locator("#userEmail").FillAsync("liudatest@test.com");
       await Page.FillAsync("#currentAddress", "200 Crescent Ave, Covington, KY 41011, United States");
       //xpath selector
       await Page.FillAsync("xpath=//*[@id='permanentAddress']", "6834 Hollywood Blvd\\nLos Angeles, California 90028-6116");
       
       await Page.GetByText("Submit").ClickAsync();

        var textToAssert = await Page.Locator("xpath=//div[@id='output']/div[@class='border col-md-12 col-sm-12']")
            .TextContentAsync();


        Assert.Multiple(() =>
        {
            Assert.That(textToAssert, Is.Not.Null);
            Assert.That(textToAssert, Is.Not.Empty);
            Assert.That(textToAssert!.Contains("Liudmyla Savinska"));
            Assert.That(textToAssert.Contains("liudatest@test.com"));
            Assert.That(textToAssert.Contains("200 Crescent Ave, Covington, KY 41011, United States"));
            Assert.That(textToAssert.Contains("6834 Hollywood Blvd\\nLos Angeles, California 90028-6116"));
        });
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await Page.CloseAsync();
        await Browser.CloseAsync();
    }
    
    private async Task TakeScreenShot()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var currentDate = DateTime.Now.ToString("dd-MM-yy");
        var currentTime = DateTime.Now.ToString("HH-mm-ss");
        var currentTestFixture = TestContext.CurrentContext.Test.ClassName;
        var screenShotName = $"{TestContext.CurrentContext.Test.Name}.png";
        var path = Path.Combine(currentDirectory, currentDate, currentTime, currentTestFixture, screenShotName);
        await Page.ScreenshotAsync(new PageScreenshotOptions { Path = path });
    }
}