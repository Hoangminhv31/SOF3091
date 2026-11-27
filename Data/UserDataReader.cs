using System.Text.Json;

namespace FirstTests.Data;

/// <summary>Đọc Data/users.json. Tầng Data chỉ có 1 lý do để thay đổi: cách nạp dữ liệu.</summary>
public static class UserDataReader
{
    public static List<UserData> Load()
    {
        // AppContext.BaseDirectory = bin/Debug/netX/ — nơi CopyToOutputDirectory chép file tới.
        var path = Path.Combine(AppContext.BaseDirectory, "Data", "users.json");

        if (!File.Exists(path))
            throw new FileNotFoundException(
                $"Không thấy {path}. Kiểm tra <CopyToOutputDirectory> cho Data/users.json trong .csproj.");

        var ds = JsonSerializer.Deserialize<List<UserData>>(File.ReadAllText(path))
                 ?? throw new InvalidOperationException("users.json sai định dạng (cần một mảng JSON).");

        if (ds.Count == 0)
            throw new InvalidOperationException("users.json không có bộ dữ liệu nào.");

        // "Username": null trong JSON sẽ ghi đè giá trị mặc định -> chuẩn hoá về chuỗi rỗng.
        foreach (var u in ds)
        {
            u.Username ??= "";
            u.Password ??= "";
            u.GhiChu   ??= "";
        }
        return ds;
    }
}
