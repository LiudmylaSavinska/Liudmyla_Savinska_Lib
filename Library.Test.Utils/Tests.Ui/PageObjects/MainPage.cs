using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public class MainPage: IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/";
    public string ExpectedTitle { get; } = "ToolsQA";

    public ILocator Elements => Page!.GetByText("Elements");

    public async Task OpenAsync()
    {
        await Page!.GotoAsync(Url);
    }
}