# SOF3091 - Automation Testing (.NET)

master

![CI](https://github.com/Hoangminhv31/SOF3091/actions/workflows/tests.yml/badge.svg)

## Mô tả
Dự án kiểm thử tự động sử dụng .NET, Selenium WebDriver và mô hình Page Object Model (POM).

## Công nghệ sử dụng
- .NET 8
- Selenium WebDriver
- NUnit
- GitHub Actions (CI/CD)

## Cấu trúc thư mục
- `Pages/`: Chứa các Page Object (BasePage, LoginPage, InventoryPage...)
- `Tests/`: Chứa các Test Case
- `.github/workflows/`: Chứa file cấu hình CI/CD

## Cách chạy test
Mở terminal và chạy các lệnh sau:
    dotnet restore
    dotnet test

*Lưu ý: Để chạy test trên môi trường không có giao diện (CI), set biến môi trường `HEADLESS=true`.*

## Y5
- `appsettings.json`: cấu hình URL, trình duyệt, timeout và chế độ headless.
- Có thể thay đổi `Url`, `Browser`, `Timeout`, `Headless` trong `appsettings.json`.

