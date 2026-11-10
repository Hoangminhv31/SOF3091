# Lab 2 - Selenium WebDriver cơ bản

## Phần A — Bảng locator

| STT | Phần tử | Locator 1 (Chọn) | Locator 2 (Dự phòng) | Lý do chọn |
|---|---|---|---|---|
| 1 | Ô Username | `By.Id("user-name")` | `By.CssSelector("[data-test='username']")` | Id là duy nhất, không phụ thuộc DOM. |
| 2 | Ô Password | `By.Id("password")` | `By.Name("password")` | Id ngắn gọn, truy xuất nhanh nhất. |
| 3 | Nút Login | `By.Id("login-button")` | `By.CssSelector("[data-test='login-button']")` | Id duy nhất. (Xem thêm ở Phần D) |
| 4 | Tên sản phẩm | `By.ClassName("inventory_item_name")` | `By.CssSelector(".inventory_item_name")` | Dùng Class vì có nhiều sản phẩm, cần lấy danh sách. |
| 5 | Giá sản phẩm | `By.ClassName("inventory_item_price")` | `By.CssSelector(".inventory_item_price")` | Dùng Class để lấy giá tương ứng với từng sản phẩm. |

*Ảnh chụp Ctrl+F xác nhận locator khớp 1 of 1:*

![Ctrl+F login](screenshots/D_ctrlf_id.png)

---

## Phần B & C — Kết quả chạy Test tự động

- Đã viết thành công các ca test bằng NUnit.
- **Ca 1 (Thuận):** Đăng nhập thành công, assert chuyển hướng sang `inventory.html`.
- **Ca 2 (Nghịch):** Đăng nhập sai (locked_out_user), assert hiện thông báo lỗi đúng nội dung.
- **Phần C:** Lấy được danh sách 6 sản phẩm, dùng `item.FindElement` để lấy tên và giá bên trong, assert giá bắt đầu bằng `$` và tên không rỗng.
- **Ảnh chụp kết quả Passed:**
![Test Passed](screenshots/B_test_passed.png)

---

## Phần D — Lồng ghép AI (Tối ưu Locator)

**1. Phần tử được chọn:** Nút Login trên trang saucedemo.com

**outerHTML (copy từ DevTools):**
```html
<input class="submit-button btn_action" data-test="login-button" id="login-button" type="submit" value="Login" name="login-button">
```

**2. Prompt đã dùng (gửi cho Claude):**
```
Tôi đang viết test Selenium C# cho trang saucedemo.com. Đây là outerHTML của nút Login:

<input class="submit-button btn_action" data-test="login-button" id="login-button" type="submit" value="Login" name="login-button">

Hãy đề xuất locator bền vững hơn, mô tả danh tính của phần tử chứ không mô tả đường đi.
Cho tôi ít nhất 2 phương án, xếp theo độ ưu tiên, và giải thích vì sao.
```

**3. Câu trả lời của AI:**
1. `By.Id("login-button")` (ưu tiên nhất): id thường duy nhất trên trang, ngắn gọn và không phụ thuộc vị trí trong DOM.
2. `By.CssSelector("[data-test='login-button']")` (dự phòng): thuộc tính `data-test` được thêm riêng cho kiểm thử nên ít bị đổi khi giao diện thay đổi.
3. `By.CssSelector("input[type='submit'][value='Login']")` (cuối cùng): mô tả theo danh tính nhưng sẽ gãy nếu đổi chữ trên nút.

AI cũng khuyên tránh `By.ClassName("btn_action")` vì class thiên về giao diện và có thể dùng cho nhiều nút khác.

**4. Tự kiểm chứng bằng Ctrl+F (tab Elements):**

| Locator | Dán vào Ctrl+F | Kết quả |
|---|---|---|
| `By.Id` | `#login-button` | 1 of 1 |
| CSS data-test | `[data-test='login-button']` | 1 of 1 |
| CSS type + value | `input[type='submit'][value='Login']` | 1 of 1 |

![Ctrl+F login button](screenshots/D_ctrlf_login_button.png)

**5. Nhận xét:** AI ưu tiên đúng `id` rồi `data-test`, và cả hai đều khớp 1 of 1 khi mình kiểm tra bằng Ctrl+F. AI cũng đúng khi cảnh báo không dùng class `btn_action`. Phương án 3 vẫn khớp 1 of 1 nhưng kém bền nhất vì phụ thuộc chữ hiển thị `Login`, AI chỉ nói lướt qua điểm này. AI cũng nói "id thường duy nhất" theo kiến thức chung, trong khi thực tế chỉ chắc chắn khi đã Ctrl+F kiểm tra trên trang. Vì vậy mình chọn `By.Id("login-button")` làm locator chính và `data-test` làm dự phòng.