using Newtonsoft.Json;

namespace TuyaSharp.DTO.Device.DeviceInstructions;

public record GetDeviceInstructionsResponse
{
    [JsonProperty("category")]
    public string? Category { get; set; }

    [JsonProperty("functions")]
    public Function[]? Functions { get; set; }
}
