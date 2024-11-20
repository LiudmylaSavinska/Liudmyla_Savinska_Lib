using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public interface IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; }
    public string ExpectedTitle { get; }
}
