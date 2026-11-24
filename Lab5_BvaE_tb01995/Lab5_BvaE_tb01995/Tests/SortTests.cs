using FirstTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests.Tests;

public class SortTests
{
    private IWebDriver driver = null!;

    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://www.saucedemo.com");
    }

    [TearDown]
    public void TearDown() => driver.Dispose();

    [Test]
    public void SapXepGiaTangDan()
    {
        var gia = new LoginPage(driver)
            .DangNhap("standard_user", "secret_sauce")
            .SapXep("lohi")
            .LayDanhSachGia();

        Assert.That(gia, Is.Ordered);
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
