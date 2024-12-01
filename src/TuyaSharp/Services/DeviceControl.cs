using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TuyaSharp.Abstractions;
using TuyaSharp.DTO.Device.DeviceCommand;
using TuyaSharp.DTO.Device.DeviceInfo;
using TuyaSharp.DTO.Device.DeviceInstructions;
using TuyaSharp.Models.Requests;

namespace TuyaSharp.Services;

public class DeviceControl(ITuyaSharpClient client) : IDeviceControl
{
    public Task<Response<GetDeviceInfoResponse?>> GetDeviceInfoAsync(
        GetDeviceInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        return client.SendAsync<GetDeviceInfoRequest, GetDeviceInfoResponse>(
            new Request<GetDeviceInfoRequest>
            {
                HttpMethod = HttpMethod.Get,
                Path = $"/v1.0/devices/{request.DeviceId}",
            }, cancellationToken: cancellationToken);
    }

    public Task<Response<GetDeviceInfoBatchesResponse?>> GetDevicesInfoBatchesAsync(
        GetDeviceInfoBatchesRequest request,
        CancellationToken cancellationToken = default)
    {
        return client.SendAsync<GetDeviceInfoBatchesRequest, GetDeviceInfoBatchesResponse>(
            new Request<GetDeviceInfoBatchesRequest>
            {
                HttpMethod = HttpMethod.Get,
                Path = "/v1.0/devices",
                QueryParameters = new SortedDictionary<string, string>
                {
                    { "device_ids", string.Join(",", request.DeviceIds) },
                    { "page_no", request.PageNumber.ToString() },
                    { "page_size", request.PageSize.ToString() }
                }
            }, cancellationToken: cancellationToken);
    }

    public Task<Response<GetDeviceInstructionsResponse?>> GetDeviceInstructionsAsync(
        GetDeviceInstructionsRequest request,
        CancellationToken cancellationToken = default)
    {
        return client.SendAsync<GetDeviceInstructionsRequest, GetDeviceInstructionsResponse>(
            new Request<GetDeviceInstructionsRequest>
            {
                HttpMethod = HttpMethod.Get,
                Path = $"/v1.0/iot-03/devices/{request.DeviceId}/functions",
            }, cancellationToken: cancellationToken);
    }

    public Task<Response<bool?>> SendCommandsAsync(
        SendDeviceCommandsRequest request,
        CancellationToken cancellationToken = default)
    {
        return client.SendAsync<SendDeviceCommandsInnerRequest, bool?>(
            new Request<SendDeviceCommandsInnerRequest>
            {
                HttpMethod = HttpMethod.Post,
                Path = $"/v1.0/devices/{request.DeviceId}/commands",
                Body = new SendDeviceCommandsInnerRequest
                {
                    Commands = request.Commands
                }
            }, cancellationToken: cancellationToken);
    }
}
