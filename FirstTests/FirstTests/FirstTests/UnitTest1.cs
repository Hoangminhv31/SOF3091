    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    namespace FirstTests;

    public class FirstSeleniumTest
    {
        [Test]
        public void OpenPage_PrintTitle_AssertTitle()
        {
            // 1. Mở trình duyệt Chrome
            IWebDriver driver = new ChromeDriver();

            try
            {
                // 2. Điều hướng đến một trang web
                driver.Navigate().GoToUrl("https://example.com");

                // 3. Đọc và in tiêu đề trang
                string title = driver.Title;
                Console.WriteLine($"Tieu de trang: {title}");

                // 4. Assert: khẳng định kết quả mong đợi
                Assert.That(title, Is.EqualTo("Example Domain"));
            }
            finally
            {
                // 5. Đóng trình duyệt bằng Quit()
                driver.Quit();
            }
        }
    }