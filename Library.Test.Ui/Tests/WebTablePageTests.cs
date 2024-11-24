using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using BrowserType = Library.Test.Utils.Tests.Ui.Fixtures.BrowserType;

namespace Library.Test.Ui.Tests;

[TestFixture]

public class WebTablePageTests
{
     private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private WebTablePage Page { get; set; }
    private readonly string _date = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Page = await _browserSetUpBuilder
            .WithBrowser(BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(false)
            .WithSlowMo(100)
            .WithTimeout(10000)
            .WithVideoSize(1900, 1080)
            .SaveVideo($"{_date}/videos/")
            .WithArgs("--start-maximized")
            .OpenNewPage<WebTablePage>();
        _browserSetUpBuilder.AddRequestResponseLogger();
        await Page.Open();
    }

    [SetUp]
    public async Task SetUp()
    {
        await _browserSetUpBuilder.Context!.Tracing.StartAsync(new TracingStartOptions()
        {
            Title = TestContext.CurrentContext.Test.ClassName + "." + TestContext.CurrentContext.Test.Name,
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }
    
    [Test]
    public async Task GoToWebTablePage_TitleIsCorrect()
    {
        var title = await Page.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo("Web Tables"));
    }

    [Test]
    public async Task ClickAddButton_ReturnRegistrationForm()
    {
        await Page.ClickAddButton();
        
        var registrationForm = Page.RegistrationForm;
        await registrationForm.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
       
        bool registrationFormIsDisplayed = await registrationForm.IsVisibleAsync();
        
        Assert.That(registrationFormIsDisplayed, Is.True);
    }

    [Test]
    public async Task FillInRegistrationForm_ReturnSavedData()
    {
        await Page.EnterFirstName("Liuda");
        await Page.EnterLastName("Test");
        await Page.EnterEmail("test@test.com");
        await Page.EnterAge("25");
        await Page.EnterSalary("12345");
        await Page.EnterDepartment("QA");

        await Page.ClickSubmitButton();

        var filledRowsCount = await Page!.FindRows();
        
        Assert.That(filledRowsCount?.Count, Is.EqualTo(4));
     
    }
    
    [Test]
    public async Task SortByAge()
    {
        await Page.SortByAge();
        
        var rowOne = await Page!.GetRowValues(1);

        Assert.Multiple(() =>
        {
            Assert.That(rowOne[0], Is.EqualTo("Liuda"));
            Assert.That(rowOne[2], Is.EqualTo("25"));

        });
    }

    [TearDown]
    public async Task TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            var testName = TestContext.CurrentContext.Test.MethodName;
            await _browserSetUpBuilder.Page!.ScreenshotAsync(new PageScreenshotOptions()
            {
                Path = $"{_date}/Screenshots/{testName}.png"
            });
        }
        
        await _browserSetUpBuilder.Context!.Tracing.StopAsync(new TracingStopOptions()
        {
            Path = Path.Combine(TestContext.CurrentContext.WorkDirectory, _date, "playwright-traces",
                $"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}.zip")
        });
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await Page.ClosePage();
        await _browserSetUpBuilder.Context!.CloseAsync();
    }
}