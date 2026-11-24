# Lab 5 — OOP trong kiểm thử & Page Object Model

## Phần E — Thẩm định AI

**AI sử dụng:** Claude (Anthropic), giao diện claude.ai
**File test được refactor:** `LoginTests.cs` (bản script-based trước refactor, ở mục 2)

### 1. Prompt đã gửi

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

[file LoginTests.cs gốc ở mục 2]
```

### 2. File test gốc (script-based, trước refactor)

```csharp
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests;

public class LoginTests
{
    private IWebDriver driver = null!;

    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://www.saucedemo.com");
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void DangNhapDung_VaoTrangSanPham()
    {
        driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
        driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
        driver.FindElement(By.Id("login-button")).Click();

        Assert.That(driver.FindElement(By.CssSelector(".title")).Displayed, Is.True);
    }

    [Test]
    public void DangNhapSai_HienThongBaoLoi()
    {
        driver.FindElement(By.Id("user-name")).SendKeys("sai_user");
        driver.FindElement(By.Id("password")).SendKeys("sai_pass");
        driver.FindElement(By.Id("login-button")).Click();

        var loi = driver.FindElement(By.CssSelector("[data-test='error']")).Text;
        Assert.That(loi, Does.Contain("do not match"));
    }
}
```

### 3. Kết quả AI trả về (nguyên văn, chưa sửa)

**Pages/BasePage.cs**
```csharp
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver driver;
    protected readonly WebDriverWait wait;

    protected BasePage(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    protected IWebElement Tim(By locator)
    {
        return wait.Until(d =>
        {
            var el = d.FindElement(locator);
            return el.Displayed ? el : null;
        })!;
    }
}
```

**Pages/LoginPage.cs**
```csharp
using OpenQA.Selenium;

namespace FirstTests.Pages;

public class LoginPage : BasePage
{
    private readonly By oTaiKhoan   = By.Id("user-name");
    private readonly By oMatKhau    = By.Id("password");
    private readonly By nutDangNhap = By.Id("login-button");
    private readonly By thongBaoLoi = By.CssSelector("[data-test='error']");

    public LoginPage(IWebDriver driver) : base(driver) { }

    public InventoryPage DangNhap(string tk, string mk)
    {
        NhapThongTin(tk, mk);
        return new InventoryPage(driver);
    }

    public LoginPage DangNhapSai(string tk, string mk)
    {
        NhapThongTin(tk, mk);
        return this;
    }

    public string LayThongBaoLoi() => Tim(thongBaoLoi).Text;

    private void NhapThongTin(string tk, string mk)
    {
        Tim(oTaiKhoan).SendKeys(tk);
        Tim(oMatKhau).SendKeys(mk);
        Tim(nutDangNhap).Click();
    }
}
```

**Pages/InventoryPage.cs**
```csharp
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

public class InventoryPage : BasePage
{
    private readonly By tieuDeTrang = By.CssSelector(".title");
    private readonly By oSapXep     = By.CssSelector("[data-test='product-sort-container']");
    private readonly By giaSanPham  = By.CssSelector(".inventory_item_price");

    public InventoryPage(IWebDriver driver) : base(driver) { }

    public bool DaHienThi() => Tim(tieuDeTrang).Displayed;

    public InventoryPage SapXep(string value)
    {
        new SelectElement(Tim(oSapXep)).SelectByValue(value);
        return this;
    }

    public List<double> LayDanhSachGia()
    {
        Tim(giaSanPham);
        return driver.FindElements(giaSanPham)
                     .Select(e => double.Parse(e.Text.Replace("$", "")))
                     .ToList();
    }
}
```

**Tests/LoginTests.cs**
```csharp
using FirstTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests.Tests;

public class LoginTests
{
    private IWebDriver driver = null!;

    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://www.saucedemo.com");
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void DangNhapDung_VaoTrangSanPham()
    {
        var inventory = new LoginPage(driver).DangNhap("standard_user", "secret_sauce");

        Assert.That(inventory.DaHienThi(), Is.True);
    }

