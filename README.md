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
4. Các bước thực hiện
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


PHẦN D – LỒNG GHÉP AI
1. Tình huống
Trong quá trình cài đặt môi trường để chạy một dự án web, mình cần cài Node.js và npm ngay trong terminal của Visual Studio Code. Mình gặp hai lỗi liên tiếp: (1) terminal không nhận lệnh npm, rồi báo PowerShell chặn không cho chạy script npm.ps1; (2) Node.js cài ra là bản v26.8.1, không đúng bản 24.x mà dự án yêu cầu. Mình dùng trợ lý AI Claude (Anthropic) để giải thích nguyên nhân và hướng dẫn sửa.
2. Prompt đã dùng và câu trả lời của AI
Lượt	Prompt của mình	Câu trả lời của AI (tóm tắt)
1	Gửi ảnh chụp terminal VS Code báo lỗi: npm : The term 'npm' is not recognized... (Hình 1).	Node đã cài nhưng VS Code mở từ trước nên chưa nhận biến môi trường PATH mới, cần tắt hẳn VS Code rồi mở lại. AI cũng chỉ ra terminal đang ở sai thư mục dự án.
2	"tôi muốn cài node trong visual luôn"	Hướng dẫn cài Node bằng lệnh winget install ngay trong terminal VS Code, hoặc dùng nvm-windows để chọn đúng phiên bản.
3	"ủa nó ghi success r mà saoo vẫn k đc"	Terminal đang mở chưa nhận PATH mới sau khi cài. AI đưa lệnh làm mới PATH trong phiên hiện tại ($env:Path = ...), hoặc tắt hẳn VS Code rồi mở lại.
4	Gửi ảnh kết quả sau khi chạy lệnh làm mới PATH: node --version ra v26.8.1, npm --version báo lỗi chặn script (Hình 2).	AI chỉ ra 2 vấn đề: (1) PowerShell đang chặn script, sửa bằng Set-ExecutionPolicy -Scope CurrentUser RemoteSigned; (2) Node v26 sai phiên bản, nên gỡ ra và cài nvm-windows, rồi chạy nvm install 24.22.0 và nvm use 24.22.0.
5	Gửi ảnh npm --version ra 11.19.0 sau khi chạy lệnh ở lượt 4 (Hình 3).	AI xác nhận lỗi chặn script đã hết và npm 11.19.0 đúng yêu cầu, nhắc mình còn phải đổi Node sang bản 24.
3. Ảnh chụp minh họa
 
Hình 1: Terminal báo lỗi lệnh npm không được nhận
 
Hình 2: Node là v26.8.1 và npm bị PowerShell chặn chạy script
 
Hình 3: Sau khi đổi chính sách thực thi, npm --version cho kết quả 11.19.0
 
Hình 4: Sau khi cài lại bằng nvm, Node là v24.22.0 và npm ci cài thành công
4. Kết quả
•	Lệnh npm chạy được, npm phiên bản 11.19.0.
•	Node.js đúng phiên bản yêu cầu v24.22.0, lệnh npm ci cài thành công 615 package.
5. Nhận xét
AI hữu ích vì đọc được thông báo lỗi trong ảnh chụp, giải thích đúng nguyên nhân (PowerShell chặn script npm.ps1, terminal chưa nhận PATH mới) và đưa ngay lệnh sửa, nên mình không phải tự tra cứu từng lỗi. AI cũng chỉ ra những chỗ mình chưa để ý, như terminal đang mở sai thư mục dự án và Node v26 không đúng bản 24.x yêu cầu. Tuy vậy câu trả lời không hoàn toàn chính xác ngay từ đầu: lệnh cài Node bằng winget mà AI gợi ý đã cài bản mới nhất (v26.8.1) thay vì bản 24, nên mình phải gỡ ra và cài lại bằng nvm, mất thêm một vòng. Mình chỉ biết lệnh nào đúng khi tự chạy và đối chiếu kết quả bằng node --version và npm --version, chứ không thể tin hoàn toàn vào câu trả lời. Vì vậy mình dùng AI để gợi ý và giải thích, còn việc kiểm tra kết quả vẫn phải tự làm.


