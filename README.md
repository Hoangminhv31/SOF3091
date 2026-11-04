PHẦN B – KỊCH BẢN SELENIUM ĐẦU TIÊN
1. Mục tiêu
Viết một test Selenium tự động: mở trình duyệt Chrome, điều hướng đến một trang web, đọc và in tiêu đề trang, kiểm tra kết quả bằng Assert, rồi đóng trình duyệt bằng Quit().
2. Môi trường thực hiện
•	Công cụ: Visual Studio
•	Ngôn ngữ: C#
•	Framework kiểm thử: NUnit 4.3.2 (kèm NUnit3TestAdapter 5.0.0)
•	Thư viện tự động hóa: Selenium.WebDriver 4.52.0
•	Trình duyệt: Google Chrome
•	Trang web kiểm thử: https://example.com
3. Các bước thực hiện
1.	Mở trình duyệt Chrome bằng new ChromeDriver().
2.	Điều hướng đến trang https://example.com bằng GoToUrl.
3.	Đọc tiêu đề trang bằng driver.Title và in ra bằng Console.WriteLine.
4.	Dùng Assert.That để khẳng định tiêu đề trang bằng “Example Domain”.
5.	Đóng trình duyệt bằng driver.Quit() trong khối finally, để trình duyệt luôn được đóng kể cả khi test thất bại.
4. Mã nguồn
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace FirstTests
{
    public class Tests
    {
        [Test]
        public void OpenPage_PrintTitle_AssertTitle()
        {
            IWebDriver driver = null;
            try
            {
                // 1. Mở trình duyệt Chrome
                driver = new ChromeDriver();
                
                // 2. Điều hướng đến trang web
                driver.Navigate().GoToUrl("https://example.com");
                
                // 3. Đọc và in tiêu đề trang
                string title = driver.Title;
                Console.WriteLine("Tieu de trang: " + title);
                
                // 4. Kiểm tra bằng Assert
                Assert.That(title, Is.EqualTo("Example Domain"));
            }
            finally
            {
                // 5. Đóng trình duyệt
                if (driver != null)
                {
                    driver.Quit();
                }
            }
        }
    }
}
 
5. Kết quả mong đợi
Tiêu đề trang đọc được là “Example Domain”, trùng với giá trị trong Assert nên test đạt (Passed).
6. Kết quả thực tế
•	Test OpenPage_PrintTitle_AssertTitle chạy thành công, trạng thái Passed (1 Passed, 0 Failed), thời gian chạy 7.2 giây.
•	Standard Output in ra: Tieu de trang: Example Domain.
•	Chrome được mở, vào trang và được đóng tự động sau khi chạy xong.

 
7. Kết luận
Kịch bản Selenium đã thực hiện đúng các bước yêu cầu: mở Chrome, điều hướng, đọc và in tiêu đề, kiểm tra bằng Assert và đóng trình duyệt bằng Quit(). Test cho kết quả Passed.

