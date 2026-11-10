using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstTests
{
    public class LoginTests
    {
        // 1. Khai báo driver ở cấp class (lỗi của bạn là do thiếu dòng này)
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            // Khởi tạo trình duyệt trước mỗi ca test
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        // Hàm phụ dùng chung để đăng nhập
        private void Login(string username, string password)
        {
            driver.FindElement(By.Id("user-name")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);

            // Dùng locator đã chốt ở Phần D
            driver.FindElement(By.Id("login-button")).Click();
        }

        [Test]
        public void Test_Login_Thanh_Cong()
        {
            // Ca 1: Luồng thuận
            Login("standard_user", "secret_sauce");

            // Assert chuyển sang inventory.html
            Assert.That(driver.Url, Does.Contain("inventory.html"), "Không chuyển được sang trang inventory");
        }

        [Test]
        public void Test_Login_That_Bai()
        {
            // Ca 2: Luồng nghịch (dùng locked_out_user)
            Login("locked_out_user", "secret_sauce");

            // Assert thông báo lỗi
            string errorMsg = driver.FindElement(By.CssSelector("[data-test='error']")).Text;
            Assert.That(errorMsg, Does.Contain("Sorry, this user has been locked out."), "Thông báo lỗi không đúng");
        }

        [Test]
        public void Test_Lay_Danh_Sach_San_Pham()
        {
            Login("standard_user", "secret_sauce");

            var items = driver.FindElements(By.ClassName("inventory_item"));
            Assert.That(items.Count, Is.EqualTo(6), "Số lượng sản phẩm không phải là 6");

            var names = new List<string>();          // <-- (1) THÊM: ngay trước foreach

            foreach (var item in items)
            {
                string name = item.FindElement(By.ClassName("inventory_item_name")).Text;
                string price = item.FindElement(By.ClassName("inventory_item_price")).Text;

                Console.WriteLine($"Sản phẩm: {name} - Giá: {price}");

                Assert.That(name, Is.Not.Empty, "Tên sản phẩm bị rỗng");
                Assert.That(price, Does.StartWith("$"), $"Giá của {name} không bắt đầu bằng $");

                names.Add(name);                     // <-- (2) THÊM: cuối vòng foreach, trước dấu }
            }

            // (3) THÊM: sau dấu } của foreach, trước dấu } đóng hàm
            Assert.That(names, Does.Contain("Sauce Labs Backpack"), "Thiếu sản phẩm Sauce Labs Backpack");
        }


        [Test]
        [Explicit]
        public void Test_Teardown()
        {
            Assert.Fail("Test fail de kiem tra chup anh.");
        }

        [TearDown]
        public void Teardown()
        {
            if (driver == null) return;

            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    string folder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "screenshots"));
                    Directory.CreateDirectory(folder);

                    string fileName = TestContext.CurrentContext.Test.Name + ".png";
                    string path = Path.Combine(folder, fileName);

                    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(path);
                    TestContext.AddTestAttachment(path);
                }
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}