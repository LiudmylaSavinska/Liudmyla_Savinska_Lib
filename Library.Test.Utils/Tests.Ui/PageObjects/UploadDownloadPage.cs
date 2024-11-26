using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public class UploadDownloadPage: IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/upload-download";
    public string ExpectedTitle { get; } = "Upload and Download";
    
    public ILocator Title => Page!.Locator("xpath=//h1[text()='Upload and Download']");
    public ILocator DownloadButton => Page!.Locator("xpath=/html//a[@id='downloadButton']");
    public ILocator ChoosefileButton => Page!.Locator("xpath=/html//input[@id='uploadFile']");
    public ILocator FilePath => Page!.Locator("//p[@id='uploadedFilePath']");
    
    public async Task<UploadDownloadPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }

    public async Task<UploadDownloadPage> FileUpload()
    {
        await ChoosefileButton.ClickAsync();
        
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Tests.UI", "AppData", "NewFile1.txt");
        await Page!.SetInputFilesAsync("input[type='file']", filePath);

        return this;
    }
    
    public async Task<UploadDownloadPage> FileDownload()
    {
        var downloadTask = Page!.RunAndWaitForDownloadAsync(async () =>
        {
            await DownloadButton.ClickAsync();
        });
        
        var download = await downloadTask;

        var downloadPath = await download.PathAsync();
        
        return this;
    }
    public async Task<UploadDownloadPage> ClosePage()
    {
        await Page!.CloseAsync();
        return this;
    }

}