
using System;
using System.IO;
using System.Text.Json;

namespace FirstTests.Config
{
    public static class ConfigReader
    {
        private static TestConfig? _config;

        public static TestConfig Config
        {
            get
            {
                if (_config == null)
                {
                    string filePath = Path.Combine(
                        AppContext.BaseDirectory,
                        "Config",
                        "appsettings.json"
                    );

                    if (!File.Exists(filePath))
                    {
                        throw new FileNotFoundException(
                            "Khong tim thay appsettings.json",
                            filePath
                        );
                    }

                    string json = File.ReadAllText(filePath);

                    _config = JsonSerializer.Deserialize<TestConfig>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    ) ?? throw new InvalidOperationException(
                        "Khong doc duoc file cau hinh"
                    );

                    if (string.IsNullOrWhiteSpace(_config.BaseUrl))
                    {
                        throw new InvalidOperationException(
                            "BaseUrl khong duoc de trong"
                        );
                    }
                }

                return _config;
            }
        }
    }
}
