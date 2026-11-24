# Lab 5 (Lab 4 POM) — Phần B & E

## Phần E — Thẩm định AI

**AI sử dụng:** (ghi tên + phiên bản)

**Prompt:**
```
Bạn là kỹ sư kiểm thử C# / Selenium / NUnit. Hãy refactor file test dưới đây
sang Page Object Model theo ĐÚNG các quy ước sau:
1. Tạo BasePage (abstract) chứa driver, WebDriverWait và hàm Tim(By) tìm phần tử
   kèm chờ. Mọi Page class phải kế thừa BasePage, KHÔNG tự khai báo lại driver/wait.
2. Locator phải là `private readonly By`; hành vi nghiệp vụ là `public`.
3. KHÔNG có Assert bên trong Page Object.
4. KHÔNG có Page Object nào trả về IWebElement.
5. Hành vi chuyển trang trả về `new TrangMoi(driver)`, hành vi ở lại trang trả về `this`.
6. Tên hàm đặt theo việc người dùng làm được (DangNhap, SapXep), không đặt theo thẻ HTML.
7. File test sau refactor không được chứa chữ "By".
8. Namespace: Pages dùng FirstTests.Pages.
9. Không được thay đổi hành vi hay bỏ mất Assert của test gốc.

Trả về: BasePage.cs, LoginPage.cs, InventoryPage.cs và file test đã refactor.

[Dán file test gốc]
```

**Kết quả AI trả về:** (dán nguyên văn code AI trả về)

**Lỗi phát hiện:**
- Vị trí: (file, hàm)
- AI làm: (mô tả)
- Vì sao sai: (giải thích)
- Cách sửa: (mô tả)

**Các tiêu chí đã kiểm:**

| # | Tiêu chí | Kết quả |
|---|----------|---------|
| 1 | Test không còn chữ `By` | |
| 2 | Không có `Assert` trong Pages | |
| 3 | Không Page nào trả `IWebElement` | |
| 4 | Locator là `private readonly` | |
| 5 | Page không khai báo lại driver/wait | |
| 6 | `Tim()` dùng `wait.Until` | |
| 7 | Không mất Assert/hành vi gốc | |
| 8 | `dotnet test` giống trước refactor | |
