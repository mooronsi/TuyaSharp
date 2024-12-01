using System.Threading;
using System.Threading.Tasks;
using TuyaSharp.DTO.Device.DeviceCommand;
using TuyaSharp.DTO.Device.DeviceInfo;
using TuyaSharp.DTO.Device.DeviceInstructions;
using TuyaSharp.Models.Requests;

namespace TuyaSharp.Abstractions;

public interface IDeviceControl
{
    Task<Response<GetDeviceInfoResponse?>> GetDeviceInfoAsync(
        GetDeviceInfoRequest request,
        CancellationToken cancellationToken = default);

    Task<Response<GetDeviceInfoBatchesResponse?>> GetDevicesInfoBatchesAsync(
        GetDeviceInfoBatchesRequest request,
        CancellationToken cancellationToken = default);

    Task<Response<GetDeviceInstructionsResponse?>> GetDeviceInstructionsAsync(
        GetDeviceInstructionsRequest request,
        CancellationToken cancellationToken = default);

    Task<Response<bool?>> SendCommandsAsync(
        SendDeviceCommandsRequest request,
        CancellationToken cancellationToken = default);
}
