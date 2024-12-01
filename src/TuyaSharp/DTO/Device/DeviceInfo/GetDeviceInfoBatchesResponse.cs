using Newtonsoft.Json;

namespace TuyaSharp.DTO.Device.DeviceInfo;

public record GetDeviceInfoBatchesResponse
{
    [JsonProperty("devices")]
    public GetDeviceInfoResponse[]? Devices { get; set; }

    [JsonProperty("last_id")]
    public string? LastId { get; set; }

    [JsonProperty("total")]
    public int? Total { get; set; }
}
