using NUnit.Framework;
using SauceDemo.Tests.Pages;

namespace SauceDemo.Tests.Tests;

public class CheckoutTests : BaseTest
{
    [Test]
    public void Checkout_HappyPath_ShowsThankYou()
    {
        var message = new LoginPage(Driver).Open(Settings.Url)
            .LoginAs("standard_user", "secret_sauce")
            .AddBackpackToCart()
            .GoToCart()
            .Checkout()
            .FillInfo("An", "Nguyen", "70000")
            .Finish()
            .GetCompleteMessage();

        Assert.That(message, Is.EqualTo("Thank you for your order!"));
    }
}