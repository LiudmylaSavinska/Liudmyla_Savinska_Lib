using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public class BrokenLinksAndImagesPage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/broken";
    public string ExpectedTitle { get; } = "Broken Links - Images";
    
    public ILocator Title => Page!.Locator("xpath=//h1[text()='Broken Links - Images']");
    public ILocator BrokenImage => Page!
        .Locator("xpath=//div[@id='app']/div[@class='body-height']/div[@class='container playgound-body']//img[@src='/images/Toolsqa_1.jpg']");
    public ILocator ValidLink => Page!.Locator("[href='http\\:\\/\\/demoqa\\.com']");
    public ILocator BrokenLink => Page!.Locator("[href='http\\:\\/\\/the-internet\\.herokuapp\\.com\\/status_codes\\/500']");
    
    public async Task<BrokenLinksAndImagesPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }
    
    public async Task<BrokenLinksAndImagesPage> ClosePage()
    {
        await Page.CloseAsync();
        return this;
    }
}