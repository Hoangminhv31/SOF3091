using FirstTests.Config;
using FirstTests.Utilities;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace FirstTests.Core;

/// <summary>
/// Lớp cha của MỌI file test. Gom toàn bộ vòng đời trình duyệt:
///   SetUp   : đọc config -> tạo driver -> mở BaseUrl
///   TearDown: (try) chụp ảnh nếu Failed -> (finally) đóng trình duyệt
/// File test con KHÔNG được có [SetUp], [TearDown], new ChromeDriver().
/// </summary>
public abstract class BaseTest
{
    protected IWebDriver driver = null!;
    protected TestConfig cfg    = null!;

    [SetUp]
    public void BaseSetup()
    {
        cfg    = ConfigReader.Load();
        driver = DriverFactory.Create(cfg);      // trả IWebDriver -> không biết là Chrome hay Firefox

        try
        {
            // Không Maximize(): kích thước đã đặt tường minh trong DriverFactory.
            driver.Navigate().GoToUrl(cfg.BaseUrl);
        }
        catch
        {
            // Mở URL lỗi (mất mạng, URL sai...) thì driver đã sinh ra rồi -> phải đóng,
            // nếu không trình duyệt sẽ chạy ngầm mãi.
            DongTrinhDuyet();
            throw;
        }
    }

    [TearDown]
    public void BaseTearDown()
    {
        try
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                ScreenshotHelper.Chup(driver);
        }
        finally
        {
            // finally: chụp ảnh lỗi dù có hỏng cũng KHÔNG được ngăn việc đóng trình duyệt.
            DongTrinhDuyet();
        }
    }

    private void DongTrinhDuyet()
    {
        if (driver is null) return;

        try
        {
            driver.Quit();
        }
        catch (Exception ex)
        {
            TestContext.Progress.WriteLine($"Quit() lỗi: {ex.Message}");
        }
        finally
        {
            driver.Dispose();
            driver = null!;
        }
    }
}
