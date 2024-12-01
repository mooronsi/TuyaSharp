using Newtonsoft.Json;

namespace TuyaSharp.DTO;

public record Function
{
    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("desc")]
    public string? Desc { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("values")]
    public string? Values { get; set; }
}
