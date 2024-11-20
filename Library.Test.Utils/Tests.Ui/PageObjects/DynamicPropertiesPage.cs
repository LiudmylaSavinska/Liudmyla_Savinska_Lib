using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public class DynamicPropertiesPage : IBasePage
{
public IPage? Page { get; set; }
public string Url { get; } = "https://demoqa.com/dynamic-properties";
public string ExpectedTitle { get; } = "Dynamic Properties";
    
public ILocator Title => Page!.Locator("xpath=//h1[text()='Dynamic Properties']");
public ILocator EnableAfterButton => Page!.Locator("xpath=/html//button[@id='enableAfter']");
public ILocator ColorChangeButton => Page!.Locator("xpath=/html//button[@id='colorChange']");
public ILocator VisibleAfterButton => Page!.Locator("xpath=/html//button[@id='visibleAfter']");
    
public async Task<DynamicPropertiesPage> Open()
{
    await Page!.GotoAsync(Url);
    return this;
}

public async Task<bool> ClickEnabledAfterButton()
{
    await EnableAfterButton.IsEnabledAsync();
    return false;
}
    
public async Task<DynamicPropertiesPage> ClosePage()
{
    await Page!.CloseAsync();
    return this;
}
}