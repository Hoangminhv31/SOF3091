using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class CartPage : BasePage
{
    private static readonly By _checkoutBtn = By.Id("checkout");

    public CartPage(IWebDriver driver) : base(driver) { }

    public CheckoutPage Checkout()
    {
        WaitForVisible(_checkoutBtn);
        Click(_checkoutBtn);
        WaitForUrlContains("/checkout-step-one.html");
        return new CheckoutPage(Driver);
    }
}