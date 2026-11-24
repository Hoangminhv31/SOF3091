using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SauceDemo.Tests.Config;

namespace SauceDemo.Tests.Tests;

public abstract class BaseTest
{
    protected IWebDriver Driver = null!;
    protected TestSettings Settings = null!;

    [SetUp]
    public void Setup()
    {
        Settings = TestSettings.Load();
        var headless = Environment.GetEnvironmentVariable("HEADLESS");
        if (!string.IsNullOrWhiteSpace(headless))
            Settings.Headless = headless.Equals("true", StringComparison.OrdinalIgnoreCase);

        if (Settings.Browser.Equals("Firefox", StringComparison.OrdinalIgnoreCase))
        {
            var options = new FirefoxOptions();
            if (Settings.Headless) options.AddArgument("-headless");
            Driver = new FirefoxDriver(options);
        }
        else
        {
            var options = new ChromeOptions();
            if (Settings.Headless) options.AddArgument("--headless=new");
            options.AddArguments("--no-sandbox", "--disable-dev-shm-usage",
                "--window-size=1920,1080");
            Driver = new ChromeDriver(options);
        }

        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
        Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(Settings.Timeout);
    }

    [TearDown]
    public void Teardown()
    {
        if (Driver != null)
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
