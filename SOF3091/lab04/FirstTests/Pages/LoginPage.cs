using OpenQA.Selenium;

namespace FirstTests.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public void NhapUsername(string username)
        {
            Tim(By.Id("user-name")).SendKeys(username);
        }

        public void NhapPassword(string password)
        {
            Tim(By.Id("password")).SendKeys(password);
        }

        public void BamLogin()
        {
            Tim(By.Id("login-button")).Click();
        }
    }
}
