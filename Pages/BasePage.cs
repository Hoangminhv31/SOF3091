using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SauceDemo.Tests.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    private readonly WebDriverWait _wait;

    protected BasePage(IWebDriver driver, int timeoutSeconds = 10)
    {
        Driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        _wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
    }

    // Tìm phần tử + chờ hiển thị
    protected IWebElement WaitForVisible(By locator) =>
        _wait.Until(d =>
        {
            var el = d.FindElement(locator);
            return el.Displayed ? el : null;
        });

    protected void WaitForUrlContains(string value)
    {
        _wait.Until(d => d.Url.Contains(value, StringComparison.OrdinalIgnoreCase));
    }

    protected void WaitForText(By locator, string expectedText)
    {
        _wait.Until(d =>
        {
            var elements = d.FindElements(locator);
            return elements.Any(el => el.Displayed && el.Text.Trim() == expectedText);
        });
    }

    protected void Click(By locator)
    {
        var element = _wait.Until(d =>
        {
            var el = d.FindElement(locator);
            return el.Displayed && el.Enabled ? el : null;
        });
        element.Click();
    }

    protected void Type(By locator, string text)
    {
        var el = WaitForVisible(locator);
        el.Clear();
        el.SendKeys(text);
    }

    protected string GetText(By locator) => WaitForVisible(locator).Text;

    protected bool IsDisplayed(By locator)
    {
        try { return WaitForVisible(locator).Displayed; }
        catch (WebDriverTimeoutException) { return false; }
    }
}