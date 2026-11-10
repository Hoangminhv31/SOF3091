using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FirstTests
{
    [TestFixture]
    public class AlertIframeTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string BaseUrl = "https://the-internet.herokuapp.com";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // ---------- Hàm hỗ trợ ----------

        private void MoTrangAlert() => driver.Navigate().GoToUrl(BaseUrl + "/javascript_alerts");

        private void BamNut(string tenNut) =>
            driver.FindElement(By.XPath($"//button[text()='{tenNut}']")).Click();

        // Chờ alert xuất hiện rồi trả về IAlert
        private IAlert ChoAlert() => wait.Until(d =>
        {
            try { return d.SwitchTo().Alert(); }
            catch (NoAlertPresentException) { return null; }
        });

        private string LayKetQua() => driver.FindElement(By.Id("result")).Text;

        // ---------- Ca 1: JS Alert -> Accept ----------
        [Test]
        public void Test_JS_Alert_Accept()
        {
            MoTrangAlert();
            BamNut("Click for JS Alert");

            IAlert alert = ChoAlert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"), "Nội dung alert không đúng");
            alert.Accept();

            Assert.That(LayKetQua(), Is.EqualTo("You successfully clicked an alert"), "Kết quả #result không đúng");
        }

        // ---------- Ca 2: JS Confirm -> Dismiss ----------
        [Test]
        public void Test_JS_Confirm_Dismiss()
        {
            MoTrangAlert();
            BamNut("Click for JS Confirm");

            IAlert alert = ChoAlert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"), "Nội dung confirm không đúng");
            alert.Dismiss();

            Assert.That(LayKetQua(), Is.EqualTo("You clicked: Cancel"), "Kết quả #result không đúng");
        }

        // ---------- Ca 3: JS Prompt -> SendKeys + Accept ----------
        [Test]
        public void Test_JS_Prompt_SendKeys_Accept()
        {
            string noiDung = "Xin chao Selenium";

            MoTrangAlert();
            BamNut("Click for JS Prompt");

            IAlert alert = ChoAlert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS prompt"), "Nội dung prompt không đúng");
            alert.SendKeys(noiDung);
            alert.Accept();

            Assert.That(LayKetQua(), Is.EqualTo("You entered: " + noiDung), "Kết quả #result không đúng");
        }

        // ---------- Ca 4: iframe (TinyMCE) ----------
        [Test]
        public void Test_Iframe_Go_Chu_Roi_Quay_Lai()
        {
            string noiDung = "Lab 3 - Selenium iframe test";

            driver.Navigate().GoToUrl(BaseUrl + "/iframe");

            // Chuyển vào iframe
            IWebElement frame = wait.Until(d => d.FindElement(By.Id("mce_0_ifr")));
            driver.SwitchTo().Frame(frame);

            // Trong iframe: <body id="tinymce" contenteditable="true">
            IWebElement editor = wait.Until(d => d.FindElement(By.Id("tinymce")));
            editor.Clear();
            editor.SendKeys(noiDung);
            Assert.That(editor.Text, Is.EqualTo(noiDung), "Chữ trong editor không đúng");

            // Quay lại trang chính
            driver.SwitchTo().DefaultContent();

            // Assert đã thoát iframe: tìm được tiêu đề ở trang chính
            string tieuDe = driver.FindElement(By.TagName("h3")).Text;
            Assert.That(tieuDe, Does.Contain("iFrame"), "Chưa quay lại được trang chính");
        }

        [TearDown]
        public void Teardown()
        {
            if (driver == null) return;

            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    string folder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "screenshots"));
                    Directory.CreateDirectory(folder);

                    string fileName = TestContext.CurrentContext.Test.Name + ".png";
                    string path = Path.Combine(folder, fileName);

                    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(path);
                    TestContext.AddTestAttachment(path);
                }
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}