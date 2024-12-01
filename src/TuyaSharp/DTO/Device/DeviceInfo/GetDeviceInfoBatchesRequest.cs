namespace TuyaSharp.DTO.Device.DeviceInfo;

public record GetDeviceInfoBatchesRequest(string[] DeviceIds, int PageNumber, int PageSize);
