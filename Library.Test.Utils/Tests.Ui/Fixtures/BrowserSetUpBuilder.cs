using Library.Test.Utils.Tests.Ui.PageObjects;
using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.Fixtures;

public class BrowserSetUpBuilder
{
     private BrowserType Type { get; set; } = BrowserType.Chromium;
    private readonly string _date = $"{DateTime.Now:MM-dd-yy}";
    private readonly string _time = $"{DateTime.Now:HH-mm-ss}";
    private readonly BrowserTypeLaunchOptions _browserTypeLaunchOptions = new();
    private readonly BrowserNewContextOptions _browserNewContextOptions = new()
    {
        ViewportSize = ViewportSize.NoViewport,
        Locale = "en-US",
        ColorScheme = ColorScheme.NoPreference
    };

    public IBrowserContext? Context { get; private set; }
    public IPage? Page { get; private set; }


    public void AddRequestResponseLogger()
    {
        Page!.Request += (_, request) => Console.WriteLine(">> " + request.Method + " " + request.Url);
        Page!.Response += (_, response) => Console.WriteLine("<< " + response.Status + " " + response.Url);
    }

    public async Task StartTracing(string traceName)
    {
        await Context!.Tracing.StartAsync(new()
        {
            Title = traceName,
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    public async Task StopTracing(string path)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var finalPath = Path.Combine(currentDirectory, "test-results", path);
        await Context!.Tracing.StopAsync(new() { Path = finalPath });
    }

    public async Task Screenshot(string testSuiteName, string screenshotName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var finalPath = Path.Combine(currentDirectory, "test-results", testSuiteName, screenshotName + ".png");
        await Page!.ScreenshotAsync(new() { Path = finalPath });
    }

    public BrowserSetUpBuilder WithBrowser(BrowserType type)
    {
        Type = type;
        return this;
    }

    public BrowserSetUpBuilder InHeadlessMode(bool headless)
    {
        _browserTypeLaunchOptions.Headless = headless;
        return this;
    }

    public BrowserSetUpBuilder WithChannel(string channel)
    {
        _browserTypeLaunchOptions.Channel = channel;
        return this;
    }

    public BrowserSetUpBuilder WithSlowMo(int slowMo)
    {
        _browserTypeLaunchOptions.SlowMo = slowMo;
        return this;
    }

    public BrowserSetUpBuilder WithTimeout(int timeout)
    {
        _browserTypeLaunchOptions.Timeout = timeout;
        return this;
    }

    public BrowserSetUpBuilder WithArgs(params string[] args)
    {
        _browserTypeLaunchOptions.Args = args;
        return this;
    }

    public BrowserSetUpBuilder WithLocale(string locale)
    {
        _browserNewContextOptions.Locale = locale;
        return this;
    }

    public BrowserSetUpBuilder WithColorScheme(ColorScheme colorScheme)
    {
        _browserNewContextOptions.ColorScheme = colorScheme;
        return this;
    }

    public BrowserSetUpBuilder WithViewportSize(int width, int height)
    {
        var viewportSize = new ViewportSize
        {
            Width = width,
            Height = height
        };
        _browserNewContextOptions.ViewportSize = viewportSize;
        return this;
    }

    public BrowserSetUpBuilder WithVideoSize(int width, int height)
    {
        _browserNewContextOptions.RecordVideoSize = new()
        {
            Width = width, Height = height
        };
        return this;
    }

    public BrowserSetUpBuilder SaveVideo(string path)
    {
        _browserNewContextOptions.RecordVideoDir = $"test-results/" + path;
        return this;
    }

    public async Task<T> OpenNewPage<T>() where T : IBasePage, new()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = Type switch
        {
            BrowserType.Chromium => await playwright.Chromium.LaunchAsync(_browserTypeLaunchOptions),
            BrowserType.Firefox => await playwright.Firefox.LaunchAsync(_browserTypeLaunchOptions),
            BrowserType.WebKit => await playwright.Webkit.LaunchAsync(_browserTypeLaunchOptions),
            _ => throw new ArgumentOutOfRangeException()
        };
        Context = await browser.NewContextAsync(_browserNewContextOptions);
        Page = await Context.NewPageAsync();
        var pageObject = new T { Page = this.Page };
        return pageObject;
    }
}