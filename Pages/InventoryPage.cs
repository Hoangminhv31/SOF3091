using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class InventoryPage : BasePage
{
    private static readonly By _title = By.ClassName("title");
    private static readonly By _addBackpack = By.Id("add-to-cart-sauce-labs-backpack");
    private static readonly By _cartBadge = By.ClassName("shopping_cart_badge");
    private static readonly By _cartLink = By.ClassName("shopping_cart_link");

    public InventoryPage(IWebDriver driver) : base(driver) { }

    public bool IsLoaded() => IsDisplayed(_title);

    public InventoryPage AddBackpackToCart()
    {
        Click(_addBackpack);
        WaitForText(_cartBadge, "1");
        return this;
    }

    public string GetCartCount() => GetText(_cartBadge);

    public CartPage GoToCart()
    {
        Click(_cartLink);
        WaitForUrlContains("/cart.html");
        return new CartPage(Driver);
    }
}