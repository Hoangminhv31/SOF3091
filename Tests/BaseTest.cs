using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SauceDemo.Tests.Tests;

public abstract class BaseTest
{
    protected IWebDriver Driver = null!;

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--headless=new", "--no-sandbox",
                             "--disable-dev-shm-usage", "--window-size=1920,1080");
        Driver = new ChromeDriver(options);
    }

    [TearDown]
    public void Teardown()
    {
        Driver.Quit();
        Driver.Dispose();
    }
}