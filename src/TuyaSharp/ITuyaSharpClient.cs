using System.Threading;
using System.Threading.Tasks;
using TuyaSharp.Abstractions;
using TuyaSharp.Models.Requests;

namespace TuyaSharp;

public interface ITuyaSharpClient
{
    CancellationToken CancellationToken { get; }

    ITokenService TokenService { get; }
    IDeviceControl DeviceControl { get; }

    Task<Response<TResponseBody?>> SendAsync<TRequestBody, TResponseBody>(
        Request<TRequestBody> request,
        bool withoutToken = false,
        CancellationToken cancellationToken = default);
}
