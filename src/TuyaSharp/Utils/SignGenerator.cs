using System.Text;

namespace TuyaSharp.Utils;

internal static class SignGenerator
{
    internal static string Generate(
        string clientId,
        string clientSecret,
        string timestamp,
        string nonce,
        string stringToSign,
        string? accessToken = null)
    {
        var sb = new StringBuilder();

        sb.Append(clientId);
        if (!string.IsNullOrEmpty(accessToken)) sb.Append(accessToken);
        sb.Append(timestamp);
        sb.Append(nonce);
        sb.Append(stringToSign);

        return CryptoHelper
            .HmacSha256(Encoding.UTF8.GetBytes(clientSecret), Encoding.UTF8.GetBytes(sb.ToString()))
            .ToHexString();
    }
}
