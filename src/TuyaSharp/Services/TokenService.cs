using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TuyaSharp.Abstractions;
using TuyaSharp.DTO.Token;
using TuyaSharp.Models.Requests;

namespace TuyaSharp.Services;

public class TokenService(ITuyaSharpClient client) : ITokenService
{
    public Task<Response<GetTokenResponse?>> GetTokenAsync(
        GetTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        return client.SendAsync<GetTokenRequest, GetTokenResponse>(new Request<GetTokenRequest>
        {
            HttpMethod = HttpMethod.Get,
            Path = "/v1.0/token",
            QueryParameters = new SortedDictionary<string, string>
            {
                { "grant_type", "1" }
            }
        }, true, cancellationToken);
    }

    public Task<Response<RefreshTokenResponse?>> RefreshTokenAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
            throw new ArgumentException("Refresh token is required.", nameof(request.RefreshToken));

        return client.SendAsync<RefreshTokenRequest, RefreshTokenResponse>(new Request<RefreshTokenRequest>
        {
            HttpMethod = HttpMethod.Get,
            Path = "/v1.0/token/" + request.RefreshToken,
        }, false, cancellationToken);
    }
}
