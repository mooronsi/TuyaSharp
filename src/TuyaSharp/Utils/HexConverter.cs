using System;

namespace TuyaSharp.Utils;

internal static class HexConverter
{
    internal static string ToHexString(this byte[] content) =>
        Convert.ToHexString(content);

    internal static byte[] FromHexString(this string content) =>
        Convert.FromHexString(content);
}
