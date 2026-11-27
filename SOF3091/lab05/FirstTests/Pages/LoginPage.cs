
using OpenQA.Selenium;

namespace FirstTests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By usernameInput =
            By.Id("user-name");

        private readonly By passwordInput =
            By.Id("password");

        private readonly By loginButton =
            By.Id("login-button");

        private readonly By errorMessage =
            By.CssSelector("[data-test='error']");

        private readonly By inventoryTitle =
            By.ClassName("title");

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public void DangNhap(string username, string password)
        {
            NhapDuLieu(usernameInput, username);
            NhapDuLieu(passwordInput, password);
            BamNut(loginButton);
        }

        public string LayThongBaoLoi()
        {
            return LayNoiDung(errorMessage);
        }

        public string LayTieuDeTrang()
        {
            return LayNoiDung(inventoryTitle);
        }

        public bool KiemTraDangNhapThanhCong()
        {
            try
            {
                return LayTieuDeTrang() == "Products";
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