    [Test]
    public void DangNhapSai_HienThongBaoLoi()
    {
        var loi = new LoginPage(driver)
            .DangNhapSai("sai_user", "sai_pass")
            .LayThongBaoLoi();

        Assert.That(loi, Does.Contain("do not match"));
    }
}
```

### 4. Thẩm định

**Các tiêu chí đã kiểm:**

| # | Tiêu chí | Cách kiểm | Kết quả |
|---|----------|-----------|---------|
| 1 | Test không còn chữ `By` | Ctrl+F `By` trong `Tests/` | Đạt (0 kết quả) |
| 2 | Không có `Assert` trong Pages | Ctrl+F `Assert` trong `Pages/` | Đạt |
| 3 | Không Page nào trả `IWebElement` | Đọc kiểu trả về của các hàm `public` | Đạt (chỉ trả `string`, `bool`, `List<double>`, Page) |
| 4 | Locator là `private readonly By` | Đọc từng dòng khai báo | Đạt |
| 5 | Page không khai báo lại driver/wait | Đọc constructor | Đạt (chỉ gọi `base(driver)`) |
| 6 | `Tim()` thực sự chờ | Xem có `wait.Until` | Đạt |
| 7 | Không mất Assert/hành vi gốc | So 2 Assert của test gốc với test mới | Đạt (còn đủ 2 Assert, cùng điều kiện) |
| 8 | `dotnet test` giống trước refactor | Chạy trước và sau | Đạt (2/2 Passed, xem `screenshots/`) |

**Chỗ AI làm chưa đúng / chưa tốt:**

**Lỗi 1: `DaHienThi()` không bao giờ trả về `false`**
- Vị trí: `InventoryPage.cs`, hàm `DaHienThi()`.
- AI làm: `public bool DaHienThi() => Tim(tieuDeTrang).Displayed;`
- Vì sao sai: `Tim()` chờ tối đa 10 giây rồi **ném `WebDriverTimeoutException`** nếu không thấy phần tử. Do đó hàm chỉ có thể trả `true` hoặc ném lỗi, không bao giờ trả `false`. Test gốc dùng `Is.True` nên vẫn xanh, nhưng nếu viết test "đăng nhập sai thì KHÔNG vào được trang sản phẩm" với `Is.False` thì test sẽ nổ lỗi timeout thay vì fail đúng nghĩa. Kiểu trả về `bool` đang hứa nhiều hơn những gì hàm làm được.
- Cách sửa:
```csharp
public bool DaHienThi() =>
    driver.FindElements(tieuDeTrang).Any(e => e.Displayed);
```
(thêm `using System.Linq;`). Nếu muốn có chờ, dùng `try { ... } catch (WebDriverTimeoutException) { return false; }`.

**Lỗi 2: `DangNhap()` trả về `InventoryPage` ngay mà không kiểm tra đã chuyển trang**
- Vị trí: `LoginPage.cs`, hàm `DangNhap()`.
- AI làm: sau khi bấm nút đăng nhập, luôn `return new InventoryPage(driver)`.
- Vì sao sai: nếu đăng nhập thất bại, người dùng vẫn ở trang login nhưng code đã giao cho test một `InventoryPage` "giả". Lỗi chỉ lộ ra sau đó ở một hàm khác với thông báo timeout khó hiểu, thay vì báo ngay lúc đăng nhập. Đây là điểm yếu của thiết kế, không vi phạm chữ nghĩa của quy ước nên prompt chưa bắt được. Cách giảm thiểu: tách rõ `DangNhap` (kỳ vọng thành công) và `DangNhapSai` (kỳ vọng ở lại), như AI đã làm, và để test dùng đúng hàm cho từng kịch bản.

**Nhận xét thêm:** `LayDanhSachGia()` chỉ chờ 1 phần tử giá đầu tiên rồi mới `FindElements` lấy cả danh sách. Với trang saucedemo thì ổn vì cả danh sách render cùng lúc, nhưng với trang tải dần có thể lấy thiếu phần tử.

**Kết luận:** AI tuân thủ đủ 9 quy ước trong prompt (đã kiểm 8 mục ở bảng trên), nhưng vẫn có 2 điểm yếu về chất lượng thiết kế ở trên. Bài học: quy ước kiểu "có/không có" dễ kiểm bằng Ctrl+F, còn ngữ nghĩa của hàm (trả `false` khi nào, chuyển trang thật hay chưa) thì phải tự đọc và tự nghĩ ca test.
