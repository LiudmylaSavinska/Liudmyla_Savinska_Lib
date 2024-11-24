using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework;

namespace Library.Test.Ui.Tests;

[TestFixture]

public class DynamicPropertiesPageTests
{
    private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private DynamicPropertiesPage Page { get; set; }
    

    [SetUp]
    public async Task OneTimeSetUp()
    {
        Page = await _browserSetUpBuilder
            .WithBrowser(BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(true)
            .WithTimeout(10000)
            .WithSlowMo(100)
            .WithArgs("--start-maximized")
            .OpenNewPage<DynamicPropertiesPage>();
        _browserSetUpBuilder.AddRequestResponseLogger();
        await Page.Open();
    }
    
    [Test]
    public async Task GoToDynamicPropertiesPage_TitleIsCorrect()
    {
        var title = await Page.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo("Dynamic Properties"));
    }
    
    [Test]
    public async Task CheckTheButtonIsEnabledAfterDelay_ReturnsTrue()
    {
        await Page.ClickEnabledAfterButton();
        await Task.Delay(5000);
        await Page.EnableAfterButton.ClickAsync();
        bool isEnabled = await Page.EnableAfterButton.IsEnabledAsync();
        
        Assert.That(isEnabled, Is.True);
    }
    
    [Test]
    public async Task CheckColorChangeButtonAfterDelay_ReturnsTrue()
    {
        var initialButtonColor = await Page.ColorChangeButton
                .EvaluateAsync<string>("element => getComputedStyle(element).color");
        
        Assert.That(initialButtonColor, 
            Is.EqualTo("rgb(255, 255, 255)"), "Initial color should be rgb(255, 255, 255).");
        
        await Task.Delay(6000);
        
        var newButtonColor = await Page.ColorChangeButton
            .EvaluateAsync<string>("element => getComputedStyle(element).color");
        
        Assert.That(newButtonColor, 
            Is.EqualTo("rgb(220, 53, 69)"), "Initial color should be rgb(220, 53, 69).");
    }
    
    [Test]
    public async Task CheckTheButtonIsVisibleBeforeDelay_ReturnsFalse()
    {
        bool isVisible = await Page.VisibleAfterButton.IsVisibleAsync();
        
        Assert.That(isVisible, Is.False);
    }
    
    [Test]
    public async Task CheckTheButtonIsVisibleAfterDelay_ReturnsTrue()
    {
        await Task.Delay(5000);
        bool isVisible = await Page.VisibleAfterButton.IsVisibleAsync();
        
        Assert.That(isVisible, Is.True);
    }
    
    [TearDown]
    public async Task OneTimeTearDown()
    {
        await _browserSetUpBuilder.Context!.CloseAsync();
        await Page.ClosePage();
    }
}