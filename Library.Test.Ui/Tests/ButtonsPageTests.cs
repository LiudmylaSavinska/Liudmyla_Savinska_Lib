using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework;

namespace Library.Test.Ui.Tests;

[TestFixture]

public class ButtonsPageTests
{
    private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private ButtonsPage Page { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Page = await _browserSetUpBuilder
            .WithBrowser(BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(true)
            .WithTimeout(10000)
            .WithArgs("--start-maximized")
            .OpenNewPage<ButtonsPage>();
        _browserSetUpBuilder.AddRequestResponseLogger();
        await Page.Open();
    }
    
    [Test]
    public async Task GoToButtonsPage_TitleIsCorrect()
    {
        var title = await Page.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo("Buttons"));
    }
    
    [Test]
    public async Task DoubleClickButton_ReturnsMessage()
    {
        await Page.DoubleClick();
        var doubleClickMessage = await Page.DoubleClickMessage.TextContentAsync();
        
        Assert.That(doubleClickMessage, Is.EqualTo("You have done a double click"));
    }
    
    [Test]
    public async Task RightClickButton_ReturnsMessage()
    {
        await Page.ClickOnRightClickButton();
        var rightClickMessage = await Page.RightClickMessage.TextContentAsync();
        
        Assert.That(rightClickMessage, Is.EqualTo("You have done a right click"));
    }
    
    [Test]
    public async Task ClickButton_ReturnsMessage()
    {
        await Page.ClickOnClickMe();
        var clickMessage = await Page.ClickMeMessage.TextContentAsync();
        
        Assert.That(clickMessage, Is.EqualTo("You have done a dynamic click"));
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _browserSetUpBuilder.Context!.CloseAsync();
        await Page.ClosePage();
    }
}