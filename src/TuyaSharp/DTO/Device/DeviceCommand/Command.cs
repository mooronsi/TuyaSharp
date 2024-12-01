using Newtonsoft.Json;

namespace TuyaSharp.DTO.Device.DeviceCommand;

public record Command
{
    [JsonProperty("code")]
    public required string Code { get; set; } = string.Empty;

    [JsonProperty("value")]
    public required string Value { get; set; } = default!;

    [JsonIgnore]
    public required string ValueType { get; set; } = string.Empty;
}
