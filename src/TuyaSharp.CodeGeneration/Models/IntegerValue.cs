using Newtonsoft.Json;

namespace TuyaSharp.CodeGeneration.Models;

public class IntegerValue
{
    [JsonProperty("unit")]
    public string? Unit { get; set; }

    [JsonProperty("min")]
    public int? Min { get; set; }

    [JsonProperty("max")]
    public int? Max { get; set; }

    [JsonProperty("scale")]
    public int? Scale { get; set; }

    [JsonProperty("step")]
    public int? Step { get; set; }
}
