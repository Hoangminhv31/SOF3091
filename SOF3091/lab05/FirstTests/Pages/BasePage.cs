
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using FirstTests.Config;

namespace FirstTests.Pages
{
    public class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;

            Wait = new WebDriverWait(
                Driver,
                TimeSpan.FromSeconds(ConfigReader.Config.TimeoutSeconds)
            );
        }

        public void MoTrangChu()
        {
            Driver.Navigate().GoToUrl(ConfigReader.Config.BaseUrl);
        }

        protected IWebElement TimPhanTu(By locator)
        {
            return Wait.Until(driver =>
            {
                try
                {
                    IWebElement element = driver.FindElement(locator);

                    return element.Displayed ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
        }

        protected void NhapDuLieu(By locator, string value)
        {
            IWebElement element = TimPhanTu(locator);

            element.Clear();
            element.SendKeys(value);
        }

        protected void BamNut(By locator)
        {
            TimPhanTu(locator).Click();
        }

        protected string LayNoiDung(By locator)
        {
            return TimPhanTu(locator).Text;
        }
    }
}
