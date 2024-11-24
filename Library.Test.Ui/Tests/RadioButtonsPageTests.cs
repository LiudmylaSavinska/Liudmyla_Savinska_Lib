using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Library.Test.Ui.Tests;

public class RadioButtonsPageTests
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
        await Page.GotoAsync("https://demoqa.com/radio-button");
    }
    
    [Test]
    public async Task OpenRadioButtonPage_TitleIsCorrect()
    {
        var title = await Page.TextContentAsync("h1");

        Assert.That(title, Is.EqualTo("Radio Button"));
    }
    
    [Test]
    public async Task ClickYesRadioButton_DisplayMessage()
    {
        await Page.ClickAsync("[for='yesRadio']");
        var doubleClickMessage = await Page.Locator("[class='mt-3']")
            .TextContentAsync();
        
        Assert.That(doubleClickMessage, Is.EqualTo("You have selected Yes"));
    }
    
    [Test]
    public async Task ClickImpressiveRadioButton_DisplayMessage()
    {
        await Page.ClickAsync("[for='impressiveRadio']");
        var doubleClickMessage = await Page.Locator("[class='mt-3']")
            .TextContentAsync();
        
        Assert.That(doubleClickMessage, Is.EqualTo("You have selected Impressive"));
    }
    
    [Test]
    public async Task CheckNoRadioButtonIsEnabled_ReturnFalse()
    {
        var isEnabledButton = await Page.Locator("[for='noRadio']").IsEnabledAsync();

        Assert.That(isEnabledButton, Is.False);
    }
    
    [TearDown]
    public async Task TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            await TakeScreenShot();
        }
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