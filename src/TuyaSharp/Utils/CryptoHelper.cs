using System.Security.Cryptography;

namespace TuyaSharp.Utils;

internal static class CryptoHelper
{
    internal static byte[] Sha256(byte[] content) =>
        SHA256.HashData(content);

    internal static byte[] HmacSha256(byte[] key, byte[] content) =>
        new HMACSHA256(key).ComputeHash(content);
}
