using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using FirstTests.Pages;

namespace FirstTests.Tests
{
    public class LoginTest
    {
        private IWebDriver? driver;
        private LoginPage? loginPage;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            loginPage = new LoginPage(driver);
        }

        [Test]
        public void LoginThanhCong()
        {
            loginPage!.NhapUsername("standard_user");
            loginPage.NhapPassword("secret_sauce");
            loginPage.BamLogin();

            Assert.That(driver!.Url, Does.Contain("inventory"));
        }

        [TearDown]
        public void Cleanup()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
    }
}
