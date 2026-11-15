using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class LoginPage : BasePage
{
    private static readonly By _username = By.Id("user-name");
    private static readonly By _password = By.Id("password");
    private static readonly By _loginBtn = By.Id("login-button");
    private static readonly By _error = By.CssSelector("[data-test='error']");

    public LoginPage(IWebDriver driver) : base(driver) { }

    public LoginPage Open()
    {
        Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        return this;
    }

    public InventoryPage LoginAs(string user, string pass)
    {
        Type(_username, user);
        Type(_password, pass);
        Click(_loginBtn);
        return new InventoryPage(Driver);
    }

    public LoginPage LoginExpectingError(string user, string pass)
    {
        Type(_username, user);
        Type(_password, pass);
        Click(_loginBtn);
        return this;
    }

    public string GetErrorMessage() => GetText(_error);
}