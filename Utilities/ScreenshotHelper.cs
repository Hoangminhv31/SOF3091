using OpenQA.Selenium;

namespace FirstTests.Utilities;

public static class ScreenshotHelper
{
    public static void Chup(IWebDriver? driver)
    {
        if (driver is null) return;

        try
        {
            var ctx = TestContext.CurrentContext;
            DirectoryInfo? projectDir = new(AppContext.BaseDirectory);
            while (projectDir is not null && !File.Exists(Path.Combine(projectDir.FullName, "FirstTests.csproj")))
                projectDir = projectDir.Parent;

            var thuMuc = Path.Combine(projectDir?.FullName ?? ctx.WorkDirectory, "screenshots");
            Directory.CreateDirectory(thuMuc);

            // Tên ca test do [TestCase] tự sinh chứa  " , ( )  -> KÝ TỰ CẤM trong tên file.
            // Không lọc là dính ArgumentException, che mất lỗi thật.
            var ten  = string.Join("_", ctx.Test.Name.Split(Path.GetInvalidFileNameChars()));
            var path = Path.Combine(thuMuc, $"{ten}_{DateTime.Now:yyyyMMdd_HHmmss_fff}.png");

            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(path);
            TestContext.AddTestAttachment(path);
            TestContext.Progress.WriteLine($"Ảnh lỗi: {path}");
        }
        catch (Exception ex)
        {
            // Nuốt lỗi: ví dụ có alert đang mở thì GetScreenshot() ném UnhandledAlertException (Bài 3).
            // Cơ chế hỗ trợ mà tự gây lỗi thì còn tệ hơn không có.
            TestContext.Progress.WriteLine($"Không chụp được ảnh: {ex.Message}");
        }
    }
}