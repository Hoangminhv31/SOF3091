using NUnit.Framework;
using SauceDemo.Tests.Pages;

namespace SauceDemo.Tests.Tests;

public class LoginTests : BaseTest
{
    [TestCaseSource(typeof(LoginTestData), nameof(LoginTestData.GetCases))]
    public void Login_DataDriven(string username, string password,
        bool shouldLogin, string expectedError)
    {
        var loginPage = new LoginPage(Driver).Open(Settings.Url);

        if (shouldLogin)
        {
            var inventory = loginPage.LoginAs(username, password);
            Assert.That(inventory.IsLoaded(), Is.True);
        }
        else
        {
            var error = loginPage.LoginExpectingError(username, password).GetErrorMessage();
            Assert.That(error, Does.Contain(expectedError));
        }
    }
}
