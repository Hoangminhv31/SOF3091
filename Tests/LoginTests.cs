using FirstTests.Core;
using FirstTests.Pages;

namespace FirstTests.Tests;

// Không còn [SetUp] / [TearDown] / new ChromeDriver() — tất cả nằm ở BaseTest.
public class LoginTests : BaseTest
{
    [Test]
    public void DangNhap_DungTaiKhoan_ThanhCong()
    {
        var trangSP = new LoginPage(driver).DangNhap("standard_user", "secret_sauce");

        Assert.That(trangSP.DaHienThi(), Is.True);
    }

    [Test]
    public void DangNhap_TaiKhoanBiKhoa_HienLoi()
    {
        var loginPage = new LoginPage(driver);
        loginPage.DangNhap("locked_out_user", "secret_sauce");

        Assert.That(loginPage.LayThongBaoLoi(), Does.Contain("locked out"));
    }

    // Các ca [TestCase] của Bài 4 đã chuyển sang Data/users.json (xem DataDrivenLoginTests).
}
