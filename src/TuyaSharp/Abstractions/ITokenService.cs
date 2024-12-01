using System.Threading;
using System.Threading.Tasks;
using TuyaSharp.DTO.Token;
using TuyaSharp.Models.Requests;

namespace TuyaSharp.Abstractions;

public interface ITokenService
{
    Task<Response<GetTokenResponse?>> GetTokenAsync(
        GetTokenRequest request,
        CancellationToken cancellationToken = default);

    Task<Response<RefreshTokenResponse?>> RefreshTokenAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default);
}
