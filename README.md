# Phần A — Bảng locator trên saucedemo.com

Trang kiểm thử: https://www.saucedemo.com
Tài khoản dùng để xem trang sản phẩm: `standard_user` / `secret_sauce`.

## 1. Bảng locator

| # | Phần tử | Locator 1 | Locator 2 | Locator được chọn | Lý do chọn |
|---|---------|-----------|-----------|-------------------|------------|
| 1 | Ô Username | CSS: `#user-name` | CSS: `[data-test="username"]` | `#user-name` | `id` duy nhất trên trang, ngắn gọn, dễ đọc. `data-test` là thuộc tính dành riêng cho kiểm thử nên được giữ làm phương án dự phòng khi `id` bị đổi. |
| 2 | Ô Password | CSS: `#password` | XPath: `//input[@placeholder='Password']` | `#password` | `id` duy nhất và ổn định. XPath theo `placeholder` dài hơn và sẽ hỏng nếu văn bản hiển thị thay đổi. |
| 3 | Nút Login | CSS: `#login-button` | XPath: `//input[@type='submit' and @value='Login']` | `#login-button` | `id` rõ nghĩa và duy nhất. XPath dài, phụ thuộc vào `value` hiển thị. |
| 4 | Tên sản phẩm (Sauce Labs Backpack) | CSS: `#item_4_title_link` | XPath: `//div[contains(@class,'inventory_item_name') and text()='Sauce Labs Backpack']` | `#item_4_title_link` | Trang có 6 sản phẩm nên `.inventory_item_name` khớp 6 phần tử, không dùng riêng được. `id` gắn với mã sản phẩm nên duy nhất và không đổi khi người dùng sắp xếp lại danh sách. |
| 5 | Giá sản phẩm (Sauce Labs Backpack) | XPath: `//div[text()='Sauce Labs Backpack']/ancestor::div[@class='inventory_item']//div[@class='inventory_item_price']` | CSS: `.inventory_item:nth-child(1) .inventory_item_price` | XPath đi từ tên sản phẩm | Giá nằm cùng khối `.inventory_item` với tên sản phẩm, nên đi từ tên lên khối cha rồi xuống giá. Cách này bám theo sản phẩm, không bám theo vị trí. `nth-child` sẽ sai khi đổi thứ tự sắp xếp. |

## 2. Ảnh chụp DevTools (Ctrl+F, khớp 1 of 1)

Mỗi ảnh chụp locator được chọn trong ô tìm kiếm của tab Elements, thể hiện kết quả **1 of 1** và dòng HTML được tô sáng.

### 2.1. Ô Username — `#user-name`

![Ô Username khớp 1 of 1](images/01-username.png)

### 2.2. Ô Password — `#password`

![Ô Password khớp 1 of 1](images/02-password.png)

### 2.3. Nút Login — `#login-button`

![Nút Login khớp 1 of 1](images/03-login.png)

### 2.4. Tên sản phẩm — `#item_4_title_link`

![Tên sản phẩm khớp 1 of 1](images/04-product-name.png)

### 2.5. Giá sản phẩm — XPath theo tên sản phẩm

```
//div[text()='Sauce Labs Backpack']/ancestor::div[@class='inventory_item']//div[@class='inventory_item_price']
```

![Giá sản phẩm khớp 1 of 1](images/05-product-price.png)

## 3. Nguyên tắc chọn locator

1. Ưu tiên `id` nếu có và duy nhất, vì ngắn gọn và ít thay đổi nhất.
2. Nếu không có `id` phù hợp thì dùng `data-test` hoặc thuộc tính ổn định khác.
3. Chỉ dùng XPath khi cần đi theo quan hệ giữa các phần tử (ví dụ từ tên sản phẩm sang giá).
4. Tránh locator dựa vào vị trí (`nth-child`, chỉ số) hoặc văn bản dễ đổi.
5. Mọi locator đều được kiểm tra bằng Ctrl+F trong DevTools và phải cho kết quả **1 of 1**.
