using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public class TextBoxPage: IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/text-box";
    public string ExpectedTitle { get; } = "Text Box";
    
    public ILocator Title => Page!.Locator("xpath=//h1[text()='Text Box']");
    
    public async Task<TextBoxPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }
    
    public async Task<TextBoxPage> ClosePage()
    {
        await Page.CloseAsync();
        return this;
    }
}
