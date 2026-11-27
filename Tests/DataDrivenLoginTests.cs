using FirstTests.Core;
using FirstTests.Data;
using FirstTests.Pages;

namespace FirstTests.Tests;

public class DataDrivenLoginTests : BaseTest
{
    // BẮT BUỘC static: NUnit gọi hàm này lúc DÒ test, trước khi tạo đối tượng của class.
    private static IEnumerable<TestCaseData> TuJson()
    {
        var i = 0;
        foreach (var u in UserDataReader.Load())
        {
            i++;
            // Chỉ số i đảm bảo tên DUY NHẤT (2 bộ cùng Username rỗng vẫn không trùng).
            yield return new TestCaseData(u.Username, u.Password, u.ThanhCong, u.GhiChu)
                .SetName($"DangNhap_{i:00}_{TenNgan(u.Username)}")
                .SetDescription(u.GhiChu);
        }
    }

    /// <summary>Rút Username thành chuỗi ngắn, an toàn để làm tên ca test / tên file ảnh.</summary>
    private static string TenNgan(string username)
    {
        if (username.Length == 0)                  return "trong";
        if (string.IsNullOrWhiteSpace(username))   return "khoang_trang";

        var sach = new string(username.Trim()
                       .Select(c => char.IsLetterOrDigit(c) || c == '_' ? c : '_')
                       .ToArray());

        if (sach.Length > 20) sach = $"{sach[..20]}_dai{username.Length}";   // chuỗi rất dài
        if (username != username.Trim()) sach += "_thua_cach";                // khoảng trắng đầu/cuối
        return sach;
    }

    [TestCaseSource(nameof(TuJson))]   // nameof: gõ sai tên là trình biên dịch báo ngay
    public void DangNhap_TuFileJson(string tk, string mk, bool mongDoiThanhCong, string ghiChu)
    {
        var trangSP = new LoginPage(driver).DangNhap(tk, mk);

        Assert.That(trangSP.DaHienThi(), Is.EqualTo(mongDoiThanhCong),
            $"Kỳ vọng ThanhCong={mongDoiThanhCong} — {ghiChu}");
    }
}
