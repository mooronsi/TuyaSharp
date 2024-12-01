using Newtonsoft.Json;

namespace TuyaSharp.DTO.Device.DeviceCommand;

public record SendDeviceCommandsRequest(string DeviceId, Command[] Commands);

internal record SendDeviceCommandsInnerRequest
{
    [JsonProperty("commands")]
    public Command[] Commands { get; set; } = [];
}
