
namespace FirstTests.Config
{
    public class TestConfig
    {
        public string BaseUrl { get; set; } = "";
        public string Browser { get; set; } = "Chrome";
        public int TimeoutSeconds { get; set; } = 10;
        public bool Headless { get; set; } = false;
        public int WindowWidth { get; set; } = 1366;
        public int WindowHeight { get; set; } = 768;
    }
}
