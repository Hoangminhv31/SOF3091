namespace FirstTests.Utilities;

public static class Log
{
    public static void Info(string msg) =>
        TestContext.Progress.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [INFO]  {msg}");

    public static void Error(string msg) =>
        TestContext.Progress.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] {msg}");
}