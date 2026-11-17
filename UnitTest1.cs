using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace FirstTests
{
    public class SauceDemoTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test01_OpenSauceDemoAndCheckTitle()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            Assert.That(_driver.Title, Does.Contain("Swag Labs"), "Tiêu d? trang không dúng!");
        }

        [Test]
        public void Test02_LoginAndAddToCart()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com/");

            // Ðang nh?p
            _wait.Until(d => d.FindElement(By.Id("user-name"))).SendKeys("standard_user");
            _driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            _driver.FindElement(By.Id("login-button")).Click();

            // Ch? chuy?n trang s?n ph?m
            _wait.Until(d => d.Url.Contains("/inventory.html"));

            // Thêm s?n ph?m vào gi? hàng & m? gi? hàng
            _wait.Until(d => d.FindElement(By.Id("add-to-cart-sauce-labs-backpack"))).Click();
            _driver.FindElement(By.ClassName("shopping_cart_link")).Click();

            // Xác nh?n s?n ph?m trong gi?
            _wait.Until(d => d.Url.Contains("/cart.html"));
            IWebElement cartItem = _wait.Until(d => d.FindElement(By.ClassName("inventory_item_name")));
            Assert.That(cartItem.Text, Is.EqualTo("Sauce Labs Backpack"));
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
