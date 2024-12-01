using TuyaSharp.DTO.Token;
using TuyaSharp.Models;

namespace TuyaSharp.Validators;

internal static class TokenValidator
{
    internal static ValidationResult Validate(this GetTokenResponse response)
    {
        if (string.IsNullOrEmpty(response.AccessToken))
        {
            return ValidationResult.Fail($"{nameof(GetTokenResponse.AccessToken)} cannot be null.");
        }

        if (string.IsNullOrEmpty(response.RefreshToken))
        {
            return ValidationResult.Fail($"{nameof(GetTokenResponse.RefreshToken)} cannot be null.");
        }

        if (response.ExpireTime == null)
        {
            return ValidationResult.Fail($"{nameof(GetTokenResponse.ExpireTime)} cannot be null.");
        }

        return ValidationResult.Ok;
    }
}
