using Microsoft.Playwright;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestBase;

public class UiTestBase
{
    protected IPlaywright Playwright;
    protected IBrowser Browser;
    protected IPage Page;
    protected IAPIRequestContext API;

    [SetUp]
    public async Task Setup()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
       Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo=200
        });
        Page = await Browser.NewPageAsync();

        API = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = "http://localhost:5000",
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                { "Accept", "application/json" }
            }
        });
    }

    [TearDown]
    public async Task TearDown()
    {
        await API.DisposeAsync();
        await Browser.CloseAsync();
        Playwright.Dispose();
    }
}
