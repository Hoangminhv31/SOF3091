# Lab 5 — Framework Selenium + NUnit: Config, DriverFactory, BaseTest, Data-Driven

## Cấu trúc

```
Config/     appsettings.json, TestConfig.cs, ConfigReader.cs
Core/       DriverFactory.cs, BaseTest.cs
Pages/      BasePage.cs, LoginPage.cs, InventoryPage.cs
Data/       users.json, UserData.cs, UserDataReader.cs
Utilities/  ScreenshotHelper.cs
Tests/      LoginTests.cs, DataDrivenLoginTests.cs
screenshots/  ảnh minh chứng (Phần B, D)
```

## Cách chạy

```bash
dotnet test                              # chạy tất cả
TEST_Browser=firefox dotnet test         # đổi trình duyệt, KHÔNG sửa .cs
```

## Minh chứng

| Phần | Ảnh | Nội dung |
|------|-----|----------|
| B | `screenshots/chrome.png`, `screenshots/firefox.png` | Cùng code, chỉ đổi `"Browser"` trong `appsettings.json` |
| D | `screenshots/truoc.png`, `screenshots/sau.png` | Thêm 1 bộ vào `users.json` → số ca test tăng, không sửa `.cs` |

---

## Phần E — Thẩm định dữ liệu test do AI sinh

**Công cụ AI:** Claude (Anthropic).

### 1. Prompt đã dùng

```
Bạn là kiểm thử viên. Hãy sinh 10 bộ dữ liệu test cho form đăng nhập của
https://www.saucedemo.com/ .
Trả về DUY NHẤT một mảng JSON, mỗi phần tử đúng schema sau (giữ nguyên tên khóa):
{ "Username": string, "Password": string, "ThanhCong": bool, "GhiChu": string }
Trong đó ThanhCong = true nếu kỳ vọng đăng nhập vào được trang sản phẩm.
Bao gồm cả luồng thuận, luồng nghịch và các ca biên.
```

### 2. Toàn bộ kết quả AI trả về (chưa chỉnh sửa)

```json
[
  { "Username": "standard_user",   "Password": "secret_sauce",   "ThanhCong": true,  "GhiChu": "Tài khoản chuẩn" },
  { "Username": "locked_out_user", "Password": "secret_sauce",   "ThanhCong": false, "GhiChu": "Tài khoản bị khóa" },
  { "Username": "problem_user",    "Password": "secret_sauce",   "ThanhCong": true,  "GhiChu": "Tài khoản có lỗi giao diện nhưng vẫn đăng nhập được" },
  { "Username": "admin",           "Password": "admin123",       "ThanhCong": true,  "GhiChu": "Tài khoản quản trị viên" },
  { "Username": "standard_user",   "Password": "wrong_password", "ThanhCong": false, "GhiChu": "Sai mật khẩu" },
  { "Username": "",                "Password": "",               "ThanhCong": false, "GhiChu": "Bỏ trống cả hai ô" },
  { "Username": " standard_user ", "Password": "secret_sauce",   "ThanhCong": true,  "GhiChu": "Thừa khoảng trắng đầu/cuối, hệ thống tự cắt bỏ" },
  { "Username": "STANDARD_USER",  "Password": "secret_sauce",   "ThanhCong": true,  "GhiChu": "Không phân biệt hoa/thường" },
  { "Username": "' OR '1'='1",    "Password": "x",              "ThanhCong": false, "GhiChu": "Thử SQL injection" },
  { "Username": "standard_user",   "Password": "wrong_password", "ThanhCong": false, "GhiChu": "Tài khoản bị khóa tạm thời sau 3 lần nhập sai liên tiếp" }
]
```

### 3. Thẩm định từng bộ

| # | Bộ dữ liệu | Kết luận | Lý do |
|---|-----------|----------|-------|
| 1 | standard_user / secret_sauce | ✅ Dùng được | Tài khoản có thật (trùng ca 1 trong `users.json`). |
| 2 | locked_out_user | ✅ Dùng được | Hành vi đúng thực tế (trùng ca 5). |
| 3 | problem_user | ✅ Dùng được | Đăng nhập vào được (trùng ca 2). |
| 4 | admin / admin123 | ❌ **KHÔNG dùng được** | Saucedemo **không có** tài khoản `admin`. AI bịa ra tài khoản cho một tính năng/vai trò không tồn tại → kỳ vọng `true` sai, ca test sẽ đỏ dù trang hoạt động đúng. |
| 5 | standard_user / wrong_password | ✅ Dùng được | Ca nghịch hợp lệ (trùng ca 6). |
| 6 | "" / "" | ✅ Dùng được | Ca biên hợp lệ (trùng ca 10). |
| 7 | `" standard_user "` kỳ vọng `true` | ❌ **KHÔNG dùng được** | AI **giả định** trang tự `trim`. Saucedemo so khớp chính xác chuỗi nhập, không cắt khoảng trắng → đăng nhập **thất bại**. Kỳ vọng đúng phải là `false` (đã sửa thành ca 12 trong `users.json`). |
| 8 | `STANDARD_USER` kỳ vọng `true` | ❌ **KHÔNG dùng được** | AI giả định không phân biệt hoa/thường. Username của saucedemo **phân biệt** hoa/thường → kỳ vọng đúng là `false` (đã sửa thành ca 13). |
| 9 | `' OR '1'='1` | ✅ Dùng được | Kỳ vọng `false` đúng; đã đưa vào `users.json` (ca 14). |
| 10 | standard_user / wrong_password, ghi chú "khóa sau 3 lần sai" | ❌ **KHÔNG dùng được** | Saucedemo **không có** cơ chế khóa tạm thời sau nhiều lần sai. Ngoài ra bản ghi này **giống hệt bộ 5** (trùng lặp), và schema 1 dòng/1 lần đăng nhập không diễn tả được kịch bản "3 lần liên tiếp". |

**Tổng kết:** 4/10 bộ không dùng được (bộ 4, 7, 8, 10); 6/10 bộ dùng được (1, 2, 3, 5, 6, 9),
trong đó 5 bộ (1, 2, 3, 5, 6) đã trùng với dữ liệu viết tay nên chỉ bộ 9 là bổ sung thêm.

> Lưu ý: bộ 7 và 8 *trông có vẻ hợp lý* và vẫn đúng schema JSON — lỗi nằm ở **kỳ vọng sai với hành vi thật
> của trang**, thứ mà AI không thể biết nếu không chạy thử. Vì vậy dữ liệu AI sinh luôn phải được đối chiếu
> với hệ thống thật trước khi đưa vào `users.json`.

### 4. Bài học rút ra

- AI sinh **dữ liệu**; con người quyết định **kỳ vọng đúng** (`ThanhCong`).
- Cần chạy thử từng bộ để xác nhận hành vi thật; không tin ghi chú của AI.
- Lưu ý khi tự thêm `performance_glitch_user`: trang cố tình trễ vài giây khi đăng nhập, mà
  `InventoryPage.DaHienThi()` chỉ chờ 3 giây → sẽ cho kết quả `false` sai. Nếu cần ca này, phải nâng timeout riêng cho nó.
