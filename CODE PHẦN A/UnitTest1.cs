using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace FirstTests
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test1_DynamicLoading_WithoutWait_ShouldFail()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");

            IWebElement startButton = driver.FindElement(By.CssSelector("#start button"));
            startButton.Click();

            // Không dùng Wait -> Lỗi NoSuchElementException ngay lập tức
            IWebElement finishText = driver.FindElement(By.CssSelector("#finish"));

            Assert.That(finishText.Text, Is.EqualTo("Hello World!"));
        }

        [Test]
        public void Test2_DynamicLoading_WithExplicitWait_ShouldPass()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");

            IWebElement startButton = driver.FindElement(By.CssSelector("#start button"));
            startButton.Click();

            // Dùng Explicit Wait chờ phần tử xuất hiện -> Pass
            IWebElement finishText = wait.Until(d => d.FindElement(By.CssSelector("#finish")));

            Assert.That(finishText.Text, Is.EqualTo("Hello World!"));
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
