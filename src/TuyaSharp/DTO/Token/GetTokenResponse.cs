using Newtonsoft.Json;

namespace TuyaSharp.DTO.Token;

public record GetTokenResponse
{
    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    [JsonProperty("expire_time")]
    public long? ExpireTime { get; set; }

    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonProperty("uid")]
    public string? Uid { get; set; }
}
