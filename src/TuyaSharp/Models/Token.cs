using System;

namespace TuyaSharp.Models;

public class Token
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset ExpireTime { get; set; }
    public string? Uid { get; set; }
    public bool IsExpired => DateTimeOffset.UtcNow > ExpireTime;
}
