using Newtonsoft.Json;

namespace TuyaSharp.CodeGeneration.Models;

public class EnumValue
{
    [JsonProperty("range")]
    public string[]? Range { get; set; }
}
