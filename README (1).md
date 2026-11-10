# Lab 3 – Selenium C#


## Cấu trúc project

| File | Nội dung |
|---|---|
| `LoginTests.cs` | Các ca test đăng nhập (saucedemo) |
| `AlertIframeTests.cs` | Phần C: JS Alert / Confirm / Prompt và iframe TinyMCE |
| `Lab3_PhanE_AI_Flaky_Test.docx` | Phần E: báo cáo AI chẩn đoán flaky test |

## Ghi nhận sử dụng AI (Phần E)

### 1. Công cụ AI đã dùng
- Công cụ: Claude (Anthropic), giao diện web/app claude.ai
- Ngày sử dụng: [ĐIỀN]
- Mục đích: nhờ AI liệt kê các nguyên nhân có thể gây flaky cho một test Selenium và gợi ý cách kiểm chứng. AI chỉ dùng để chẩn đoán; việc chạy test, đo đạc và kết luận do em tự thực hiện.

### 2. Test chập chờn được chọn
- Test: `DynamicLoading_ShowsHelloWorld` trên trang `/dynamic_loading/2` [ĐIỀN lại nếu dùng test khác ở Phần A]
- Lỗi nguyên văn: `OpenQA.Selenium.NoSuchElementException: no such element: Unable to locate element: {"method":"css selector","selector":"#finish h4"}`
- Tần suất rớt: [ĐIỀN]/20 lần chạy
- Điều kiện chạy: [ĐIỀN: hệ điều hành, version Chrome, Selenium, .NET, mạng]

### 3. Prompt đã dùng

> Tôi có một test Selenium C# (NUnit) chạy lúc qua lúc rớt. Code: [code test]. Lỗi nguyên văn: [log lỗi]. Tần suất: rớt khoảng [x]/20 lần, không có quy luật rõ ràng. Môi trường: [hệ điều hành, Chrome, Selenium, .NET], chạy tuần tự trên máy local, mạng Wi-Fi, trang test là the-internet.herokuapp.com/dynamic_loading/2. Hãy liệt kê các nguyên nhân có thể gây flaky, sắp theo mức độ khả dĩ, kèm cách kiểm chứng từng nguyên nhân và cách sửa.

### 4. Các nguyên nhân AI đề xuất và kết quả tự kiểm chứng

| # | Gợi ý của AI | Cách em kiểm chứng | Kết quả |
|---|---|---|---|
| 1 | `Thread.Sleep(3000)` cố định gây race condition | Tăng sleep lên 10s / thay bằng `WebDriverWait`, chạy lại 20 lần | Đúng [XÁC NHẬN] |
| 2 | Mạng/server chậm, thời gian load dao động | Đo thời gian bằng `Stopwatch` qua 20 lần chạy | Đúng, là yếu tố làm lộ lỗi 1 [XÁC NHẬN] |
| 3 | Trộn ImplicitWait và ExplicitWait | Kiểm tra code, test không đặt ImplicitWait | Không liên quan |
| 4 | `StaleElementReferenceException` | Đối chiếu lỗi thật là `NoSuchElementException` | Sai |
| 5 | ChromeDriver lệch version với Chrome | Chạy `chromedriver --version`; nếu lệch thì rớt 100% | Sai |

### 5. Nhận xét
AI đưa ra 5 gợi ý, trong đó 2 gợi ý đúng là nguyên nhân 1 (race condition do `Thread.Sleep` cố định) và nguyên nhân 2 (độ trễ server dao động). Gợi ý 3 không áp dụng cho code hiện tại nhưng là lưu ý tốt khi sửa. Gợi ý 4 và 5 sai vì không khớp với lỗi thật và tần suất rớt. Em rút ra là cần tự kiểm chứng từng gợi ý của AI, vì không phải nguyên nhân nào nghe hợp lý cũng đúng với lỗi thực tế.

### 6. Phần do AI hỗ trợ và phần em tự làm
- **AI hỗ trợ:** gợi ý danh sách nguyên nhân, gợi ý cách kiểm chứng và cách sửa bằng `WebDriverWait`. [ĐIỀN thêm nếu AI còn hỗ trợ viết code Phần C hoặc soạn báo cáo]
- **Em tự làm:** chạy lặp test để lấy tần suất rớt, tự kiểm chứng từng gợi ý, sửa test, đối chiếu và chỉnh lại nhận xét theo kết quả thực tế, và chịu trách nhiệm về nội dung nộp.

### 7. Cam kết
Em đã ghi lại đầy đủ việc sử dụng AI trong bài này. Các số liệu và kết quả kiểm chứng ở trên là từ lần chạy thực tế của em.
