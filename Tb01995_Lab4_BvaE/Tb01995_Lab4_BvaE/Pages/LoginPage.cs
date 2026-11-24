using OpenQA.Selenium;

namespace FirstTests.Pages;

public class LoginPage : BasePage
{
    private readonly By oTaiKhoan   = By.Id("user-name");
    private readonly By oMatKhau    = By.Id("password");
    private readonly By nutDangNhap = By.Id("login-button");
    private readonly By thongBaoLoi = By.CssSelector("[data-test='error']");

    public LoginPage(IWebDriver driver) : base(driver) { }

    // Chuyển trang -> InventoryPage
    public InventoryPage DangNhap(string tk, string mk)
    {
        NhapThongTin(tk, mk);
        return new InventoryPage(driver);
    }

    // Ở lại trang -> this
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
