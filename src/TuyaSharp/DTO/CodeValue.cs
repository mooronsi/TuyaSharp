using Newtonsoft.Json;

namespace TuyaSharp.DTO;

public record CodeValue
{
    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("value")]
    public object? Value { get; set; }
}
