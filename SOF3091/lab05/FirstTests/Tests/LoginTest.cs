
using System;
using System.Drawing;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using FirstTests.Config;
using FirstTests.Pages;

namespace FirstTests.Tests
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver driver = null!;
        private LoginPage loginPage = null!;

        [SetUp]
        public void SetUp()
        {
            TestConfig config = ConfigReader.Config;

            switch (config.Browser.ToLower())
            {
                case "chrome":
                    ChromeOptions chromeOptions = new ChromeOptions();

                    if (config.Headless)
                    {
                        chromeOptions.AddArgument("--headless=new");
                    }

                    driver = new ChromeDriver(chromeOptions);
                    break;

                case "edge":
                    EdgeOptions edgeOptions = new EdgeOptions();

                    if (config.Headless)
                    {
                        edgeOptions.AddArgument("--headless=new");
                    }

                    driver = new EdgeDriver(edgeOptions);
                    break;

                case "firefox":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();

                    if (config.Headless)
                    {
                        firefoxOptions.AddArgument("-headless");
                    }

                    driver = new FirefoxDriver(firefoxOptions);
                    break;

                default:
                    throw new ArgumentException(
                        "Trinh duyet khong duoc ho tro: " + config.Browser
                    );
            }

            driver.Manage().Window.Size = new Size(
                config.WindowWidth,
                config.WindowHeight
            );

            loginPage = new LoginPage(driver);

            loginPage.MoTrangChu();
        }

        [Test]
        public void DangNhapThanhCong()
        {
            loginPage.DangNhap(
                "standard_user",
                "secret_sauce"
            );

            Assert.That(
                loginPage.KiemTraDangNhapThanhCong(),
                Is.True,
                "Dang nhap thanh cong phai hien thi trang Products"
            );
        }

        [Test]
        public void DangNhapThatBai()
        {
            loginPage.DangNhap(
                "invalid_user",
                "wrong_password"
            );

            string message = loginPage.LayThongBaoLoi();

            Assert.That(
                message,
                Does.Contain("Username and password do not match"),
                "He thong phai hien thi thong bao dang nhap that bai"
            );
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
