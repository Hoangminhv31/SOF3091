using System.Text.Json;

namespace SauceDemo.Tests.Config;

public class TestSettings
{
    public string Url { get; set; } = "https://www.saucedemo.com/";
    public string Browser { get; set; } = "Chrome";
    public int Timeout { get; set; } = 10;
    public bool Headless { get; set; } = true;

    public static TestSettings Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<TestSettings>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}
