using System;
using TuyaSharp.DTO.Token;
using TuyaSharp.Models;

namespace TuyaSharp.Mappers;

internal static class TokenMapper
{
    internal static Token Map(this GetTokenResponse response)
    {
        var token = new Token
        {
            AccessToken = response.AccessToken
                          ?? throw new ArgumentNullException(nameof(GetTokenResponse.AccessToken)),
            RefreshToken = response.RefreshToken
                           ?? throw new ArgumentNullException(nameof(GetTokenResponse.RefreshToken)),
            Uid = response.Uid
        };

        var expireTime = response.ExpireTime
                         ?? throw new ArgumentNullException(nameof(GetTokenResponse.ExpireTime));
        token.ExpireTime = DateTimeOffset.UtcNow.AddSeconds(expireTime);

        return token;
    }

    internal static Token Map(this RefreshTokenResponse response)
    {
        var token = new Token
        {
            AccessToken = response.AccessToken
                          ?? throw new ArgumentNullException(nameof(RefreshTokenResponse.AccessToken)),
            RefreshToken = response.RefreshToken
                           ?? throw new ArgumentNullException(nameof(RefreshTokenResponse.RefreshToken)),
            Uid = response.Uid
        };

        var expireTime = response.ExpireTime
                         ?? throw new ArgumentNullException(nameof(RefreshTokenResponse.ExpireTime));
        token.ExpireTime = DateTimeOffset.UtcNow.AddSeconds(expireTime);

        return token;
    }
}
