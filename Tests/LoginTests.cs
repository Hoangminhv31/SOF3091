using NUnit.Framework;
using SauceDemo.Tests.Pages;

namespace SauceDemo.Tests.Tests;

public class LoginTests : BaseTest
{
    [Test]
    public void Login_ValidUser_OpensInventory()
    {
        var inventory = new LoginPage(Driver).Open()
            .LoginAs("standard_user", "secret_sauce");

        Assert.That(inventory.IsLoaded(), Is.True);
    }

    [Test]
    public void Login_LockedUser_ShowsError()
    {
        var error = new LoginPage(Driver).Open()
            .LoginExpectingError("locked_out_user", "secret_sauce")
            .GetErrorMessage();

        Assert.That(error, Does.Contain("locked out"));
    }
}