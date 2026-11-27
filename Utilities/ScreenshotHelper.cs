using OpenQA.Selenium;

namespace FirstTests.Utilities;

/// <summary>Chụp ảnh màn hình khi ca test Failed (tách ra từ Bài 3).</summary>
public static class ScreenshotHelper
{
    private const int DoDaiTenToiDa = 80;

    public static string? Chup(IWebDriver? driver)
    {
        if (driver is null) return null;

        try
        {
            var ctx    = TestContext.CurrentContext;
            var thuMuc = Path.Combine(ctx.WorkDirectory, "screenshots");
            Directory.CreateDirectory(thuMuc);

            // Tên ca test có thể chứa  " < > ( ) , ' ...  -> ký tự cấm trong tên file.
            var ten = string.Join("_", ctx.Test.Name.Split(Path.GetInvalidFileNameChars()));
            ten = ten.Replace(' ', '_');
            if (ten.Length > DoDaiTenToiDa) ten = ten[..DoDaiTenToiDa];

            var path = Path.Combine(thuMuc, $"{ten}_{DateTime.Now:HHmmss}.png");

            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(path);
            TestContext.AddTestAttachment(path);
            TestContext.Progress.WriteLine($"Ảnh lỗi: {path}");
            return path;
        }
        catch (Exception ex)
        {
            // Cơ chế hỗ trợ KHÔNG được tự gây lỗi (vd: alert đang mở -> UnhandledAlertException).
            TestContext.Progress.WriteLine($"Không chụp được ảnh: {ex.Message}");
            return null;
        }
    }
}
