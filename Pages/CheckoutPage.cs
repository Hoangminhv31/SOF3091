using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class CheckoutPage : BasePage
{
    private readonly By _firstName = By.Id("first-name");
    private static readonly By _lastName = By.Id("last-name");
    private static readonly By _zip = By.Id("postal-code");
    private static readonly By _continue = By.Id("continue");
    private static readonly By _finish = By.Id("finish");
    private static readonly By _complete = By.ClassName("complete-header");

    public CheckoutPage(IWebDriver driver) : base(driver) { }

    public CheckoutPage FillInfo(string first, string last, string zip)
    {
        Type(_firstName, first);
        Type(_lastName, last);
        Type(_zip, zip);
        Click(_continue);
        return this;
    }

    public CheckoutPage Finish()
    {
        Click(_finish);
        return this;
    }

    public string GetCompleteMessage() => GetText(_complete);
}