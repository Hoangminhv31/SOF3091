using System.Text.Json;

namespace SauceDemo.Tests.Tests;

public class LoginTestData
{
    public string Name { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool ShouldLogin { get; set; }
    public string ExpectedError { get; set; } = "";

    public static IEnumerable<TestCaseData> GetCases()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "TestData", "login-cases.json");
        var json = File.ReadAllText(path);
        var cases = JsonSerializer.Deserialize<List<LoginTestData>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        foreach (var item in cases)
            yield return new TestCaseData(item.Username, item.Password,
                item.ShouldLogin, item.ExpectedError).SetName("Login_" + item.Name.Replace(" ", "_"));
    }
}
